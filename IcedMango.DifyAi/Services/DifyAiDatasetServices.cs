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
    public async Task<DifyApiResult<Dify_CreateDatasetResDto>> CreateDatasetAsync(Dify_CreateDatasetParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<Dify_CreateDatasetResDto>(
            "datasets",
            paramDto,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }


    /// <summary>
    ///     Get dataset list
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="limit">Number of items returned(range 1-100)</param>
    public async Task<DifyApiResult<Dify_BaseRequestResDto<List<Dify_GetDatasetListResDto>>>> GetDatasetListAsync(
        int page, int limit = 20, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<Dify_BaseRequestResDto<List<Dify_GetDatasetListResDto>>>(
            $"datasets?page={page}&limit={limit}",
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    #endregion

    #region document

    #region text_document

    /// <summary>
    ///     Create a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> CreateDocumentByTextAsync(
        Dify_CreateDocumentByTextParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApi_CreateDocumentByTextParam>();

        apiParam.IndexingTechnique = paramDto.EnableHighQualityIndex == true ? "high_quality" : "economy";

        apiParam.ProcessRule = new Dify_Dataset_ProcessRule()
        {
            Mode = paramDto.IsAutomaticProcess == true ? "automatic" : "custom",
            Rules = new Dify_Dataset_ProcessRule_RuleItem()
            {
                PreProcessingRules = new List<Dify_Dataset_ProcessRule_PreProcessingRules>()
                {
                    new ()
                    {
                        Id = "remove_extra_spaces",
                        Enabled = paramDto.RemoveExtraSpaces
                    },
                    new ()
                    {
                        Id = "remove_urls_emails",
                        Enabled = paramDto.RemoveUrlsEmails
                    }
                },
                Segmentation = new Dify_Dataset_ProcessRule_Segmentation()
                {
                    Separator = paramDto.Separator,
                    MaxTokens = paramDto.MaxTokens
                }
            }
        };


        var res = await _requestExtension.HttpPost<Dify_CreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/document/create_by_text",
            apiParam,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }


    /// <summary>
    ///     Update a document by text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> UpdateDocumentByTextAsync(
        Dify_UpdateDocumentByTextParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApi_UpdateDocumentByTextParam>();

        apiParam.ProcessRule = new Dify_Dataset_ProcessRule()
        {
            Mode = paramDto.IsAutomaticProcess == true ? "automatic" : "custom",
            Rules = new Dify_Dataset_ProcessRule_RuleItem()
            {
                PreProcessingRules = new List<Dify_Dataset_ProcessRule_PreProcessingRules>()
                {
                    new ()
                    {
                        Id = "remove_extra_spaces",
                        Enabled = paramDto.RemoveExtraSpaces
                    },
                    new ()
                    {
                        Id = "remove_urls_emails",
                        Enabled = paramDto.RemoveUrlsEmails
                    }
                },
                Segmentation = new Dify_Dataset_ProcessRule_Segmentation()
                {
                    Separator = paramDto.Separator,
                    MaxTokens = paramDto.MaxTokens
                }
            }
        };

        var res = await _requestExtension.HttpPost<Dify_CreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/update_by_text",
            apiParam,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    #endregion

    #region file_document

    /// <summary>
    ///     Create a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> CreateDocumentByFileAsync(
        Dify_CreateDocumentByFileParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApi_CreateDocumentByFileParam>();

        apiParam.IndexingTechnique = paramDto.EnableHighQualityIndex == true ? "high_quality" : "economy";

        apiParam.ProcessRule = new Dify_Dataset_ProcessRule()
        {
            Mode = paramDto.IsAutomaticProcess == true ? "automatic" : "custom",
            Rules = new Dify_Dataset_ProcessRule_RuleItem()
            {
                PreProcessingRules = new List<Dify_Dataset_ProcessRule_PreProcessingRules>()
                {
                    new ()
                    {
                        Id = "remove_extra_spaces",
                        Enabled = paramDto.RemoveExtraSpaces
                    },
                    new ()
                    {
                        Id = "remove_urls_emails",
                        Enabled = paramDto.RemoveUrlsEmails
                    }
                },
                Segmentation = new Dify_Dataset_ProcessRule_Segmentation()
                {
                    Separator = paramDto.Separator,
                    MaxTokens = paramDto.MaxTokens
                }
            }
        };


        var res = await _requestExtension.PostUploadDocumentAsync<Dify_CreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/document/create_by_file",
            apiParam,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    /// <summary>
    ///     Update a document by file
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_CreateModifyDocumentResDto>> UpdateDocumentByFileAsync(
        Dify_UpdateDocumentByFileParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var apiParam = paramDto.Adapt<DifyApi_UpdateDocumentByFileParam>();

        apiParam.IndexingTechnique = paramDto.EnableHighQualityIndex == true ? "high_quality" : "economy";

        apiParam.ProcessRule = new Dify_Dataset_ProcessRule()
        {
            Mode = paramDto.IsAutomaticProcess == true ? "automatic" : "custom",
            Rules = new Dify_Dataset_ProcessRule_RuleItem()
            {
                PreProcessingRules = new List<Dify_Dataset_ProcessRule_PreProcessingRules>()
                {
                    new ()
                    {
                        Id = "remove_extra_spaces",
                        Enabled = paramDto.RemoveExtraSpaces
                    },
                    new ()
                    {
                        Id = "remove_urls_emails",
                        Enabled = paramDto.RemoveUrlsEmails
                    }
                },
                Segmentation = new Dify_Dataset_ProcessRule_Segmentation()
                {
                    Separator = paramDto.Separator,
                    MaxTokens = paramDto.MaxTokens
                }
            }
        };

        var res = await _requestExtension.PostUploadFileAsync<Dify_CreateModifyDocumentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/update_by_file",
            paramDto,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    #endregion

    /// <summary>
    ///      Delete document
    /// </summary>
    /// <param name="datasetId">Knowledge ID</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_BaseRequestResDto>> DeleteDocumentAsync(string datasetId, string documentId,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpDelete<Dify_BaseRequestResDto>(
            $"datasets/{datasetId}/documents/{documentId}",
            null,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);
        return res;
    }

    /// <summary>
    ///     Get document embedding status (progress)
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="batch"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_BaseRequestResDto<List<Dify_GetDocumentEmbeddingResDto>>>>
        GetDocumentEmbeddingAsync(string datasetId, string batch, string overrideApiKey = "",
            CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<Dify_BaseRequestResDto<List<Dify_GetDocumentEmbeddingResDto>>>(
            $"datasets/{datasetId}/documents/{batch}/indexing-status",
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    /// <summary>
    ///     Get knowledge document list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_BaseRequestResDto<List<Dify_GetDocumentListResDto>>>> GetDocumentListAsync(
        Dify_GetDocumentListParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<Dify_BaseRequestResDto<List<Dify_GetDocumentListResDto>>>(
            $"datasets/{paramDto.DatasetId}/documents?keyword={HttpUtility.UrlEncode(paramDto.Keyword)}&page={paramDto.Page}&limit={paramDto.Limit}",
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    #endregion

    #region segments

    /// <summary>
    ///     Add document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_DocumentSegmentResDto>> AddDocumentSegmentAsync(
        Dify_AddDocumentSegmentParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<Dify_DocumentSegmentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/segments",
            paramDto,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    /// <summary>
    ///     Add document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_DocumentSegmentResDto>> UpdatedDocumentSegmentAsync(
        Dify_UpdateDocumentSegmentParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<Dify_DocumentSegmentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/segments/{paramDto.SegmentId}",
            paramDto,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    /// <summary>
    ///     Get document segment
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_DocumentSegmentResDto>> GetDocumentSegmentAsync(
        Dify_GetDocumentSegmentParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<Dify_DocumentSegmentResDto>(
            $"datasets/{paramDto.DatasetId}/documents/{paramDto.DocumentId}/segments?keyword={HttpUtility.UrlEncode(paramDto.Keyword)}&status={paramDto.Status}",
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    /// <summary>
    /// Delete document segment
    /// </summary>
    /// <param name="datasetId"></param>
    /// <param name="documentId"></param>
    /// <param name="segmentId"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<Dify_DocumentSegmentResDto>> DeleteDocumentSegmentAsync(
        string datasetId, string documentId, string segmentId, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpDelete<Dify_DocumentSegmentResDto>(
            $"datasets/{datasetId}/documents/{documentId}/segments/{segmentId}",
            null,
            overrideApiKey,
            cancellationToken,
            DifyApiClientName.Dataset);

        return res;
    }

    #endregion
}