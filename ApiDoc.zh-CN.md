# API文档

## 1. 介绍

### 返回参数基础结构
```csharp
public class DifyApiResult<T>
{
    public int? Code { get; set; } // 此参数中为dify api返回状态码, 可能为空
    public bool? Success { get; set; } // 使用此参数判断本次请求是否成功
    public string Message { get; set; } // 此参数中为dify api返回的消息或发生异常抛出的错误信息
    public T Data { get; set; } // 此参数中, 参照dify api文档中返回的数据结构
}
```

### 传入参数基础概念
- `overrideApiKey`: 支持在调用时传入覆盖配置文件中的BotApiKey, 用于多个Bot的场景

## 2. 已实现的API接口

### 发送对话消息
- 方法名称: CreateChatCompletionBlockModeAsync
- 返回参数: DifyApiResult<Dify_CreateChatCompletionResDto>

### 停止响应
- 方法名称: StopChatCompletionAsync
- 返回参数: DifyApiResult<Dify_StopChatCompletionResDto>

### 消息反馈（点赞）
- 方法名称: CreateFeedbackAsync
- 返回参数: DifyApiResult<Dify_CreateFeedbackResDto>

### 获取下一轮建议问题列表
- 方法名称: GetSuggestionsAsync
- 返回参数: DifyApiResult<Dify_BaseRequestResDto<List<string>>>

### 获取对话历史消息
- 方法名称: GetConversationHistoryMessageAsync
- 返回参数: DifyApiResult<Dify_GetConversationHistoryMessageResDto>

### 获取对话列表
- 方法名称: GetConversationListAsync
- 返回参数: DifyApiResult<Dify_GetConversationListResDto>

### 删除对话
- 方法名称: DeleteConversationAsync
- 无返回参数

### 重命名对话
- 方法名称: RenameConversationAsync
- 返回参数: DifyApiResult<Dify_RenameConversationResDto>

### 音频转文本
- 方法名称: AudioToTextAsync
- 返回参数: DifyApiResult<Dify_AudioToTextResDto>

### 获取应用信息
- 方法名称: GetApplicationInfoAsync
- 返回参数: DifyApiResult<Dify_GetApplicationInfoResDto>

### 获取应用元数据
- 方法名称: GetApplicationMetaAsync
- 返回参数: DifyApiResult<Dify_GetApplicationMetaResDto>

### 上传文件
- 方法名称: FileUploadAsync
- 返回参数: DifyApiResult<Dify_FileUploadResDto>
