// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

#include "pch.h"
#include <wtypes.h>
#include <dshow.h>
#include <string>
#include "MahjongCVCore.h"

using namespace Microsoft::WRL;

HRESULT EnumSingleVideoInputDevice(IPropertyBag* propBag, EnumVideoInputDevicesCallback callback)
{
    VARIANT var;
    VariantInit(&var);

    // Get the device path and reset the variant for the next step.
    RETURN_IF_FAILED(propBag->Read(L"DevicePath", &var, 0));
    RETURN_HR_IF(E_UNEXPECTED, (var.vt != VT_BSTR));
    RETURN_HR_IF(E_UNEXPECTED, (var.bstrVal == nullptr));

    std::wstring pathStr(var.bstrVal, SysStringLen(var.bstrVal));
    VariantClear(&var);

    // Get the readable name for the camera.
    std::wstring nameStr;
    if (SUCCEEDED(propBag->Read(L"Description", &var, 0)) ||
        SUCCEEDED(propBag->Read(L"FriendlyName", &var, 0)))
    {
        if (var.vt == VT_BSTR)
        {
            nameStr = std::wstring(var.bstrVal, SysStringLen(var.bstrVal));
        }
    }

    callback(pathStr.data(), ((nameStr.length() != 0) ? nameStr.data() : nullptr));
    return S_OK;
}

extern "C" HRESULT EnumVideoInputDevices(EnumVideoInputDevicesCallback callback)
{
    ComPtr<ICreateDevEnum> devEnum;
    RETURN_IF_FAILED(CoCreateInstance(CLSID_SystemDeviceEnum, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&devEnum)));

    ComPtr<IEnumMoniker> monikerEnum;
    RETURN_IF_FAILED(devEnum->CreateClassEnumerator(CLSID_VideoInputDeviceCategory, &monikerEnum, 0));

    ComPtr<IMoniker> moniker;
    while ((monikerEnum != nullptr) && monikerEnum->Next(1, &moniker, nullptr) == S_OK)
    {
        ComPtr<IPropertyBag> propBag;
        if (SUCCEEDED(moniker->BindToStorage(0, 0, IID_PPV_ARGS(&propBag))))
        {
            EnumSingleVideoInputDevice(propBag.Get(), callback);
        }
    }
    return S_OK;
}