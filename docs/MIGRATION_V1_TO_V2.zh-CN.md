# IcedMango.DifyAi v1.x 到 v2.0 迁移指南

本文档详细说明从 v1.x 版本迁移到 v2.0 版本所需的全部变更。

## 目录

- [版本信息](#版本信息)
- [主要变更概览](#主要变更概览)
- [迁移步骤](#迁移步骤)
  - [1. 服务注册方式变更](#1-服务注册方式变更)
  - [2. 服务获取方式变更](#2-服务获取方式变更)
  - [3. DTO 类名变更](#3-dto-类名变更)
  - [4. 方法签名变更](#4-方法签名变更)
  - [5. 命名空间变更](#5-命名空间变更)
  - [6. 异常处理变更](#6-异常处理变更)
- [完整迁移示例](#完整迁移示例)
- [新增功能](#新增功能)
- [常见问题](#常见问题)

---

## 版本信息

| 版本 | NuGet 包版本 | 兼容 Dify API |
|------|-------------|-------------|
| v1.x | 1.1.7 及更早 | v0.6.x      |
| v2.0 | 2.0.0-alpha.dify1.11.2 | v1.11.2     |

---

## 主要变更概览

| 变更项 | v1.x | v2.0 |
|--------|------|------|
| 服务注册 | `AddDifyAiServices()` 读取配置文件 | `AddDifyAi()` 流式注册多实例 |
| 服务获取 | 直接注入 `IDifyAiChatServices` | 通过 `IDifyAiServicesFactory` 获取 |
| 实例数量 | 单实例（一个 Bot + 一个 Dataset） | 多实例（任意数量 Bot 和 Dataset） |
| API Key 覆盖 | 每个方法支持 `overrideApiKey` 参数 | 移除，每个实例绑定固定 ApiKey |
| DTO 命名 | `Dify_` 前缀（如 `Dify_CreateChatCompletionParamDto`） | `Dify` 前缀（如 `DifyCreateChatCompletionParamDto`） |
| 配置方式 | appsettings.json 配置节 | 代码中显式注册 |
| 命名空间 | `DifyAi.Services` | `DifyAi.Interface` |

---

## 迁移步骤

### 1. 服务注册方式变更

#### v1.x 方式

```csharp
// appsettings.json
{
  "DifyAi": {
    "BaseUrl": "https://api.dify.ai/v1/",
    "BotApiKey": "app-xxxxxxxx",
    "DatasetApiKey": "dataset-xxxxxxxx",
    "ProxyConfig": ""  // 可选
  }
}

// Program.cs
builder.Services.AddDifyAiServices();
```

#### v2.0 方式

```csharp
// Program.cs
using DifyAi.ServiceExtension;

builder.Services.AddDifyAi(register =>
{
    // 快速注册 - 使用默认 BaseUrl (https://api.dify.ai/v1/)
    register.RegisterBot("MyBot", "app-xxxxxxxx");
    register.RegisterDataset("MyDataset", "dataset-xxxxxxxx");

    // 或者指定自定义 BaseUrl
    register.RegisterBot("MyBot", "app-xxxxxxxx", "https://custom.dify.ai/v1/");
});
```

#### v2.0 高级配置（代理和 SSL）

```csharp
builder.Services.AddDifyAi(register =>
{
    register.RegisterBot(new DifyAiInstanceConfig
    {
        Name = "MyBot",
        ApiKey = "app-xxxxxxxx",
        BaseUrl = "https://api.dify.ai/v1/",
        ProxyUrl = "socks5://127.0.0.1:8889",      // 可选：代理地址
        IgnoreSslErrors = true                      // 可选：忽略 SSL 证书错误（仅开发环境）
    });
});
```

### 2. 服务获取方式变更

#### v1.x 方式

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

#### v2.0 方式

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
        // 通过名称获取服务实例
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

### 3. DTO 类名变更

所有 DTO 类名从 `Dify_` 前缀改为 `Dify` 前缀（移除下划线）。

| v1.x | v2.0 |
|------|------|
| `Dify_CreateChatCompletionParamDto` | `DifyCreateChatCompletionParamDto` |
| `Dify_CreateChatCompletionResDto` | `DifyCreateChatCompletionResDto` |
| `Dify_GetConversationListParamDto` | `DifyGetConversationListParamDto` |
| `Dify_GetConversationListResDto` | `DifyGetConversationListResDto` |
| `Dify_BaseRequestResDto` | `DifyBaseRequestResDto` |
| `Dify_BaseErrorResDto` | `DifyBaseErrorResDto` |
| ... | ... |

**快速替换方法（IDE 全局替换）：**

```
查找: Dify_
替换: Dify
```

### 4. 方法签名变更

v2.0 移除了所有方法的 `overrideApiKey` 参数。

#### v1.x 方式

```csharp
// 使用默认 ApiKey
await _chatServices.CreateChatCompletionBlockModeAsync(param);

// 临时覆盖 ApiKey
await _chatServices.CreateChatCompletionBlockModeAsync(param, "app-another-key");
```

#### v2.0 方式

```csharp
// 每个实例绑定固定的 ApiKey，无需也无法覆盖
await chatService.CreateChatCompletionBlockModeAsync(param);

// 如需使用不同的 ApiKey，注册多个实例
builder.Services.AddDifyAi(register =>
{
    register.RegisterBot("Bot1", "app-key1");
    register.RegisterBot("Bot2", "app-key2");
});

// 使用时按需获取
var bot1 = _servicesFactory.GetBotService("Bot1");
var bot2 = _servicesFactory.GetBotService("Bot2");
```

### 5. 命名空间变更

| v1.x | v2.0 |
|------|------|
| `using DifyAi.Services;` | `using DifyAi.Interface;` |
| `using DifyAi.ServiceExtension;` | `using DifyAi.ServiceExtension;` (保持不变) |

### 6. 异常处理变更

v2.0 引入了自定义异常层次结构。

#### 异常类型

| 异常类 | 用途 |
|--------|------|
| `DifySDKException` | 基类，所有 SDK 异常的父类 |
| `DifyConfigurationException` | 配置验证错误（如 ApiKey 为空） |
| `DifyInstanceNotFoundException` | 请求的实例名称未注册 |
| `DifyInvalidFileException` | 文件验证失败（如文件不存在） |

#### 异常处理示例

```csharp
try
{
    var chatService = _servicesFactory.GetBotService("NonExistentBot");
}
catch (DifyInstanceNotFoundException ex)
{
    // ex.InstanceName = "NonExistentBot"
    // ex.InstanceType = "Bot"
    Console.WriteLine($"实例 '{ex.InstanceName}' 未注册");
}
```

---

## 完整迁移示例

### v1.x 完整代码

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

### v2.0 完整代码

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

## 新增功能

v2.0 新增了以下 API 支持：

### Chat API

| 方法 | 说明 |
|------|------|
| `GetConversationVariablesAsync` | 获取会话变量 |
| `GetAppFeedbacksAsync` | 获取应用反馈列表 |
| `GetFilePreviewAsync` | 获取文件预览 |
| `TextToAudioAsync` | 文本转语音 |

### Dataset API

| 方法 | 说明 |
|------|------|
| `GetDatasetDetailAsync` | 获取数据集详情 |
| `UpdateDatasetAsync` | 更新数据集信息 |
| `GetDocumentDetailAsync` | 获取文档详情 |
| `RetrieveAsync` | 知识库检索 |

---

## 常见问题

### Q1: 为什么移除 overrideApiKey 参数?

v2.0 采用多实例架构，每个实例在注册时绑定固定的 ApiKey。这样设计的优点：

1. 更清晰的实例隔离
2. 避免运行时 ApiKey 管理混乱
3. 支持 HttpClient 连接池优化
4. 更好的线程安全性

如需使用多个 ApiKey，请注册多个实例。

### Q2: 如何从配置文件读取 ApiKey?

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

### Q3: 实例名称有什么限制?

实例名称是字符串类型，建议使用有意义的名称便于维护。同类型实例名称不能重复（如两个 Bot 不能同名），但 Bot 和 Dataset 可以同名。

### Q4: 如何处理实例不存在的情况?

```csharp
try
{
    var service = _servicesFactory.GetBotService("UnknownBot");
}
catch (DifyInstanceNotFoundException ex)
{
    _logger.LogError("Bot 实例 '{Name}' 未注册", ex.InstanceName);
    // 处理错误...
}
```

### Q5: v2.0 是否向后兼容?

不兼容。v2.0 是破坏性更新，必须按照本指南进行迁移。主要不兼容项：

- 服务注册方式完全改变
- 服务获取方式完全改变
- 所有 DTO 类名改变
- 所有方法签名改变（移除 overrideApiKey）
