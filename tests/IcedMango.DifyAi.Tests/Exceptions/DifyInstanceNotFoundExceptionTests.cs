namespace IcedMango.DifyAi.Tests.Exceptions;

/// <summary>
/// Unit tests for DifyInstanceNotFoundException
/// </summary>
public class DifyInstanceNotFoundExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetInstanceName()
    {
        // Arrange
        var instanceName = "TestBot";
        var instanceType = "Bot";

        // Act
        var exception = new DifyInstanceNotFoundException(instanceName, instanceType);

        // Assert
        exception.InstanceName.Should().Be(instanceName);
    }

    [Fact]
    public void Constructor_ShouldSetInstanceType()
    {
        // Arrange
        var instanceName = "TestDataset";
        var instanceType = "Dataset";

        // Act
        var exception = new DifyInstanceNotFoundException(instanceName, instanceType);

        // Assert
        exception.InstanceType.Should().Be(instanceType);
    }

    [Fact]
    public void Message_ShouldIncludeInstanceName()
    {
        // Arrange
        var instanceName = "CustomerService";
        var instanceType = "Bot";

        // Act
        var exception = new DifyInstanceNotFoundException(instanceName, instanceType);

        // Assert
        exception.Message.Should().Contain(instanceName);
    }

    [Fact]
    public void Message_ShouldIncludeInstanceType()
    {
        // Arrange
        var instanceName = "FAQ";
        var instanceType = "Dataset";

        // Act
        var exception = new DifyInstanceNotFoundException(instanceName, instanceType);

        // Assert
        exception.Message.Should().Contain(instanceType);
    }

    [Fact]
    public void Message_ShouldSuggestRegistration()
    {
        // Arrange
        var instanceName = "TestBot";
        var instanceType = "Bot";

        // Act
        var exception = new DifyInstanceNotFoundException(instanceName, instanceType);

        // Assert
        exception.Message.Should().Contain("Register");
    }

    [Fact]
    public void InheritsFromDifySDKException()
    {
        // Arrange & Act
        var exception = new DifyInstanceNotFoundException("Test", "Bot");

        // Assert
        exception.Should().BeAssignableTo<DifySDKException>();
    }

    [Theory]
    [InlineData("Bot")]
    [InlineData("Dataset")]
    public void InstanceType_ShouldIndicateCorrectType(string instanceType)
    {
        // Arrange & Act
        var exception = new DifyInstanceNotFoundException("Test", instanceType);

        // Assert
        exception.InstanceType.Should().Be(instanceType);
    }
}
