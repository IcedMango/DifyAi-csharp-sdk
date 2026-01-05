using System.Web;
using DifyAi.Dto.ApiParamDto;
using Mapster;

namespace DifyAi.Services;

/// <summary>
///     Knowledge related service
/// </summary>
public class DifyAiDatasetServices : IDifyAiDatasetServices
{
    private readonly IRequestExtension _requestExtension;

    public DifyAiDatasetServices(IRequestExtension requestExtension)
    {
        _requestExtension = requestExtension;
    }

    #region dataset

    /// <summary>
    ///     Create a dataset
    /// </summary>
    /// <param name="paramDto"></param>
    public async Task<DifyApiResult<DifyCreateDatasetResDto>> CreateDatasetAsync(DifyCreateDatasetParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifyCreateDatasetResDto>(
            "datasets",
            paramDto,
            cancellationToken);

        return res;
    }


    /// <summary>
    ///     Get dataset list
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="limit">Number of items returned(range 1-100)</param>
    public async Task<DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>> GetDatasetListAsync(
        int page, int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>(
            $"datasets?page={page}&limit={limit}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get dataset list with extended parameters
    /// </summary>
    /// <param name="paramDto">Extended parameters including keyword, tag_ids, include_all</param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>> GetDatasetListAsync(
        DifyGetDatasetListParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>
        {
            $"page={paramDto.Page}",
            $"limit={paramDto.Limit}"
        };

        if (!string.IsNullOrWhiteSpace(paramDto.Keyword))
        {
            queryParams.Add($"keyword={HttpUtility.UrlEncode(paramDto.Keyword)}");
        }

        if (paramDto.TagIds != null && paramDto.TagIds.Count > 0)
        {
            queryParams.Add($"tag_ids={string.Join(",", paramDto.TagIds)}");
        }

        if (paramDto.IncludeAll)
        {
            queryParams.Add("include_all=true");
        }

        var res = await _requestExtension.HttpGet<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>(
            $"datasets?{string.Join("&", queryParams)}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get dataset detail
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyDatasetDetailResDto>> GetDatasetDetailAsync(
        string datasetId,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyDatasetDetailResDto>(
            $"datasets/{datasetId}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Update dataset
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyDatasetDetailResDto>> UpdateDatasetAsync(
        DifyUpdateDatasetParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPatch<DifyDatasetDetailResDto>(
            $"datasets/{paramDto.DatasetId}",
            paramDto,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Delete dataset
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyBaseRequestResDto>> DeleteDatasetAsync(
        string datasetId,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpDelete<DifyBaseRequestResDto>(
            $"datasets/{datasetId}",
            null,
            cancellationToken);

        return res;
    }

    #endregion

    #region document

    #region text_document

    /// <summary>
    ///     Create a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyCreateModifyDocumentResDto>> CreateDocumentByTextAsync(
        DifyCreateDocumentByTextParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApiCreateDocumentByTextParam>();

        apiParam.IndexingTechnique = paramDto.EnableHighQualityIndex == true ? "high_quality" : "economy";

        apiParam.ProcessRule = ProcessRuleBuilder.Build(
            paramDto.IsAutomaticProcess,
            paramDto.RemoveExtraSpaces,
            paramDto.RemoveUrlsEmails,
            paramDto.Separator,
            paramDto.MaxTokens);

        var res = await _requestExtension.HttpPost<DifyCreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/document/create-by-text",
            apiParam,
            cancellationToken);

        return res;
    }


    /// <summary>
    ///     Update a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyCreateModifyDocumentResDto>> UpdateDocumentByTextAsync(
        DifyUpdateDocumentByTextParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApiUpdateDocumentByTextParam>();

        apiParam.ProcessRule = ProcessRuleBuilder.Build(
            paramDto.IsAutomaticProcess,
            paramDto.RemoveExtraSpaces,
            paramDto.RemoveUrlsEmails,
            paramDto.Separator,
            paramDto.MaxTokens);

        var res = await _requestExtension.HttpPost<DifyCreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/update-by-text",
            apiParam,
            cancellationToken);

        return res;
    }

    #endregion

    #region file_document

    /// <summary>
    ///     Create a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyCreateModifyDocumentResDto>> CreateDocumentByFileAsync(
        DifyCreateDocumentByFileParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApiCreateDocumentByFileParam>();

        apiParam.IndexingTechnique = paramDto.EnableHighQualityIndex == true ? "high_quality" : "economy";

        apiParam.ProcessRule = ProcessRuleBuilder.Build(
            paramDto.IsAutomaticProcess,
            paramDto.RemoveExtraSpaces,
            paramDto.RemoveUrlsEmails,
            paramDto.Separator,
            paramDto.MaxTokens);

        var res = await _requestExtension.PostUploadDocumentAsync<DifyCreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/document/create-by-file",
            apiParam,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Update a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyCreateModifyDocumentResDto>> UpdateDocumentByFileAsync(
        DifyUpdateDocumentByFileParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApiUpdateDocumentByFileParam>();

        apiParam.IndexingTechnique = paramDto.EnableHighQualityIndex == true ? "high_quality" : "economy";

        apiParam.ProcessRule = ProcessRuleBuilder.Build(
            paramDto.IsAutomaticProcess,
            paramDto.RemoveExtraSpaces,
            paramDto.RemoveUrlsEmails,
            paramDto.Separator,
            paramDto.MaxTokens);

        var res = await _requestExtension.PostUploadFileAsync<DifyCreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/update-by-file",
            paramDto,
            cancellationToken);

        return res;
    }

    #endregion

    /// <summary>
    ///     Delete document
    /// </summary>
    /// <param name="datasetId">Knowledge ID</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyBaseRequestResDto>> DeleteDocumentAsync(string datasetId, string documentId,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpDelete<DifyBaseRequestResDto>(
            $"datasets/{datasetId}/documents/{documentId}",
            null,
            cancellationToken);
        return res;
    }

