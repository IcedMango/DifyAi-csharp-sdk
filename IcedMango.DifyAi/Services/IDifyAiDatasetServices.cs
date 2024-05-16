namespace DifyAi.Services;

public interface IDifyAiDatasetServices
{
    /// <summary>
    ///     Create a dataset
    /// </summary>
    /// <param name="paramDto"></param>
    Task<DifyApiResult<Dify_CreateDatasetResDto>> CreateDatasetAsync(Dify_CreateDatasetParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get dataset list
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="limit">Number of items returned(range 1-100)</param>
    Task<DifyApiResult<Dify_BaseRequestResDto<List<Dify_GetDatasetListResDto>>>> GetDatasetListAsync(
        int page, int limit = 20, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> CreateDocumentByTextAsync(
        Dify_CreateDocumentByTextParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> UpdateDocumentByTextAsync(
        Dify_UpdateDocumentByTextParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> CreateDocumentByFileAsync(
        Dify_CreateDocumentByFileParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> UpdateDocumentByFileAsync(
        Dify_UpdateDocumentByFileParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///      Delete document
    /// </summary>
    /// <param name="datasetId">Knowledge ID</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task DeleteDocumentAsync(string datasetId, string documentId, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get document embedding status (progress)
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="batch"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_BaseRequestResDto<List<Dify_GetDocumentEmbeddingResDto>>>>
        GetDocumentEmbeddingAsync(string datasetId, string batch, string overrideApiKey = "",
            CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get knowledge document list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_BaseRequestResDto<List<Dify_GetDocumentListResDto>>>> GetDocumentListAsync(
        Dify_GetDocumentListParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Add document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_DocumentSegmentResDto>> AddDocumentSegmentAsync(
        Dify_AddDocumentSegmentParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Add document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_DocumentSegmentResDto>> UpdatedDocumentSegmentAsync(
        Dify_UpdateDocumentSegmentParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_DocumentSegmentResDto>> GetDocumentSegmentAsync(
        Dify_GetDocumentSegmentParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete document segment
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="documentId"></param>
    /// <param name="segmentId"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<Dify_DocumentSegmentResDto>> DeleteDocumentSegmentAsync(
        string datasetId, string documentId, string segmentId, string overrideApiKey = "",
        CancellationToken cancellationToken = default);
}