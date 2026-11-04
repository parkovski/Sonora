#include "sonora.h"
#include "log.h"
#include <cstddef>
#include <cstring>
#include <functional>
#include <unordered_map>
#include <mutex>
#include <libremidi/libremidi.hpp>
#include <libremidi/backends/winmidi/config.hpp>

namespace sonora {

enum class MidiDirection {
  In,
  Out,
};

struct MidiPortInfo {
  char *name;
  char *id;
  uint64_t number;
  const libremidi::port_information *port;
  MidiDirection direction;

  explicit MidiPortInfo(const libremidi::port_information &port_info,
                        MidiDirection direction)
    : name{new char[port_info.port_name.length() + 1]}
    , id{nullptr}
    , number{port_info.port}
    , port{new libremidi::port_information{port_info}}
    , direction{direction}
  {
    memcpy(name, port_info.port_name.data(),
           port_info.port_name.length());
    name[port_info.port_name.length()] = 0;

    switch (port_info.api) {
    case ALSA_SEQ:
    case ALSA_SEQ_UMP:
    case ALSA_RAW:
    case ALSA_RAW_UMP:
    case COREMIDI:
    case COREMIDI_UMP:
    case WINDOWS_MIDI_SERVICES:
      {
        // long long max value is 19 chars long.
        char port_num[20] = { 0 };
        sprintf(port_num, "%lld", (long long)port_info.port);
        size_t port_num_length = strlen(port_num);
        size_t length, cur_length = 0;
        length = port_info.manufacturer.length()
          + 1 + port_info.product.length()
          + 1 + port_info.device_name.length()
          + 1 + port_info.port_name.length()
          + 1 + port_num_length;
        id = new char[length + 1];
        if (port_info.manufacturer.length()) {
          memcpy(id, port_info.manufacturer.data(),
                 port_info.manufacturer.length());
          cur_length += port_info.manufacturer.length();
          id[cur_length++] = ' ';
        }
        if (port_info.product.length()) {
          memcpy(id + cur_length, port_info.product.data(),
                 port_info.product.length());
          cur_length += port_info.product.length();
          id[cur_length++] = ' ';
        }
        memcpy(id + cur_length, port_info.device_name.data(),
               port_info.device_name.length());
        id[cur_length++] = ' ';
        memcpy(id + cur_length, port_info.port_name.data(),
               port_info.port_name.length());
        id[cur_length++] = ' ';
        memcpy(id + cur_length, port_num, port_num_length);
        cur_length += port_num_length;
        id[cur_length] = 0;
      }
      break;

    case JACK_MIDI:
    case JACK_UMP:
    case WINDOWS_UWP:
    case WEBMIDI:
    case PIPEWIRE:
    case PIPEWIRE_UMP:
    case ANDROID_AMIDI:
    default:
      {
        // long long max value is 19 chars long.
        char port_num[20] = { 0 };
        sprintf(port_num, "%lld", (long long)port_info.port);
        size_t port_num_length = strlen(port_num);
        size_t length =
          port_info.port_name.length()
          + 1 + port_num_length;
        id = new char[length + 1];
        memcpy(id, port_info.port_name.data(),
               port_info.port_name.length());
        id[port_info.port_name.length()] = ' ';
        memcpy(id + port_info.port_name.length() + 1, port_num,
               port_num_length);
        id[length] = 0;
      }
      break;

    case KDMAPI:
    case WINDOWS_MM:
      {
        // long long max value is 19 chars long.
        char port_num[20] = { 0 };
        sprintf(port_num, "%lld", (long long)port_info.port);
        size_t port_num_length = strlen(port_num);
        size_t length =
          port_info.display_name.length()
          + 1 + port_num_length;
        id = new char[length + 1];
        memcpy(id, port_info.display_name.data(),
               port_info.display_name.length());
        id[port_info.display_name.length()] = ' ';
        memcpy(id + port_info.display_name.length() + 1, port_num,
               port_num_length);
        id[length] = 0;
      }
      break;
    }

    log::trace("Created MIDI port info for {} {} ({}).",
               direction == MidiDirection::In ? "input" : "output", name, id);
  }

