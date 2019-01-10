// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2019

#pragma once

#include <mfapi.h>

class MFInitHelper WrlFinal : public Microsoft::WRL::RuntimeClass<
    Microsoft::WRL::RuntimeClassFlags<Microsoft::WRL::RuntimeClassType::ClassicCom>, 
    Microsoft::WRL::FtmBase>
{
public:
    HRESULT RuntimeClassInitialize()
    {
        RETURN_IF_FAILED(MFStartup(MF_VERSION, MFSTARTUP_LITE));
        return S_OK;
    }
    
    ~MFInitHelper()
    {
        MFShutdown();
    }
};