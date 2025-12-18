#pragma once

#ifdef __cplusplus
extern "C" {
#endif

/// 直接获取 IDFA（未授权 / 不可用时返回 NULL）
const char* DeviceIDFA(void);

/// 获取 IDFV（始终可用，除非极端情况）
const char* DeviceIDFV(void);

const char* DeviceId();
/// 释放 Unity 拿到的字符串
void Device_Free(const char* ptr);

#ifdef __cplusplus
}
#endif