  ~MidiPortInfo() {
    delete[] name;
    delete[] id;
    delete port;
  }
};

struct StringHash {
  using is_transparent = void;

  size_t operator()(const char *s) const noexcept {
    return std::hash<std::string_view>{}(std::string_view{s});
  }

  size_t operator()(std::string_view s) const noexcept {
    return std::hash<std::string_view>{}(s);
  }

  size_t operator()(const std::string &s) const noexcept {
    return std::hash<std::string>{}(s);
  }
};

struct MidiState {
  void *receiver;
  void (*port_added)(void *receiver, const MidiPortInfo &info);
  void (*port_removed)(void *receiver, const MidiPortInfo &info);
  libremidi::observer observer;
  libremidi::midi_in midi_in;
  libremidi::midi_out midi_out;
  std::mutex _mutex;
  std::unordered_map<std::string, libremidi::input_port,
                     StringHash, std::equal_to<>> _inputs;
  std::unordered_map<std::string, libremidi::output_port,
                     StringHash, std::equal_to<>> _outputs;

  libremidi::observer_configuration get_observer_config() {
    return {
      .input_added = [this] (const libremidi::input_port &port) {
        std::lock_guard lock{_mutex};
        MidiPortInfo info{port, MidiDirection::In};
        _inputs.emplace(info.id, port);
        if (port_added) {
          port_added(receiver, info);
        }
      },
      .input_removed = [this] (const libremidi::input_port &port) {
        std::lock_guard lock{_mutex};
        MidiPortInfo info{port, MidiDirection::In};
        auto it = _inputs.find(info.id);
        if (it != _inputs.end()) {
          _inputs.erase(it);
        }
        if (port_removed) {
          port_removed(receiver, info);
        }
      },
      .output_added = [this] (const libremidi::output_port &port) {
        std::lock_guard lock{_mutex};
        MidiPortInfo info{port, MidiDirection::Out};
        _outputs.emplace(info.id, port);
        if (port_added) {
          port_added(receiver, info);
        }
      },
      .output_removed = [this] (const libremidi::output_port &port) {
        std::lock_guard lock{_mutex};
        MidiPortInfo info{port, MidiDirection::Out};
        auto it = _outputs.find(info.id);
        if (it != _outputs.end()) {
          _outputs.erase(it);
        }
        if (port_removed) {
          port_removed(receiver, info);
        }
      }
    };
  }

  void midi_in_callback(const libremidi::ump &message) {
  }

  explicit MidiState(
    void *receiver,
    void (*port_added)(void *receiver, const MidiPortInfo &info),
    void (*port_removed)(void *receiver, const MidiPortInfo &info)
  )
    : receiver{receiver}
    , port_added{port_added}
    , port_removed{port_removed}
    , observer{}
    , midi_in{libremidi::ump_input_configuration{
      .on_message = std::bind(&MidiState::midi_in_callback, this,
                              std::placeholders::_1),
    }}
    , midi_out{}
  {}
};

} // namespace sonora

using namespace sonora;

SNRAPI SNREXPORT MidiState *SnrMidiStateNew(
  void *receiver,
  void (*port_added)(void *receiver, const MidiPortInfo &info),
  void (*port_removed)(void *receiver, const MidiPortInfo &info)
) {
  return new MidiState{receiver, port_added, port_removed};
}

SNRAPI SNREXPORT void SnrMidiStateFree(MidiState *state) {
  delete state;
}

SNRAPI SNREXPORT void SnrMidiStateListen(MidiState *state) {
  libremidi::observer_api_configuration api_config =
    libremidi::midi1::default_api();
  state->observer = libremidi::observer{
    state->get_observer_config(),
    api_config
  };
}

SNRAPI SNREXPORT void SnrMidiStateOpenPort(MidiState *state,
                                           const MidiPortInfo &info) {
}
