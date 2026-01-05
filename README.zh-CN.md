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
> 当前 SDK 处于 alpha 阶段，尚未经过完全测试。如有问题欢迎提交 Issue 或 PR！

# 介绍

[![简体中文](https://img.shields.io/badge/简体中文-green)](https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.zh-CN.md)
[![English](https://img.shields.io/badge/English-red)](https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.md)

Dify的C# SDK, 提供聊天机器人交互/知识库管理的API支持。

**版本**: `2.0.0-alpha.dify1.11.2` (兼容 Dify API v1.11.2)

**核心特性**:
- **多实例支持**: 管理多个不同 API Key 的 Bot/Dataset 实例
- **DI 模式 + 工厂模式**: 无缝集成 ASP.NET Core 依赖注入
- **全面覆盖**: 聊天、知识库、文件上传、音频处理
- **现代 .NET**: 支持 .NET 6.0、7.0、8.0

**V2.0 破坏性变更**
- **新 DI 架构**: V2.0 使用工厂模式,通过 `AddDifyAi()` 注册, 完全废弃1.0时使用方式
- **移除 `overrideApiKey`**: 每个实例在注册时绑定其 API Key
- **迁移指南**: [中文](./docs/MIGRATION_V1_TO_V2.zh-CN.md) | [English](./docs/MIGRATION_V1_TO_V2.md)

# 快速开始

## 安装

```bash
dotnet add package IcedMango.DifyAi
# 或
Install-Package IcedMango.DifyAi
```

## 使用方式: DI 模式 + 工厂模式

**适用场景**: ASP.NET Core 应用程序 - 通过依赖注入管理多个 Dify 实例

### 步骤 1: 在 Program.cs 中注册服务

```csharp
using DifyAi.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// 注册 Dify AI 服务,支持多实例
builder.Services.AddDifyAi(register =>
{
    // 快速注册 - Bot 实例
    register.RegisterBot("CustomerService", "app-xxx");
    register.RegisterBot("TechSupport", "app-yyy", "https://custom.dify.ai/v1/");

    // 快速注册 - Dataset 实例
    register.RegisterDataset("ProductDocs", "dataset-xxx");
    register.RegisterDataset("FAQ", "dataset-yyy");

    // 高级配置:使用代理和 SSL 设置注册
    register.RegisterBot(new DifyAiInstanceConfig
    {
        Name = "ProxyBot",
        ApiKey = "app-zzz",
        BaseUrl = "https://api.dify.ai/v1/",
        ProxyUrl = "socks5://127.0.0.1:8889",
        IgnoreSslErrors = true  // 仅用于开发/测试环境
    });
});

var app = builder.Build();
app.Run();
```

### 步骤 2: 通过依赖注入使用服务

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
        // 根据实例名称获取 Bot 服务
        var chatService = _servicesFactory.GetBotService("CustomerService");

        // 聊天对话
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
        // 动态使用不同实例
        var techBot = _servicesFactory.GetBotService("TechSupport");
        var result = await techBot.CreateChatCompletionBlockModeAsync(
            new DifyCreateChatCompletionParamDto { Query = question, User = "user123" });

        return Ok(result);
    }
}
```

## 核心功能与示例

### 1. 聊天 API

```csharp
var chatService = _servicesFactory.GetBotService("CustomerService");

// 阻塞模式聊天
var chatResult = await chatService.CreateChatCompletionBlockModeAsync(
    new DifyCreateChatCompletionParamDto
    {
        Query = "你好,你是谁?",
        User = "user123"
    });

// 停止聊天
await chatService.StopChatCompletionAsync(new DifyStopChatCompletionParamDto
{
    TaskId = "task-xxx",
    User = "user123"
});

// 获取会话列表
var conversations = await chatService.GetConversationListAsync(
    new DifyGetConversationListParamDto
    {
        User = "user123",
        Limit = 20
    });

// 音频转文字
var audioResult = await chatService.AudioToTextAsync(new DifyAudioToTextParamDto
{
    FilePath = "/path/to/audio.mp3",
    User = "user123"
});

// 文字转音频
var ttsResult = await chatService.TextToAudioAsync(new DifyTextToAudioParamDto
{
    MessageId = "msg-xxx",
    Text = "你好",
    User = "user123"
});

// 获取会话变量
var variables = await chatService.GetConversationVariablesAsync(
    new DifyGetConversationVariablesParamDto
    {
        ConversationId = "conv-xxx",
        User = "user123",
        Limit = 20
    });

// 获取文件预览
var filePreview = await chatService.GetFilePreviewAsync("file-xxx", "user123");
```

### 2. 知识库 API

```csharp
var datasetService = _servicesFactory.GetDatasetService("ProductDocs");

// 创建数据集
var dataset = await datasetService.CreateDatasetAsync(new DifyCreateDatasetParamDto
{
    Name = "我的数据集",
    Description = "产品文档"
});

// 更新数据集
await datasetService.UpdateDatasetAsync(new DifyUpdateDatasetParamDto
{
    DatasetId = "dataset-xxx",
    Name = "更新后的名称",
    Description = "更新后的描述"
});

// 获取数据集列表(支持分页)
var datasets = await datasetService.GetDatasetListAsync(1, 20);

// 获取数据集详情
var detail = await datasetService.GetDatasetDetailAsync("dataset-xxx");

// 通过文本创建文档
var doc = await datasetService.CreateDocumentByTextAsync(
    new DifyCreateDocumentByTextParamDto
    {
        DatasetId = "dataset-id",
        Text = "文档内容",
        Name = "文档名称",
        IsAutomaticProcess = true
    });

// 通过文件创建文档
var fileDoc = await datasetService.CreateDocumentByFileAsync(
    new DifyCreateDocumentByFileParamDto
    {
        DatasetId = "dataset-id",
        FilePath = "/path/to/file.pdf",
        Name = "文件文档",
        IsAutomaticProcess = true
    });

// 获取文档列表
var documents = await datasetService.GetDocumentListAsync(
    new DifyGetDocumentListParamDto
    {
        DatasetId = "dataset-xxx",
        Page = 1,
        Limit = 20
    });

// 获取文档详情
var docDetail = await datasetService.GetDocumentDetailAsync("dataset-xxx", "doc-xxx");

// 从知识库检索
var retrieveResult = await datasetService.RetrieveAsync(new DifyRetrieveParamDto
{
    DatasetId = "dataset-id",
    Query = "搜索查询",
    TopK = 3
});

// 添加文档片段
await datasetService.AddDocumentSegmentAsync(new DifyAddDocumentSegmentParamDto
{
    DatasetId = "dataset-xxx",
    DocumentId = "doc-xxx",
    Content = "片段内容"
});

// 更新文档片段
await datasetService.UpdateDocumentSegmentAsync(
    new DifyUpdateDocumentSegmentParamDto
    {
        DatasetId = "dataset-xxx",
        DocumentId = "doc-xxx",
        SegmentId = "segment-xxx",
        Content = "更新后的内容"
    });
```

### 3. 多实例管理

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
        // 根据部门动态选择 Bot 实例
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
        // 动态选择 Dataset 实例
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

## API 文档

详细的 API 文档请参阅:
- [API 文档](./ApiDoc.md) | [API 文档 (中文)](./ApiDoc.zh-CN.md)

## V2.0 破坏性变更

### 1. 移除 `overrideApiKey` 参数
```csharp
// 旧版本 V1.x (不再支持)
await chatService.CreateChatCompletionAsync(param, overrideApiKey: "app-yyy");

// 新版本 V2.0 (注册多个实例)
builder.Services.AddDifyAi(register =>
{
    register.RegisterBot("Bot1", "app-xxx");
    register.RegisterBot("Bot2", "app-yyy");
});

var bot1 = _factory.GetBotService("Bot1");
var bot2 = _factory.GetBotService("Bot2");
```

### 2. DTO 命名规范
```csharp
// 旧版本 V1.x
new Dify_CreateChatCompletionParamDto()

// 新版本 V2.0
new DifyCreateChatCompletionParamDto()
```

### 3. 移除静态容器
```csharp
// 旧版本 V1.x (静态容器)
DifyAiContainer.RegisterBot("Bot1", "app-xxx");
var service = DifyAiContainer.GetBotService("Bot1");

// 新版本 V2.0 (DI 模式)
builder.Services.AddDifyAi(register => register.RegisterBot("Bot1", "app-xxx"));
// 在 Controller/Service 中注入 IDifyAiServicesFactory
```


## 贡献

欢迎提交 Issue 和 Pull Request!

## 开源协议

本项目基于 MIT 许可证 - 详见 [LICENSE](./LICENSE) 文件。

## 贡献许可协议 (CLA)

提交 PR 即表示同意在 MIT 许可下将您的贡献授权给项目所有者。
