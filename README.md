# Dify C# SDK

[![Github](https://img.shields.io/badge/OpenSource-Github-blue)](https://github.com/IcedMango/DifyAi-csharp-sdk)
[![Nuget Version](https://img.shields.io/nuget/v/IcedMango.DifyAi)](https://www.nuget.org/packages/IcedMango.DifyAi)
[![Nuget Download](https://img.shields.io/nuget/dt/IcedMango.DifyAi)](https://www.nuget.org/packages/IcedMango.DifyAi)
[![GitHub License](https://img.shields.io/github/license/IcedMango/DifyAi-csharp-sdk)](https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/LICENSE)
[![Action Status](https://img.shields.io/github/actions/workflow/status/IcedMango/DifyAi-csharp-sdk/publishNuGet.yml)](https://github.com/IcedMango/DifyAi-csharp-sdk/actions)
[![Commit Status](https://img.shields.io/github/commit-activity/m/IcedMango/DifyAi-csharp-sdk?labelColor=%20%2332b583&color=%20%2312b76a)](https://github.com/IcedMango/DifyAi-csharp-sdk)
[![Language](https://img.shields.io/github/languages/top/IcedMango/DifyAi-csharp-sdk)](https://github.com/IcedMango/DifyAi-csharp-sdk)
[![.NET-6.0](https://img.shields.io/badge/.NET-6.0-blue)](https://github.com/IcedMango/DifyAi-csharp-sdk)
[![.NET-7.0](https://img.shields.io/badge/.NET-7.0-blue)](https://github.com/IcedMango/DifyAi-csharp-sdk)
[![.NET-8.0](https://img.shields.io/badge/.NET-8.0-blue)](https://github.com/IcedMango/DifyAi-csharp-sdk)

> **Warning**
> This SDK is currently in alpha and has not been fully tested. Issues and Pull Requests are welcome!

# Introduction

[![简体中文](https://img.shields.io/badge/简体中文-green)](https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.zh-CN.md)
[![English](https://img.shields.io/badge/English-red)](https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.md)

Professional C# SDK for Dify AI platform, providing comprehensive API support for chatbot interactions and knowledge base management.

**Version**: `2.0.0-alpha.dify1.11.2` (Compatible with Dify API v1.11.2)

**Core Features**:
- **Multi-Instance Support**: Manage multiple Bot/Dataset instances with different API Keys
- **DI Mode + Factory Pattern**: Seamless integration with ASP.NET Core dependency injection
- **Comprehensive Coverage**: Chat, knowledge base, file upload, audio processing
- **Thread Safety**: Support concurrent operations
- **Modern .NET**: Support for .NET 6.0, 7.0, 8.0

**V2.0 Breaking Changes**
- **New DI Architecture**: V2.0 uses factory pattern, registered via `AddDifyAi()`
- **Removed `overrideApiKey`**: Each instance is bound to its API Key at registration time
- **Migration Guide**: [English](./docs/MIGRATION_V1_TO_V2.md) | [Chinese](./docs/MIGRATION_V1_TO_V2.zh-CN.md)

# Quick Start

## Installation

```bash
dotnet add package IcedMango.DifyAi
# or
Install-Package IcedMango.DifyAi
```

## Usage: DI Mode + Factory Pattern

**Use Case**: ASP.NET Core applications - Manage multiple Dify instances via dependency injection

### Step 1: Register services in Program.cs

```csharp
using DifyAi.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Register Dify AI services with multi-instance support
builder.Services.AddDifyAi(register =>
{
    // Quick registration - Bot instances
    register.RegisterBot("CustomerService", "app-xxx");
    register.RegisterBot("TechSupport", "app-yyy", "https://custom.dify.ai/v1/");

    // Quick registration - Dataset instances
    register.RegisterDataset("ProductDocs", "dataset-xxx");
    register.RegisterDataset("FAQ", "dataset-yyy");

    // Advanced configuration: Register with proxy and SSL settings
    register.RegisterBot(new DifyAiInstanceConfig
    {
        Name = "ProxyBot",
        ApiKey = "app-zzz",
        BaseUrl = "https://api.dify.ai/v1/",
        ProxyUrl = "socks5://127.0.0.1:8889",
        IgnoreSslErrors = true  // For development/testing only
    });
});

var app = builder.Build();
app.Run();
```

### Step 2: Use services via dependency injection

```csharp
using DifyAi.Interface;
using DifyAi.Dto.ParamDto.Bot;

public class ChatController : ControllerBase
{
    private readonly IDifyAiServicesFactory _servicesFactory;

    public ChatController(IDifyAiServicesFactory servicesFactory)
    {
        _servicesFactory = servicesFactory;
    }

    public async Task<IActionResult> SendMessage(string message)
    {
        // Get Bot service by instance name
        var chatService = _servicesFactory.GetBotService("CustomerService");

        // Chat completion
        var result = await chatService.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto
            {
                Query = message,
                User = "user123",
                ConversationId = ""
            });

        if (result.Success)
        {
            return Ok(result.Data.Answer);
        }

        return BadRequest(result.Message);
    }

    public async Task<IActionResult> TechSupport(string question)
    {
        // Use different instances dynamically
        var techBot = _servicesFactory.GetBotService("TechSupport");
        var result = await techBot.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto { Query = question, User = "user123" });

        return Ok(result);
    }
}
```

## Core Features & Examples

### 1. Chat API

```csharp
var chatService = _servicesFactory.GetBotService("CustomerService");

// Block mode chat
var chatResult = await chatService.CreateChatCompletionBlockModeAsync(
    new DifyCreateChatCompletionParamDto
    {
        Query = "Hello, who are you?",
        User = "user123"
    });

// Stop chat
await chatService.StopChatCompletionAsync(new DifyStopChatCompletionParamDto
{
    TaskId = "task-xxx",
    User = "user123"
});

// Get conversation list
var conversations = await chatService.GetConversationListAsync(
    new DifyGetConversationListParamDto
    {
        User = "user123",
        Limit = 20
    });

// Audio to text
var audioResult = await chatService.AudioToTextAsync(new DifyAudioToTextParamDto
{
    FilePath = "/path/to/audio.mp3",
    User = "user123"
});

// Text to audio
var ttsResult = await chatService.TextToAudioAsync(new DifyTextToAudioParamDto
{
    MessageId = "msg-xxx",
    Text = "Hello",
    User = "user123"
});

// Get conversation variables
var variables = await chatService.GetConversationVariablesAsync(
    new DifyGetConversationVariablesParamDto
    {
        ConversationId = "conv-xxx",
        User = "user123",
        Limit = 20
    });

// Get file preview
var filePreview = await chatService.GetFilePreviewAsync("file-xxx", "user123");
```

### 2. Knowledge Base API

```csharp
var datasetService = _servicesFactory.GetDatasetService("ProductDocs");

// Create dataset
var dataset = await datasetService.CreateDatasetAsync(new DifyCreateDatasetParamDto
{
    Name = "My Dataset",
    Description = "Product documentation"
});

// Update dataset
await datasetService.UpdateDatasetAsync(new DifyUpdateDatasetParamDto
{
    DatasetId = "dataset-xxx",
    Name = "Updated name",
    Description = "Updated description"
});

// Get dataset list (with pagination)
var datasets = await datasetService.GetDatasetListAsync(1, 20);

// Get dataset detail
var detail = await datasetService.GetDatasetDetailAsync("dataset-xxx");

// Create document by text
var doc = await datasetService.CreateDocumentByTextAsync(
    new DifyCreateDocumentByTextParamDto
    {
        DatasetId = "dataset-id",
        Text = "Document content",
        Name = "Document name",
        IsAutomaticProcess = true
    });

// Create document by file
var fileDoc = await datasetService.CreateDocumentByFileAsync(
    new DifyCreateDocumentByFileParamDto
    {
        DatasetId = "dataset-id",
        FilePath = "/path/to/file.pdf",
        Name = "File document",
        IsAutomaticProcess = true
    });

// Get document list
var documents = await datasetService.GetDocumentListAsync(
    new DifyGetDocumentListParamDto
    {
        DatasetId = "dataset-xxx",
        Page = 1,
        Limit = 20
    });

// Get document detail
var docDetail = await datasetService.GetDocumentDetailAsync("dataset-xxx", "doc-xxx");

// Retrieve from knowledge base
var retrieveResult = await datasetService.RetrieveAsync(new DifyRetrieveParamDto
{
    DatasetId = "dataset-id",
    Query = "Search query",
    TopK = 3
});

// Add document segment
await datasetService.AddDocumentSegmentAsync(new DifyAddDocumentSegmentParamDto
{
    DatasetId = "dataset-xxx",
    DocumentId = "doc-xxx",
    Content = "Segment content"
});

// Update document segment
await datasetService.UpdateDocumentSegmentAsync(
    new DifyUpdateDocumentSegmentParamDto
    {
        DatasetId = "dataset-xxx",
        DocumentId = "doc-xxx",
        SegmentId = "segment-xxx",
        Content = "Updated content"
    });
```

### 3. Multi-Instance Management

```csharp
public class MultiInstanceController : ControllerBase
{
    private readonly IDifyAiServicesFactory _factory;

    public MultiInstanceController(IDifyAiServicesFactory factory)
    {
        _factory = factory;
    }

    public async Task<IActionResult> RouteByDepartment(string department, string message)
    {
        // Dynamically select Bot instance based on department
        var botName = department switch
        {
            "customer" => "CustomerService",
            "tech" => "TechSupport",
            _ => "CustomerService"
        };

        var chatService = _factory.GetBotService(botName);
        var result = await chatService.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto
            {
                Query = message,
                User = "user123"
            });

        return Ok(result);
    }

    public async Task<IActionResult> SearchKnowledge(string category, string query)
    {
        // Dynamically select Dataset instance
        var datasetName = category == "products" ? "ProductDocs" : "FAQ";
        var datasetService = _factory.GetDatasetService(datasetName);

        var result = await datasetService.RetrieveAsync(new DifyRetrieveParamDto
        {
            DatasetId = "dataset-xxx",
            Query = query,
            TopK = 5
        });

        return Ok(result);
    }
}
```

## API Documentation

For detailed API documentation, please refer to:
- [API Documentation](./ApiDoc.md) | [API Documentation (Chinese)](./ApiDoc.zh-CN.md)

## V2.0 Breaking Changes

### 1. Removed `overrideApiKey` parameter
```csharp
// Old version V1.x (no longer supported)
await chatService.CreateChatCompletionAsync(param, overrideApiKey: "app-yyy");

// New version V2.0 (register multiple instances)
builder.Services.AddDifyAi(register =>
{
    register.RegisterBot("Bot1", "app-xxx");
    register.RegisterBot("Bot2", "app-yyy");
});

var bot1 = _factory.GetBotService("Bot1");
var bot2 = _factory.GetBotService("Bot2");
```

### 2. DTO naming convention
```csharp
// Old version V1.x
new Dify_CreateChatCompletionParamDto()

// New version V2.0
new DifyCreateChatCompletionParamDto()
```

### 3. Removed static container
```csharp
// Old version V1.x (static container)
DifyAiContainer.RegisterBot("Bot1", "app-xxx");
var service = DifyAiContainer.GetBotService("Bot1");

// New version V2.0 (DI mode)
builder.Services.AddDifyAi(register => register.RegisterBot("Bot1", "app-xxx"));
// Inject IDifyAiServicesFactory in Controller/Service
```


## Contributing

Issues and Pull Requests are welcome!

## License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.

## Contributor License Agreement (CLA)

By submitting a PR, you agree to license your contribution to the project owner under the MIT license.
