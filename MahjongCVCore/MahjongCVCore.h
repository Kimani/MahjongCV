// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

// Callers will need to ensure COM is initialized on the threads calling into this DLL.
// Interop callers from C# will get this for free without any needed explicit action.

#pragma once

typedef void (__stdcall *EnumVideoInputDevicesCallback) (PCWSTR path, _In_opt_ PCWSTR name);

extern "C" HRESULT EnumVideoInputDevices(EnumVideoInputDevicesCallback callback);
