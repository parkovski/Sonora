#define _CRT_SECURE_NO_WARNINGS
#include <portmidi.h>
#include <cstdlib>
#include <cstdio>
#include <string>
#include <iostream>
#include <thread>
#include "../src/hostinterface.h"

void midi_read(PmStream *stream, bool *stop) {
  using namespace sonora::host;
  PMAContext ctx = SnrAudioContextNew();
  DeviceEnumerator *devEnum = SnrDeviceEnumeratorNew(ctx);
  PMADevice dev = SnrDeviceNew(ctx, devEnum, true, 2, 2, 44100);
  SnrDeviceEnumeratorFree(devEnum);
  SnrDeviceStart(dev);

  PmEvent buf[100];
  while (!*stop) {
    auto r = Pm_Read(stream, buf, sizeof(buf)/sizeof(buf[0]));
    if (r == 0) continue;
    if (r >= 0) {
      printf("read %d events\n", r);
    } else {
      printf("Pm_Read error %d\n", r);
    }
    for (int i = 0; i < r; ++i) {
      int status = Pm_MessageStatus(buf[i].message);
      if (status == 0x90) {
        printf("Note on: %d\n", Pm_MessageData1(buf[i].message));
        SnrDeviceSetNote(dev, Pm_MessageData1(buf[i].message));
      } else if (status == 0x80) {
        printf("Note off: %d\n", Pm_MessageData1(buf[i].message));
      }
    }
  }

  SnrDeviceFree(dev);
  SnrAudioContextFree(ctx);
}

int main() {
  Pm_Initialize();

  int devices = Pm_CountDevices();
  printf("Found %d devices\n", devices);

  for (int i = 0; i < devices; ++i) {
    auto *di = Pm_GetDeviceInfo(i);
    printf("%d: %s%s %s\n", i, di->input ? "in" : "", di->output ? "out" : "",
           di->name);
  }
  std::cin >> devices;
  printf("open device %d\n", devices);

  PmStream *stream;
  if (Pm_OpenInput(&stream, devices, NULL, 100, NULL, NULL) != pmNoError) {
    printf("Error\n");
  }

  bool stop = false;
  std::thread thread([&stop, stream]{ midi_read(stream, &stop); });

  printf("press enter to stop.\n");
  std::cin >> devices;
  std::string line;
  getline(std::cin, line);
  stop = true;
  thread.join();

  Pm_Close(stream);

  Pm_Terminate();
  return 0;
}
