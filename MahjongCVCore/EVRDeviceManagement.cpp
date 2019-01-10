// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

#include "pch.h"
#include "MahjongCVCore.h"
#include "EVRDevice.h"

extern "C" HRESULT InitializeEVR()   { return EVRDevice::EnsureInitialized(); }
extern "C" HRESULT UninitializeEVR() { return EVRDevice::EnsureUninitialized(); }
