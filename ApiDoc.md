# API Documentation

## 1. Introduction

### Basic structure of return parameters
```csharp
public class DifyApiResult<T>
{
    public int? Code { get; set; } // This parameter is the status code returned by the dify api, it may be null
    public bool? Success { get; set; } // Use this parameter to determine whether the request was successful
    public string Message { get; set; } // This parameter is the message returned by the dify api or the error message thrown by an exception
    public T Data { get; set; } // In this parameter, refer to the data structure returned in the dify api documentation
}
```

### Basic concepts of input parameters
- `overrideApiKey`: Supports passing in to override the BotApiKey in the configuration file, used for multiple Bot scenarios

## 2. Implemented API interfaces

### Send Chat Message
- Method name: CreateChatCompletionBlockModeAsync
- Return parameter: DifyApiResult<Dify_CreateChatCompletionResDto>

### Stop Generate
- Method name: StopChatCompletionAsync
- Return parameter: DifyApiResult<Dify_StopChatCompletionResDto>

### Message feedback (like)
- Method name: CreateFeedbackAsync
- Return parameter: DifyApiResult<Dify_CreateFeedbackResDto>

### Get next questions suggestions for the current message
- Method name: GetSuggestionsAsync
- Return parameter: DifyApiResult<Dify_BaseRequestResDto<List<string>>>

### Get Conversation History Messages
- Method name: GetConversationHistoryMessageAsync
- Return parameter: DifyApiResult<Dify_GetConversationHistoryMessageResDto>

### Get conversation list
- Method name: GetConversationListAsync
- Return parameter: DifyApiResult<Dify_GetConversationListResDto>

### Delete a conversation
- Method name: DeleteConversationAsync
- No return parameter

### Rename conversation
- Method name: RenameConversationAsync
- Return parameter: DifyApiResult<Dify_RenameConversationResDto>

### Audio to text
- Method name: AudioToTextAsync
- Return parameter: DifyApiResult<Dify_AudioToTextResDto>

### Get application information
- Method name: GetApplicationInfoAsync
- Return parameter: DifyApiResult<Dify_GetApplicationInfoResDto>

### Get application metadata
- Method name: GetApplicationMetaAsync
- Return parameter: DifyApiResult<Dify_GetApplicationMetaResDto>

### File Upload
- Method name: FileUploadAsync
- Return parameter: DifyApiResult<Dify_FileUploadResDto>