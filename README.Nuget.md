# DifyAi C# SDK

**Work with DifyAi `v1.11.2`**

> 一个 Dify 的 C# SDK,用于与 Dify 的 API 进行交互。支持聊天/知识库 API。
>
> A Dify C# SDK for interacting with DifyAi's APIs. Supports Chat/Knowledge Base APIs.

## Quick Start

```csharp
using DifyAi.ServiceExtension;

// Register in Program.cs
builder.Services.AddDifyAi(register =>
{
    register.RegisterBot("MyBot", "app-xxx");
    register.RegisterDataset("MyDataset", "dataset-xxx");
});

// Use in Controller
public class ChatController : ControllerBase
{
    private readonly IDifyAiServicesFactory _factory;

    public ChatController(IDifyAiServicesFactory factory)
    {
        _factory = factory;
    }

    public async Task<IActionResult> Chat(string message)
    {
        var chatService = _factory.GetBotService("MyBot");
        var result = await chatService.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto
            {
                Query = message,
                User = "user123"
            });

        return Ok(result);
    }
}
```

## Documentation

📖 Full docs on Github → [https://github.com/IcedMango/DifyAi-csharp-sdk](https://github.com/IcedMango/DifyAi-csharp-sdk)
