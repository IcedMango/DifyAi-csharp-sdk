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

## 2. Chat API (IDifyAiChatServices)

### Send Chat Message
- Method name: CreateChatCompletionBlockModeAsync
- Return parameter: DifyApiResult<DifyCreateChatCompletionResDto>

### Stop Generate
- Method name: StopChatCompletionAsync
- Return parameter: DifyApiResult<DifyStopChatCompletionResDto>

### Message feedback (like)
- Method name: CreateFeedbackAsync
- Return parameter: DifyApiResult<DifyCreateFeedbackResDto>

### Get next questions suggestions for the current message
- Method name: GetSuggestionsAsync
- Return parameter: DifyApiResult<DifyBaseRequestResDto<List<string>>>

### Get Conversation History Messages
- Method name: GetConversationHistoryMessageAsync
- Return parameter: DifyApiResult<DifyGetConversationHistoryMessageResDto>

### Get conversation list
- Method name: GetConversationListAsync
- Return parameter: DifyApiResult<DifyGetConversationListResDto>

### Delete a conversation
- Method name: DeleteConversationAsync
- Return parameter: DifyApiResult<DifyBaseRequestResDto>

### Rename conversation
- Method name: RenameConversationAsync
- Return parameter: DifyApiResult<DifyRenameConversationResDto>

### Audio to text
- Method name: AudioToTextAsync
- Return parameter: DifyApiResult<DifyAudioToTextResDto>

### Get application information
- Method name: GetApplicationInfoAsync
- Return parameter: DifyApiResult<DifyGetApplicationInfoResDto>

### Get application metadata
- Method name: GetApplicationMetaAsync
- Return parameter: DifyApiResult<DifyGetApplicationMetaResDto>

### File Upload
- Method name: FileUploadAsync
- Return parameter: DifyApiResult<DifyFileUploadResDto>

### Get conversation variables
- Method name: GetConversationVariablesAsync
- Return parameter: DifyApiResult<DifyGetConversationVariablesResDto>

### Get app feedbacks
- Method name: GetAppFeedbacksAsync
- Return parameter: DifyApiResult<DifyGetAppFeedbacksResDto>

### Get file preview
- Method name: GetFilePreviewAsync
- Return parameter: DifyApiResult<DifyGetFilePreviewResDto>

### Text to audio
- Method name: TextToAudioAsync
- Return parameter: DifyApiResult<DifyTextToAudioResDto>

## 3. Dataset API (IDifyAiDatasetServices)

### Dataset Management

#### Create dataset
- Method name: CreateDatasetAsync
- Return parameter: DifyApiResult<DifyCreateDatasetResDto>

#### Get dataset list
- Method name: GetDatasetListAsync
- Return parameter: DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>

#### Get dataset detail
- Method name: GetDatasetDetailAsync
- Return parameter: DifyApiResult<DifyDatasetDetailResDto>

#### Update dataset
- Method name: UpdateDatasetAsync
- Return parameter: DifyApiResult<DifyDatasetDetailResDto>

#### Delete dataset
- Method name: DeleteDatasetAsync
- Return parameter: DifyApiResult<DifyBaseRequestResDto>

### Document Management

#### Create document by text
- Method name: CreateDocumentByTextAsync
- Return parameter: DifyApiResult<DifyCreateModifyDocumentResDto>

#### Update document by text
- Method name: UpdateDocumentByTextAsync
- Return parameter: DifyApiResult<DifyCreateModifyDocumentResDto>

#### Create document by file
- Method name: CreateDocumentByFileAsync
- Return parameter: DifyApiResult<DifyCreateModifyDocumentResDto>

#### Update document by file
- Method name: UpdateDocumentByFileAsync
- Return parameter: DifyApiResult<DifyCreateModifyDocumentResDto>

#### Delete document
- Method name: DeleteDocumentAsync
- Return parameter: DifyApiResult<DifyBaseRequestResDto>

#### Get document list
- Method name: GetDocumentListAsync
- Return parameter: DifyApiResult<DifyBaseRequestResDto<List<DifyGetDocumentListResDto>>>

#### Get document detail
- Method name: GetDocumentDetailAsync
- Return parameter: DifyApiResult<DifyDocumentDetailResDto>

#### Get document embedding status
- Method name: GetDocumentEmbeddingAsync
- Return parameter: DifyApiResult<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>>

#### Retrieve knowledge base
- Method name: RetrieveAsync
- Return parameter: DifyApiResult<DifyRetrieveResDto>

### Document Segment Management

#### Add document segment
- Method name: AddDocumentSegmentAsync
- Return parameter: DifyApiResult<DifyDocumentSegmentResDto>

#### Update document segment
- Method name: UpdateDocumentSegmentAsync
- Return parameter: DifyApiResult<DifyDocumentSegmentResDto>

#### Get document segment
- Method name: GetDocumentSegmentAsync
- Return parameter: DifyApiResult<DifyDocumentSegmentResDto>

#### Delete document segment
- Method name: DeleteDocumentSegmentAsync
- Return parameter: DifyApiResult<DifyDocumentSegmentResDto>
