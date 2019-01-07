// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

#include "pch.h"
#include <wrl.h>
#include <shtypes.h>
#include <initguid.h>

#pragma comment(lib,"runtimeobject")

using namespace Microsoft::WRL;

STDAPI DllGetActivationFactory(_In_ HSTRING activatableClassId, _COM_Outptr_ IActivationFactory** factory)
{
    return Module<ModuleType::InProc>::GetModule().GetActivationFactory(activatableClassId, factory);
}

__control_entrypoint(DllExport)
STDAPI DllCanUnloadNow()
{
    return (Module<ModuleType::InProc>::GetModule().GetObjectCount() == 0) ? S_OK : S_FALSE;
}

STDAPI_(BOOL) DllMain(HINSTANCE instance, DWORD reason, void* /*reserved*/)
{
    if (reason == DLL_PROCESS_ATTACH)
    {
        DisableThreadLibraryCalls(instance);
    }
    return TRUE;
}

STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, void** ppv)
{
    return Module<ModuleType::InProc>::GetModule().GetClassObject(rclsid, riid, ppv);
}
