// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

#pragma once

#define WIN32_LEAN_AND_MEAN

#include <SDKDDKVer.h>
#include <windows.h>
#include <wrl/client.h>

#define RETURN_IF_FAILED(x)      do { HRESULT stat = (x); if (FAILED(stat)) return stat;    } while(0)
#define RETURN_HR_IF(hrMacro, x) do { bool    stat = (x); if (x)            return hrMacro; } while(0)
