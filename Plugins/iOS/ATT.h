#pragma once
#include <stdbool.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef void (*ATTCallback)(int status);

// 注册 Unity 回调
void ATT_SetCallback(ATTCallback cb);

// 主动请求 ATT
void ATT_Request(void);

// 查询状态
bool ATT_IsAuthorized(void);

#ifdef __cplusplus
}
#endif
