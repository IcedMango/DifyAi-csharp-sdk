using DifyAi.Dto.ParamDto;
using DifyAi.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

/// <summary>
/// Demo controller demonstrating DifyAi SDK usage
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    private readonly IDifyAiServicesFactory _servicesFactory;

    public DemoController(IDifyAiServicesFactory servicesFactory)
    {
        _servicesFactory = servicesFactory;
    }

    #region Chat API Examples

    /// <summary>
    /// Send a chat message to the bot
    /// </summary>
    [HttpPost("chat")]
    public async Task<IActionResult> Chat(
        [FromQuery] string message,
        [FromQuery] string user = "demo-user",
        [FromQuery] string conversationId = "")
    {
        var chatService = _servicesFactory.GetBotService("MyBot");

        var result = await chatService.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto
            {
                Query = message,
                User = user,
                ConversationId = conversationId
            });

        if (result.Success == true)
        {
            return Ok(new
            {
                answer = result.Data?.Answer,
                conversationId = result.Data?.ConversationId,
                messageId = result.Data?.MessageId
            });
        }

        return BadRequest(new { error = result.Message });
    }

    /// <summary>
    /// Get conversation list
    /// </summary>
    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations([FromQuery] string user = "demo-user")
    {
        var chatService = _servicesFactory.GetBotService("MyBot");

        var result = await chatService.GetConversationListAsync(
            new DifyGetConversationListParamDto { User = user, Limit = 20 });

        if (result.Success == true)
            return Ok(result.Data);

        return BadRequest(new { error = result.Message });
    }

    /// <summary>
    /// Get application info
    /// </summary>
    [HttpGet("app-info")]
    public async Task<IActionResult> GetAppInfo([FromQuery] string user = "demo-user")
    {
        var chatService = _servicesFactory.GetBotService("MyBot");
        var result = await chatService.GetApplicationInfoAsync(user);

        if (result.Success == true)
            return Ok(result.Data);

        return BadRequest(new { error = result.Message });
    }

    #endregion

    #region Dataset API Examples

    /// <summary>
    /// Get dataset list
    /// </summary>
    [HttpGet("datasets")]
    public async Task<IActionResult> GetDatasets([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var datasetService = _servicesFactory.GetDatasetService("MyDataset");
        var result = await datasetService.GetDatasetListAsync(page, limit);

        if (result.Success == true)
            return Ok(result.Data);

        return BadRequest(new { error = result.Message });
    }

    /// <summary>
    /// Retrieve from knowledge base
    /// </summary>
    [HttpGet("retrieve")]
    public async Task<IActionResult> Retrieve(
        [FromQuery] string datasetId,
        [FromQuery] string query,
        [FromQuery] int topK = 3)
    {
        var datasetService = _servicesFactory.GetDatasetService("MyDataset");

        var result = await datasetService.RetrieveAsync(new DifyRetrieveParamDto
        {
            DatasetId = datasetId,
            Query = query,
            TopK = topK
        });

        if (result.Success == true)
            return Ok(result.Data);

        return BadRequest(new { error = result.Message });
    }

    #endregion
}
