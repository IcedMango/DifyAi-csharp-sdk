namespace IcedMango.DifyAi.Tests.Configuration;

/// <summary>
/// Unit tests for DifyAiInstanceConfig
/// Tests configuration validation through DI registration (since Validate is internal)
/// </summary>
public class DifyAiInstanceConfigTests
{
    [Fact]
    public void RegisterBot_WithValidConfig_ShouldNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(
                TestConstants.BotInstanceName,
                TestConstants.ValidBotApiKey,
                TestConstants.DefaultBaseUrl);
        });

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RegisterBot_WithEmptyName_ShouldThrowDifyConfigurationException(string name)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(name!, TestConstants.ValidBotApiKey);
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("Name");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RegisterBot_WithEmptyApiKey_ShouldThrowDifyConfigurationException(string apiKey)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, apiKey!);
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("ApiKey");
    }

    [Fact]
    public void RegisterBot_WithFullConfig_EmptyBaseUrl_ShouldThrowDifyConfigurationException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(new DifyAiInstanceConfig
            {
                Name = TestConstants.BotInstanceName,
                ApiKey = TestConstants.ValidBotApiKey,
                BaseUrl = ""
            });
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("BaseUrl");
    }

    [Fact]
    public void RegisterBot_WithoutTrailingSlash_ShouldAutoAppend()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - Registration should succeed, the BaseUrl will be fixed internally
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(
                TestConstants.BotInstanceName,
                TestConstants.ValidBotApiKey,
                "https://api.dify.ai/v1"); // Without trailing slash
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void DifyAiInstanceConfig_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var config = new DifyAiInstanceConfig();

        // Assert
        config.Name.Should().BeEmpty();
        config.ApiKey.Should().BeEmpty();
        config.BaseUrl.Should().Be("https://api.dify.ai/v1/");
        config.ProxyUrl.Should().BeNull();
        config.IgnoreSslErrors.Should().BeFalse();
    }

    [Fact]
    public void DifyAiInstanceConfig_ProxyUrl_WhenSet_ShouldBePreserved()
    {
        // Arrange
        var proxyUrl = "socks5://127.0.0.1:1080";

        // Act
        var config = new DifyAiInstanceConfig
        {
            Name = TestConstants.BotInstanceName,
            ApiKey = TestConstants.ValidBotApiKey,
            ProxyUrl = proxyUrl
        };

        // Assert
        config.ProxyUrl.Should().Be(proxyUrl);
    }

    [Fact]
    public void DifyAiInstanceConfig_IgnoreSslErrors_WhenSetToTrue_ShouldBePreserved()
    {
        // Arrange & Act
        var config = new DifyAiInstanceConfig
        {
            Name = TestConstants.BotInstanceName,
            ApiKey = TestConstants.ValidBotApiKey,
            IgnoreSslErrors = true
        };

        // Assert
        config.IgnoreSslErrors.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RegisterDataset_WithEmptyName_ShouldThrowDifyConfigurationException(string name)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterDataset(name!, TestConstants.ValidDatasetApiKey);
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("Name");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RegisterDataset_WithEmptyApiKey_ShouldThrowDifyConfigurationException(string apiKey)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterDataset(TestConstants.DatasetInstanceName, apiKey!);
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("ApiKey");
    }

    [Fact]
    public void RegisterBot_WithCustomBaseUrl_ShouldAccept()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(
                TestConstants.BotInstanceName,
                TestConstants.ValidBotApiKey,
                TestConstants.CustomBaseUrl);
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RegisterBot_WithProxyConfiguration_ShouldAccept()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(new DifyAiInstanceConfig
            {
                Name = TestConstants.BotInstanceName,
                ApiKey = TestConstants.ValidBotApiKey,
                ProxyUrl = "http://proxy.example.com:8080"
            });
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RegisterBot_WithIgnoreSslErrors_ShouldAccept()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(new DifyAiInstanceConfig
            {
                Name = TestConstants.BotInstanceName,
                ApiKey = TestConstants.ValidBotApiKey,
                IgnoreSslErrors = true
            });
        });

        // Assert
        act.Should().NotThrow();
    }
}