    /// <summary>
    ///     Get document detail
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="documentId"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyDocumentDetailResDto>> GetDocumentDetailAsync(
        string datasetId, string documentId,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyDocumentDetailResDto>(
            $"datasets/{datasetId}/documents/{documentId}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Knowledge base retrieval
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyRetrieveResDto>> RetrieveAsync(
        DifyRetrieveParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifyRetrieveResDto>(
            $"datasets/{paramDto.DatasetId}/retrieve",
            paramDto,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get document embedding status (progress)
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="batch"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>>>
        GetDocumentEmbeddingAsync(string datasetId, string batch,
            CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>>(
            $"datasets/{datasetId}/documents/{batch}/indexing-status",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get knowledge document list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyBaseRequestResDto<List<DifyGetDocumentListResDto>>>> GetDocumentListAsync(
        DifyGetDocumentListParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyBaseRequestResDto<List<DifyGetDocumentListResDto>>>(
            $"datasets/{paramDto.DatasetId}/documents?keyword={HttpUtility.UrlEncode(paramDto.Keyword)}&page={paramDto.Page}&limit={paramDto.Limit}",
            cancellationToken);

        return res;
    }

    #endregion

    #region segments

    /// <summary>
    ///     Add document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyDocumentSegmentResDto>> AddDocumentSegmentAsync(
        DifyAddDocumentSegmentParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifyDocumentSegmentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/segments",
            paramDto,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Update document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifySingleDocumentSegmentResDto>> UpdateDocumentSegmentAsync(
        DifyUpdateDocumentSegmentParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifySingleDocumentSegmentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/segments/{paramDto.SegmentId}",
            paramDto,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyDocumentSegmentResDto>> GetDocumentSegmentAsync(
        DifyGetDocumentSegmentParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyDocumentSegmentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/segments?keyword={HttpUtility.UrlEncode(paramDto.Keyword)}&status={paramDto.Status}&page={paramDto.Page}&limit={paramDto.Limit}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Delete document segment
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="documentId"></param>
    /// <param name="segmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyDocumentSegmentResDto>> DeleteDocumentSegmentAsync(
        string datasetId, string documentId, string segmentId,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpDelete<DifyDocumentSegmentResDto>(
            $"datasets/{datasetId}/documents/{documentId}/segments/{segmentId}",
            null,
            cancellationToken);

        return res;
    }

    #endregion
}
