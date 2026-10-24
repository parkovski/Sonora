#pragma once

#ifdef SONORA_DLL
# ifdef _MSC_VER
#   define SNREXPORT __declspec(dllexport)
# else
#   define SNREXPORT __attribute__((visibility("default")))
# endif
#else
# define SNREXPORT
#endif
