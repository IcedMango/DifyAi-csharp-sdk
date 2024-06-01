<h1 style="text-align: center;">Dify C# SDK</h1>
<p align="center">
    <a href="https://www.nuget.org/packages/IcedMango.DifyAi" target="_blank"><img alt="Nuget Version" src="https://img.shields.io/badge/OpenSource-Github-blue"></a>
    <a href="https://www.nuget.org/packages/IcedMango.DifyAi" target="_blank"><img alt="Nuget Version" src="https://img.shields.io/nuget/v/IcedMango.DifyAi"></a>
    <a href="https://www.nuget.org/packages/IcedMango.DifyAi" target="_blank"><img alt="Nuget Download" src="https://img.shields.io/nuget/dt/IcedMango.DifyAi"/></a>
    <a href="https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/LICENSE" target="_blank"><img alt="GitHub License" src="https://img.shields.io/github/license/IcedMango/DifyAi-csharp-sdk"/></a>
    <a href="https://github.com/IcedMango/DifyAi-csharp-sdk/actions" target="_blank"><img alt="Action Status" src="https://img.shields.io/github/actions/workflow/status/IcedMango/DifyAi-csharp-sdk/publishNuGet.yml"/></a>
    <a href="https://img.shields.io/github/license/IcedMango/DifyAi-csharp-sdk" target="_blank"><img alt="Commit Status" src="https://img.shields.io/github/commit-activity/m/IcedMango/DifyAi-csharp-sdk?labelColor=%20%2332b583&color=%20%2312b76a"/></a>
</p>
<p align="center">
    <img alt="Language" src="https://img.shields.io/github/languages/top/IcedMango/DifyAi-csharp-sdk"/>
    <img alt=".NET-6.0" src="https://img.shields.io/badge/.NET-6.0-blue"/>
    <img alt=".NET-7.0" src="https://img.shields.io/badge/.NET-7.0-blue"/>
    <img alt=".NET-8.0" src="https://img.shields.io/badge/.NET-8.0-blue"/>
</p>
<p align="center">
    <a href="https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.zh-CN.md" target="_blank"><img alt="简体中文" src="https://img.shields.io/badge/简体中文-green"></a>
    <a href="https://github.com/IcedMango/DifyAi-csharp-sdk/blob/main/README.md" target="_blank"><img alt="English" src="https://img.shields.io/badge/English-red"></a>
</p>

# Introduction

A Dify C# SDK for interacting with Dify's APIs.

Supports Chat/Knowledge Base APIs.

If you encounter any issues, feel free to raise an Issue or PR.

# Quick Start

## Installation

```
Install-Package IcedMango.DifyAi
```

## Service Registration:

### Startup.cs

```csharp
// Startup.cs

using DifyAi.ServiceExtension;

public void ConfigureServices(IServiceCollection services)
{
    ...other code

    // Register services
    services.AddDifyAiServices();
}
```

### Configuration File (appsettings.json)

**The following configurations must be filled in**

Here is a sample configuration that you need to modify according to your actual situation.

Note:

- BaseUrl: **[Required]** The URL of the Dify API instance. **Must end with a `/`**.
- BotApiKey: **[Required]** Your bot API key.
- DatasetApiKey: Your knowledge base API key.
- Proxy: Proxy settings, supports http, https, socks5. If not needed, leave it blank.

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

## Usage

```csharp
using DifyAi.Dto.ParamDto;
using DifyAi.Services;

namespace TestDifyAi;
public class TestClass
{
    // Chat bot public API
    private readonly IDifyAiChatServices _difyAiChatServices;
    
    // Knowledge base public API
    private readonly IDifyAiDatasetServices _difyAiDatasetServices;
    
    public TestClass(IDifyAiChatServices difyAiChatServices, IDifyAiDatasetServices difyAiDatasetServices)
    {
        _difyAiChatServices = difyAiChatServices;
        _difyAiDatasetServices = difyAiDatasetServices;
    }

    // Chat bot
    public async Task<string> TestCompletion()
    {
        var res = await _difyAiChatServices.CreateChatCompletionBlockModeAsync(new Dify_CreateChatCompletionParamDto()
        {
            Query = "Who are you?",
            User = "IcedMango",
            ConversationId = string.Empty
        });
        
        if (res.Success)
        {
            return res.Data.Answer;
        }

        return "Error";
    }
    
    // Add knowledge base document
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
        
        var res = await _difyAiDatasetServices.CreateDocumentByTextAsync(difyAiDto);
        if (res.Success)
        {
            var docInfo = res.Data.Document;
            return true;
        }
        return false;
    }
}
```

## API Documentation

[Click to view](ApiDoc.zh-CN.md)

## TODO
- [ ] Unit Tests
- [ ] Message Completion Streaming Mode

# License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.

# Contributor License Agreement (CLA)

By submitting a PR, you agree to license your contributions to the project owner under the MIT License.
