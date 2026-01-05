namespace IcedMango.DifyAi.Tests.Exceptions;

/// <summary>
/// Unit tests for DifySDKException
/// </summary>
public class DifySDKExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var exception = new DifySDKException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.StatusCode.Should().BeNull();
        exception.ErrorCode.Should().BeNull();
        exception.RawResponse.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithMessageAndInner_ShouldSetBoth()
    {
        // Arrange
        var message = "Test error message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new DifySDKException(message, innerException);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void Constructor_WithStatusCode_ShouldSetStatusCode()
    {
        // Arrange
        var message = "Bad request";
        var statusCode = 400;
        var errorCode = "invalid_param";

        // Act
        var exception = new DifySDKException(message, statusCode, errorCode);

        // Assert
        exception.Message.Should().Be(message);
        exception.StatusCode.Should().Be(statusCode);
        exception.ErrorCode.Should().Be(errorCode);
    }

    [Fact]
    public void Constructor_WithRawResponse_ShouldPreserveRawResponse()
    {
        // Arrange
        var message = "API error";
        var statusCode = 500;
        var errorCode = "internal_error";
        var rawResponse = "{\"error\": \"Something went wrong\"}";

        // Act
        var exception = new DifySDKException(message, statusCode, errorCode, rawResponse);

        // Assert
        exception.RawResponse.Should().Be(rawResponse);
    }

    [Fact]
    public void Constructor_WithAllParameters_ShouldSetAll()
    {
        // Arrange
        var message = "API error";
        var statusCode = 400;
        var errorCode = "invalid_param";
        var rawResponse = "{\"code\": \"invalid_param\"}";
        var innerException = new HttpRequestException("Network error");

        // Act
        var exception = new DifySDKException(message, statusCode, errorCode, rawResponse, innerException);

        // Assert
        exception.Message.Should().Be(message);
        exception.StatusCode.Should().Be(statusCode);
        exception.ErrorCode.Should().Be(errorCode);
        exception.RawResponse.Should().Be(rawResponse);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void StatusCode_WhenNull_ShouldBeNull()
    {
        // Arrange & Act
        var exception = new DifySDKException("Error", null, null);

        // Assert
        exception.StatusCode.Should().BeNull();
    }

    [Fact]
    public void ErrorCode_WhenNull_ShouldBeNull()
    {
        // Arrange & Act
        var exception = new DifySDKException("Error", 400, null);

        // Assert
        exception.ErrorCode.Should().BeNull();
    }
}
