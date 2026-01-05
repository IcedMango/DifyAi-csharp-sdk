# IcedMango.DifyAi Migration Guide: v1.x to v2.0

This document provides a complete guide for migrating from v1.x to v2.0.

## Table of Contents

- [Version Information](#version-information)
- [Breaking Changes Overview](#breaking-changes-overview)
- [Migration Steps](#migration-steps)
  - [1. Service Registration Changes](#1-service-registration-changes)
  - [2. Service Retrieval Changes](#2-service-retrieval-changes)
  - [3. DTO Class Name Changes](#3-dto-class-name-changes)
  - [4. Method Signature Changes](#4-method-signature-changes)
  - [5. Namespace Changes](#5-namespace-changes)
  - [6. Exception Handling Changes](#6-exception-handling-changes)
- [Complete Migration Example](#complete-migration-example)
- [New Features](#new-features)
- [FAQ](#faq)

---

## Version Information

| Version | NuGet Package | Compatible Dify API |
|---------|---------------|---------------------|
| v1.x | 1.1.7 and earlier | v0.6.x |
| v2.0 | 2.0.0-alpha.dify1.11.2 | v1.11.2 |

---

## Breaking Changes Overview

| Change | v1.x | v2.0 |
|--------|------|------|
| Service Registration | `AddDifyAiServices()` reads from config file | `AddDifyAi()` fluent multi-instance registration |
| Service Retrieval | Direct injection of `IDifyAiChatServices` | Via `IDifyAiServicesFactory` |
| Instance Count | Single instance (one Bot + one Dataset) | Multiple instances (any number of Bots and Datasets) |
| API Key Override | Each method supports `overrideApiKey` parameter | Removed, each instance bound to fixed ApiKey |
| DTO Naming | `Dify_` prefix (e.g., `Dify_CreateChatCompletionParamDto`) | `Dify` prefix (e.g., `DifyCreateChatCompletionParamDto`) |
| Configuration | appsettings.json configuration section | Explicit code registration |
| Namespace | `DifyAi.Services` | `DifyAi.Interface` |

---

## Migration Steps

### 1. Service Registration Changes

#### v1.x Approach

```csharp
// appsettings.json
{
  "DifyAi": {
    "BaseUrl": "https://api.dify.ai/v1/",
    "BotApiKey": "app-xxxxxxxx",
    "DatasetApiKey": "dataset-xxxxxxxx",
    "ProxyConfig": ""  // Optional
  }
}

// Program.cs
builder.Services.AddDifyAiServices();
```

#### v2.0 Approach

```csharp
// Program.cs
using DifyAi.ServiceExtension;

builder.Services.AddDifyAi(register =>
{
    // Quick registration - uses default BaseUrl (https://api.dify.ai/v1/)
    register.RegisterBot("MyBot", "app-xxxxxxxx");
    register.RegisterDataset("MyDataset", "dataset-xxxxxxxx");

    // Or specify custom BaseUrl
    register.RegisterBot("MyBot", "app-xxxxxxxx", "https://custom.dify.ai/v1/");
});
```

#### v2.0 Advanced Configuration (Proxy and SSL)

```csharp
builder.Services.AddDifyAi(register =>
{
    register.RegisterBot(new DifyAiInstanceConfig
    {
        Name = "MyBot",
        ApiKey = "app-xxxxxxxx",
        BaseUrl = "https://api.dify.ai/v1/",
        ProxyUrl = "socks5://127.0.0.1:8889",      // Optional: Proxy URL
        IgnoreSslErrors = true                      // Optional: Ignore SSL errors (dev only)
    });
});
```

### 2. Service Retrieval Changes

#### v1.x Approach

```csharp
public class ChatController : ControllerBase
{
    private readonly IDifyAiChatServices _chatServices;
    private readonly IDifyAiDatasetServices _datasetServices;

    public ChatController(
        IDifyAiChatServices chatServices,
        IDifyAiDatasetServices datasetServices)
    {
        _chatServices = chatServices;
        _datasetServices = datasetServices;
    }

    public async Task<IActionResult> Chat(string message)
    {
        var result = await _chatServices.CreateChatCompletionBlockModeAsync(
            new Dify_CreateChatCompletionParamDto
            {
                Query = message,
                User = "user123"
            });
        return Ok(result);
    }
}
```

#### v2.0 Approach

```csharp
public class ChatController : ControllerBase
{
    private readonly IDifyAiServicesFactory _servicesFactory;

    public ChatController(IDifyAiServicesFactory servicesFactory)
    {
        _servicesFactory = servicesFactory;
    }

    public async Task<IActionResult> Chat(string message)
    {
        // Get service instance by name
        var chatService = _servicesFactory.GetBotService("MyBot");

        var result = await chatService.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto
            {
                Query = message,
                User = "user123"
            });
        return Ok(result);
    }

    public async Task<IActionResult> GetDatasets()
    {
        var datasetService = _servicesFactory.GetDatasetService("MyDataset");
        var result = await datasetService.GetDatasetListAsync(1, 20);
        return Ok(result);
    }
}
```

### 3. DTO Class Name Changes

All DTO class names changed from `Dify_` prefix to `Dify` prefix (underscore removed).

| v1.x | v2.0 |
|------|------|
| `Dify_CreateChatCompletionParamDto` | `DifyCreateChatCompletionParamDto` |
| `Dify_CreateChatCompletionResDto` | `DifyCreateChatCompletionResDto` |
| `Dify_GetConversationListParamDto` | `DifyGetConversationListParamDto` |
| `Dify_GetConversationListResDto` | `DifyGetConversationListResDto` |
| `Dify_BaseRequestResDto` | `DifyBaseRequestResDto` |
| `Dify_BaseErrorResDto` | `DifyBaseErrorResDto` |
| ... | ... |

**Quick Replace (IDE Global Replace):**

```
Find: Dify_
Replace: Dify
```

### 4. Method Signature Changes

v2.0 removes the `overrideApiKey` parameter from all methods.

#### v1.x Approach

```csharp
// Use default ApiKey
await _chatServices.CreateChatCompletionBlockModeAsync(param);

// Temporarily override ApiKey
await _chatServices.CreateChatCompletionBlockModeAsync(param, "app-another-key");
```

#### v2.0 Approach

```csharp
// Each instance is bound to a fixed ApiKey, cannot be overridden
await chatService.CreateChatCompletionBlockModeAsync(param);

// To use different ApiKeys, register multiple instances
builder.Services.AddDifyAi(register =>
{
    register.RegisterBot("Bot1", "app-key1");
    register.RegisterBot("Bot2", "app-key2");
});

// Retrieve as needed
var bot1 = _servicesFactory.GetBotService("Bot1");
var bot2 = _servicesFactory.GetBotService("Bot2");
```

### 5. Namespace Changes

| v1.x | v2.0 |
|------|------|
| `using DifyAi.Services;` | `using DifyAi.Interface;` |
| `using DifyAi.ServiceExtension;` | `using DifyAi.ServiceExtension;` (unchanged) |

### 6. Exception Handling Changes

v2.0 introduces a custom exception hierarchy.

#### Exception Types

| Exception Class | Purpose |
|-----------------|---------|
| `DifySDKException` | Base class for all SDK exceptions |
| `DifyConfigurationException` | Configuration validation errors (e.g., empty ApiKey) |
| `DifyInstanceNotFoundException` | Requested instance name not registered |
| `DifyInvalidFileException` | File validation failures (e.g., file not found) |

#### Exception Handling Example

```csharp
try
{
    var chatService = _servicesFactory.GetBotService("NonExistentBot");
}
catch (DifyInstanceNotFoundException ex)
{
    // ex.InstanceName = "NonExistentBot"
    // ex.InstanceType = "Bot"
    Console.WriteLine($"Instance '{ex.InstanceName}' is not registered");
}
```

---

## Complete Migration Example

### v1.x Complete Code

```csharp
// appsettings.json
{
  "DifyAi": {
    "BaseUrl": "https://api.dify.ai/v1/",
    "BotApiKey": "app-xxxxxxxx",
    "DatasetApiKey": "dataset-xxxxxxxx"
  }
}

// Program.cs
using DifyAi.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDifyAiServices();
var app = builder.Build();
app.Run();

// ChatController.cs
using DifyAi.Services;

public class ChatController : ControllerBase
{
    private readonly IDifyAiChatServices _chatServices;

    public ChatController(IDifyAiChatServices chatServices)
    {
        _chatServices = chatServices;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        var result = await _chatServices.CreateChatCompletionBlockModeAsync(
            new Dify_CreateChatCompletionParamDto
            {
                Query = request.Message,
                User = request.UserId,
                ConversationId = request.ConversationId
            });

        return Ok(result);
    }
}
```

### v2.0 Complete Code

```csharp
// Program.cs
using DifyAi.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDifyAi(register =>
{
    register.RegisterBot("MyBot", "app-xxxxxxxx", "https://api.dify.ai/v1/");
    register.RegisterDataset("MyDataset", "dataset-xxxxxxxx", "https://api.dify.ai/v1/");
});

var app = builder.Build();
app.Run();

// ChatController.cs
using DifyAi.Interface;

public class ChatController : ControllerBase
{
    private readonly IDifyAiServicesFactory _servicesFactory;

    public ChatController(IDifyAiServicesFactory servicesFactory)
    {
        _servicesFactory = servicesFactory;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        var chatService = _servicesFactory.GetBotService("MyBot");

        var result = await chatService.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto
            {
                Query = request.Message,
                User = request.UserId,
                ConversationId = request.ConversationId
            });

        return Ok(result);
    }
}
```

---

## New Features

v2.0 adds the following API support:

### Chat API

| Method | Description |
|--------|-------------|
| `GetConversationVariablesAsync` | Get conversation variables |
| `GetAppFeedbacksAsync` | Get application feedback list |
| `GetFilePreviewAsync` | Get file preview |
| `TextToAudioAsync` | Text to audio conversion |

### Dataset API

| Method | Description |
|--------|-------------|
| `GetDatasetDetailAsync` | Get dataset details |
| `UpdateDatasetAsync` | Update dataset information |
| `GetDocumentDetailAsync` | Get document details |
| `RetrieveAsync` | Knowledge base retrieval |

---

## FAQ

### Q1: Why was the overrideApiKey parameter removed?

v2.0 adopts a multi-instance architecture where each instance is bound to a fixed ApiKey at registration. Benefits of this design:

1. Clearer instance isolation
2. Avoids runtime ApiKey management confusion
3. Supports HttpClient connection pooling optimization
4. Better thread safety

To use multiple ApiKeys, register multiple instances.

### Q2: How to read ApiKey from configuration file?

```csharp
var config = builder.Configuration;

builder.Services.AddDifyAi(register =>
{
    register.RegisterBot(
        "MyBot",
        config["DifyAi:BotApiKey"],
        config["DifyAi:BaseUrl"]);
});
```

### Q3: Are there restrictions on instance names?

Instance names are strings. Meaningful names are recommended for maintainability. Same-type instance names cannot be duplicated (e.g., two Bots cannot share the same name), but a Bot and Dataset can share the same name.

### Q4: How to handle non-existent instances?

```csharp
try
{
    var service = _servicesFactory.GetBotService("UnknownBot");
}
catch (DifyInstanceNotFoundException ex)
{
    _logger.LogError("Bot instance '{Name}' is not registered", ex.InstanceName);
    // Handle error...
}
```

### Q5: Is v2.0 backward compatible?

No. v2.0 is a breaking change release. You must follow this guide to migrate. Main incompatibilities:

- Service registration method completely changed
- Service retrieval method completely changed
- All DTO class names changed
- All method signatures changed (overrideApiKey removed)
