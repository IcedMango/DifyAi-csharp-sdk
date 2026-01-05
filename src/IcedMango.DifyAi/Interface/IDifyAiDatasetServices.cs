namespace DifyAi.Interface;

public interface IDifyAiDatasetServices
{
    #region dataset

    /// <summary>
    ///     Create a dataset
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyCreateDatasetResDto>> CreateDatasetAsync(
        DifyCreateDatasetParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get dataset list
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="limit">Number of items returned(range 1-100)</param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>> GetDatasetListAsync(
        int page,
        int limit = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get dataset list with extended parameters
    /// </summary>
    /// <param name="paramDto">Extended parameters including keyword, tag_ids, include_all</param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>> GetDatasetListAsync(
        DifyGetDatasetListParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get dataset detail
    /// </summary>
    /// <param name="datasetId">Dataset ID</param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyDatasetDetailResDto>> GetDatasetDetailAsync(
        string datasetId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update dataset
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyDatasetDetailResDto>> UpdateDatasetAsync(
        DifyUpdateDatasetParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete dataset
    /// </summary>
    /// <param name="datasetId">Dataset ID</param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestResDto>> DeleteDatasetAsync(
        string datasetId,
        CancellationToken cancellationToken = default);

    #endregion

    #region document

    /// <summary>
    ///     Create a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyCreateModifyDocumentResDto>> CreateDocumentByTextAsync(
        DifyCreateDocumentByTextParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyCreateModifyDocumentResDto>> UpdateDocumentByTextAsync(
        DifyUpdateDocumentByTextParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyCreateModifyDocumentResDto>> CreateDocumentByFileAsync(
        DifyCreateDocumentByFileParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyCreateModifyDocumentResDto>> UpdateDocumentByFileAsync(
        DifyUpdateDocumentByFileParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete document
    /// </summary>
    /// <param name="datasetId">Knowledge ID</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestResDto>> DeleteDocumentAsync(
        string datasetId,
        string documentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get document embedding status (progress)
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="batch"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>>> GetDocumentEmbeddingAsync(
        string datasetId,
        string batch,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get knowledge document list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestResDto<List<DifyGetDocumentListResDto>>>> GetDocumentListAsync(
        DifyGetDocumentListParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get document detail
    /// </summary>
    /// <param name="datasetId">Dataset ID</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyDocumentDetailResDto>> GetDocumentDetailAsync(
        string datasetId,
        string documentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Knowledge base retrieval
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyRetrieveResDto>> RetrieveAsync(
        DifyRetrieveParamDto paramDto,
        CancellationToken cancellationToken = default);

    #endregion

    #region segments

    /// <summary>
    ///     Add document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyDocumentSegmentResDto>> AddDocumentSegmentAsync(
        DifyAddDocumentSegmentParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifySingleDocumentSegmentResDto>> UpdateDocumentSegmentAsync(
        DifyUpdateDocumentSegmentParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyDocumentSegmentResDto>> GetDocumentSegmentAsync(
        DifyGetDocumentSegmentParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete document segment
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="documentId"></param>
    /// <param name="segmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyDocumentSegmentResDto>> DeleteDocumentSegmentAsync(
        string datasetId,
        string documentId,
        string segmentId,
        CancellationToken cancellationToken = default);

    #endregion
}