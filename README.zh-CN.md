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

# 介绍

[![简体中文](https://img.shields.io/badge/简体中文-green)](https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.zh-CN.md)
[![English](https://img.shields.io/badge/English-red)](https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.md)

一个Dify的C# SDK，用于与Dify的API进行交互。 支持聊天/知识库API。

如果你遇到任何问题，请随时提出Issue或PR。

# 快速开始

## 安装

```

Install-Package IcedMango.DifyAi

```

## 服务注册：

### Startup.cs

```csharp
//Startup.cs

using DifyAi.ServiceExtension;

public void ConfigureServices(IServiceCollection services)
{

    ...其他代码

    // 注册服务
    services.AddDifyAiServices();
}

```

### 配置文件 (appsettings.json)

**必须填写下面的配置**

这里提供了一段示例配置，你需要根据实际情况进行修改。

注意：

- BaseUrl：【必填】Dify API实例的Url地址。**必须以`/`结尾**。
- BotApiKey：【必填】你的机器人api Key。
- DatasetApiKey：你的知识库api Key。
- Proxy：代理设置，支持http、https、socks5。如果不需要，请留空。

```json
{
  "DifyAi": {
    "BaseUrl": "https://example.com/v1/",
    "BotApiKey": "app-your-bot-key",
    "DatasetApiKey": "dataset-your-dataset-key",
    "Proxy": "socks5://127.0.0.1:8889"
  }
}
```

## 用法

```csharp
using DifyAi.Dto.ParamDto;
using DifyAi.Services;

namespace TestDifyAi;
public class TestClass
{
    // 聊天机器人开放api
    private readonly IDifyAiChatServices _difyAiChatServices;
    
    // 知识库开放api
    private readonly IDifyAiDatasetServices _difyAiDatasetServices;
    public TestClass(IDifyAiChatServices difyAiChatServices, IDifyAiDatasetServices difyAiDatasetServices)
    {
        _difyAiChatServices = difyAiChatServices;
        _difyAiDatasetServices = difyAiDatasetServices;
    }

    // 聊天机器人
    public async Task<string> TestCompletion()
    {
        var res = await _difyAiChatServices.CreateChatCompletionBlockModeAsync(new Dify_CreateChatCompletionParamDto()
        {
            Query = "Who are you?",
            User = "IcedMango",
            ConversationId = string.Empty
        });
        
        if (res.Success == true)
        {
            return res.Data.Answer;
        }

        return "Error";
    }
    
    // 添加知识库文档
    public async Task<bool> AddDatasetDocAsync()
    {
        var difyAiDto = new Dify_CreateDocumentByTextParamDto() 
        {
            DatasetId = "your-dataset-id",
            Text = "who are you? Why are you here? I am a bot.",
            Name = "About me",
            IsAutomaticProcess = true,
            EnableHighQualityIndex = true
        };
        
        var res = await _datasetServices.CreateDocumentByTextAsync(difyAiDto);
        if (res.Success == true)
        {
            var docInfo = res.Data.Document;
            return true;
        }
        return false;
    }
}
```


## 待办
- [ ] 单元测试
- [ ] 消息完成流模式

# 开源协议

本项目根据MIT许可证获得许可 - 有关详细信息，请参阅[LICENSE](./LICENSE)文件。

# 贡献许可协议(CLA)

如提交PR，则默认同意在MIT许可下授权您对本项目的所有贡献到项目所有者。