namespace IcedMango.DifyAi.Tests.Exceptions;

/// <summary>
/// Unit tests for DifyConfigurationException
/// </summary>
public class DifyConfigurationExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Configuration error";

        // Act
        var exception = new DifyConfigurationException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.PropertyName.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithPropertyName_ShouldSetPropertyName()
    {
        // Arrange
        var message = "ApiKey cannot be empty";
        var propertyName = "ApiKey";

        // Act
        var exception = new DifyConfigurationException(message)
        {
            PropertyName = propertyName
        };

        // Assert
        exception.PropertyName.Should().Be(propertyName);
    }

    [Fact]
    public void Constructor_WithInnerException_ShouldPreserveInner()
    {
        // Arrange
        var message = "Configuration error";
        var innerException = new ArgumentException("Invalid argument");

        // Act
        var exception = new DifyConfigurationException(message, innerException);

        // Assert
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void PropertyName_WhenNull_ShouldBeNull()
    {
        // Arrange & Act
        var exception = new DifyConfigurationException("Error");

        // Assert
        exception.PropertyName.Should().BeNull();
    }

    [Fact]
    public void InheritsFromDifySDKException()
    {
        // Arrange & Act
        var exception = new DifyConfigurationException("Error");

        // Assert
        exception.Should().BeAssignableTo<DifySDKException>();
    }
}
