// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

#include "pch.h"

BOOL APIENTRY DllMain(HMODULE module, DWORD  callReason, LPVOID reserved)
{
    switch (callReason)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
