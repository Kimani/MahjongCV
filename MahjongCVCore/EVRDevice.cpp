// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

#include "pch.h"
#include <uuids.h>
#include <mfapi.h>
#include "EVRDevice.h"

using namespace Microsoft::WRL;

ComPtr<EVRDevice> EVRDevice::g_sharedInstance;

// EVRDevice
HRESULT EVRDevice::RuntimeClassInitialize()
{
    //RETURN_IF_FAILED(CoCreateInstance(CLSID_EnhancedVideoRenderer, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&m_evrDevice)));

    // Initialize Media Foundation
    RETURN_IF_FAILED(MakeAndInitialize<MFInitHelper>(&m_mfInitHelper));
    

    return S_OK;
}

EVRDevice::~EVRDevice()
{

}

// Statics
HRESULT EVRDevice::GetInstance(REFIID riid, void** ppv) { return g_sharedInstance.CopyTo(riid, ppv); }

HRESULT EVRDevice::EnsureInitialized()
{
    if (g_sharedInstance.Get() == nullptr) { RETURN_IF_FAILED(MakeAndInitialize<EVRDevice>(&g_sharedInstance)); }
    return S_OK;
}

HRESULT EVRDevice::EnsureUninitialized()
{
    g_sharedInstance = nullptr;
    return S_OK;
}
