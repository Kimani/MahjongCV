// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2019

#pragma once

#include "ResourceHelpers.h"

class EVRDevice WrlFinal : public Microsoft::WRL::RuntimeClass<
    Microsoft::WRL::RuntimeClassFlags<Microsoft::WRL::RuntimeClassType::ClassicCom>,
    Microsoft::WRL::FtmBase>
{
public:
    static HRESULT EnsureInitialized();
    static HRESULT EnsureUninitialized();
    static HRESULT GetInstance(REFIID riid, void** ppv);

    ~EVRDevice();
    HRESULT RuntimeClassInitialize();
 
private:
    static Microsoft::WRL::ComPtr<EVRDevice> g_sharedInstance;

    Microsoft::WRL::ComPtr<IUnknown>     m_evrDevice;
    Microsoft::WRL::ComPtr<MFInitHelper> m_mfInitHelper;
};