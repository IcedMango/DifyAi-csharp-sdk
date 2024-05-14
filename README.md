# Description
English | [中文](./README.zh-CN.md)

A simple [Dify](https://dify.ai/) C# SDK.

Currently support partial robot api.

Wiki api working in progress.

UnitTest missing, will be added soon. If you have encountered any problem, please feel free to open an issue or PR.

# Important Notice

**This project is under early development, and the API may change at any time. You may encounter bugs, missing features, or incomplete documentation.**

# QuickStart

## Install

```
Install-Package IcedMango.DifyAi 
```

## Service Registration:
### Startup.cs
```csharp

//Startup.cs

using DifyAi.ServiceExtension;


public void ConfigureServices(IServiceCollection services)
{
    ...other code
    
    // register service
    services.AddDifyAiServices();
}

```

### appsetting.json

Note: 
- BaseUrl: EndPoint of dify api instance. **Must end with /**
- BotApiKey: You can start with 'Bearer app-your-bot-key' or just use 'app-your-bot-key'
- Proxy: Proxy setting, support http, https, socks5. Leave empty if you don't need it.

```json
{
  "DifyAi": {
    "BaseUrl": "https://example.com/v1/", 
    "BotApiKey": "app-your-bot-key",
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
    private readonly IDifyAiChatServices _difyAiChatServices;

    public TestClass(IDifyAiChatServices difyAiChatServices)
    {
        _difyAiChatServices = difyAiChatServices;
    }

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
}


```


## Todo List
- [ ] Robot Api(partially done)
- [ ] UnitTest
- [ ] Api Document
- [ ] Message Completion Stream Mode
- [ ] Wiki Related Api

# License
This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.

# Contribution License Agreement
By contributing, you agree to license your contribution to project owner under the MIT license.
