# 描述

[English](./README.md) | 中文

一个简单的 [Dify](https://dify.ai/) C# SDK。

目前支持部分聊天机器人API，知识库相关API正在开发中

缺少单元测试，将在后续添加。如果你遇到任何问题，请随时提出Issue或PR。

# 重要通知

**该项目处于早期开发阶段，API可能随时更改。可能会遇到错误、缺失功能或文档不完整的问题。**

# 快速开始

## 安装(暂未发布, 不可用)

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

### appsetting.json

这里提供了一段示例配置，你需要根据实际情况进行修改。

注意：

- BaseUrl：Dify API实例的Url地址。**必须以`/`结尾**。

- BotApiKey：可以包含'Bearer app-your-bot-key'或者只使用 'app-your-bot-key'。

- Proxy：代理设置，支持http、https、socks5。如果不需要，请留空。

```json
{
    "DifyAi": {
        "BaseUrl": "https://example.com/v1/",
        "BotApiKey": "app-your-bot-key",
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
    private readonly IDifyAiChatServices _difyAiChatServices;
    public TestClass(IDifyAiChatServices difyAiChatServices)
    {
        _difyAiChat Services = difyAiChatServices;
    }

    public async Task<string> TestCompletion()
    {
        var res = await _difyAiChatServices. CreateChatCompletionBlockModeAsync(new Dify_CreateChatCompletionParamDto()
        {
            Query = "Who are you?",
            User = "IcedMango",
            ConversationId = string.Empty
        });
        
        if (res.Success == true)
        {
            return res.Answer;
        }

        return "Error";
    }
}
```
## 待办
- [ ] 机器人API（部分完成）
- [ ] 单元测试
- [ ] API文档
- [ ] 消息完成流模式
- [ ] Wiki相关API

# 开源协议

本项目根据MIT许可证获得许可 - 有关详细信息，请参阅[LICENSE](./LICENSE)文件。

# 贡献许可协议(CLA)

如提交PR，则默认同意在MIT许可下授权您对本项目的所有贡献到项目所有者。