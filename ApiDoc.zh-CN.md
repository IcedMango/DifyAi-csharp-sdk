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

## 2. 聊天API (IDifyAiChatServices)

### 发送对话消息
- 方法名称: CreateChatCompletionBlockModeAsync
- 返回参数: DifyApiResult<DifyCreateChatCompletionResDto>

### 停止响应
- 方法名称: StopChatCompletionAsync
- 返回参数: DifyApiResult<DifyStopChatCompletionResDto>

### 消息反馈（点赞）
- 方法名称: CreateFeedbackAsync
- 返回参数: DifyApiResult<DifyCreateFeedbackResDto>

### 获取下一轮建议问题列表
- 方法名称: GetSuggestionsAsync
- 返回参数: DifyApiResult<DifyBaseRequestResDto<List<string>>>

### 获取对话历史消息
- 方法名称: GetConversationHistoryMessageAsync
- 返回参数: DifyApiResult<DifyGetConversationHistoryMessageResDto>

### 获取对话列表
- 方法名称: GetConversationListAsync
- 返回参数: DifyApiResult<DifyGetConversationListResDto>

### 删除对话
- 方法名称: DeleteConversationAsync
- 返回参数: DifyApiResult<DifyBaseRequestResDto>

### 重命名对话
- 方法名称: RenameConversationAsync
- 返回参数: DifyApiResult<DifyRenameConversationResDto>

### 音频转文本
- 方法名称: AudioToTextAsync
- 返回参数: DifyApiResult<DifyAudioToTextResDto>

### 获取应用信息
- 方法名称: GetApplicationInfoAsync
- 返回参数: DifyApiResult<DifyGetApplicationInfoResDto>

### 获取应用元数据
- 方法名称: GetApplicationMetaAsync
- 返回参数: DifyApiResult<DifyGetApplicationMetaResDto>

### 上传文件
- 方法名称: FileUploadAsync
- 返回参数: DifyApiResult<DifyFileUploadResDto>

### 获取对话变量
- 方法名称: GetConversationVariablesAsync
- 返回参数: DifyApiResult<DifyGetConversationVariablesResDto>

### 获取应用反馈列表
- 方法名称: GetAppFeedbacksAsync
- 返回参数: DifyApiResult<DifyGetAppFeedbacksResDto>

### 获取文件预览
- 方法名称: GetFilePreviewAsync
- 返回参数: DifyApiResult<DifyGetFilePreviewResDto>

### 文本转音频
- 方法名称: TextToAudioAsync
- 返回参数: DifyApiResult<DifyTextToAudioResDto>

## 3. 知识库API (IDifyAiDatasetServices)

### 知识库管理

#### 创建知识库
- 方法名称: CreateDatasetAsync
- 返回参数: DifyApiResult<DifyCreateDatasetResDto>

#### 获取知识库列表
- 方法名称: GetDatasetListAsync
- 返回参数: DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>

#### 获取知识库详情
- 方法名称: GetDatasetDetailAsync
- 返回参数: DifyApiResult<DifyDatasetDetailResDto>

#### 更新知识库
- 方法名称: UpdateDatasetAsync
- 返回参数: DifyApiResult<DifyDatasetDetailResDto>

#### 删除知识库
- 方法名称: DeleteDatasetAsync
- 返回参数: DifyApiResult<DifyBaseRequestResDto>

### 文档管理

#### 通过文本创建文档
- 方法名称: CreateDocumentByTextAsync
- 返回参数: DifyApiResult<DifyCreateModifyDocumentResDto>

#### 通过文本更新文档
- 方法名称: UpdateDocumentByTextAsync
- 返回参数: DifyApiResult<DifyCreateModifyDocumentResDto>

#### 通过文件创建文档
- 方法名称: CreateDocumentByFileAsync
- 返回参数: DifyApiResult<DifyCreateModifyDocumentResDto>

#### 通过文件更新文档
- 方法名称: UpdateDocumentByFileAsync
- 返回参数: DifyApiResult<DifyCreateModifyDocumentResDto>

#### 删除文档
- 方法名称: DeleteDocumentAsync
- 返回参数: DifyApiResult<DifyBaseRequestResDto>

#### 获取文档列表
- 方法名称: GetDocumentListAsync
- 返回参数: DifyApiResult<DifyBaseRequestResDto<List<DifyGetDocumentListResDto>>>

#### 获取文档详情
- 方法名称: GetDocumentDetailAsync
- 返回参数: DifyApiResult<DifyDocumentDetailResDto>

#### 获取文档向量化进度
- 方法名称: GetDocumentEmbeddingAsync
- 返回参数: DifyApiResult<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>>

#### 检索知识库
- 方法名称: RetrieveAsync
- 返回参数: DifyApiResult<DifyRetrieveResDto>

### 文档分段管理

#### 添加文档分段
- 方法名称: AddDocumentSegmentAsync
- 返回参数: DifyApiResult<DifyDocumentSegmentResDto>

#### 更新文档分段
- 方法名称: UpdateDocumentSegmentAsync
- 返回参数: DifyApiResult<DifyDocumentSegmentResDto>

#### 获取文档分段
- 方法名称: GetDocumentSegmentAsync
- 返回参数: DifyApiResult<DifyDocumentSegmentResDto>

#### 删除文档分段
- 方法名称: DeleteDocumentSegmentAsync
- 返回参数: DifyApiResult<DifyDocumentSegmentResDto>
