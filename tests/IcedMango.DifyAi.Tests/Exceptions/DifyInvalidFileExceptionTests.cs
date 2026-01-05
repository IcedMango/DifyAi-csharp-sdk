namespace IcedMango.DifyAi.Tests.Exceptions;

/// <summary>
/// Unit tests for DifyInvalidFileException
/// </summary>
public class DifyInvalidFileExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Invalid file";

        // Act
        var exception = new DifyInvalidFileException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.FilePath.Should().BeNull();
        exception.SupportedFormats.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithFilePath_ShouldSetFilePath()
    {
        // Arrange
        var message = "File not found";
        var filePath = "/path/to/file.txt";

        // Act
        var exception = new DifyInvalidFileException(message, filePath);

        // Assert
        exception.FilePath.Should().Be(filePath);
    }

    [Fact]
    public void Constructor_WithSupportedFormats_ShouldSetSupportedFormats()
    {
        // Arrange
        var message = "Unsupported file format";
        var filePath = "/path/to/file.exe";
        var supportedFormats = new[] { "mp3", "wav", "mp4", "webm" };

        // Act
        var exception = new DifyInvalidFileException(message, filePath, supportedFormats);

        // Assert
        exception.SupportedFormats.Should().BeEquivalentTo(supportedFormats);
    }

    [Fact]
    public void Message_ShouldBePreserved()
    {
        // Arrange
        var message = "The file format is not supported for audio transcription";
        var filePath = "/path/to/audio.ogg";
        var supportedFormats = new[] { "mp3", "wav", "m4a" };

        // Act
        var exception = new DifyInvalidFileException(message, filePath, supportedFormats);

        // Assert
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void SupportedFormats_WhenNull_ShouldBeNull()
    {
        // Arrange & Act
        var exception = new DifyInvalidFileException("Error", "/path");

        // Assert
        exception.SupportedFormats.Should().BeNull();
    }

    [Fact]
    public void SupportedFormats_ShouldContainAllFormats()
    {
        // Arrange
        var supportedFormats = new[] { "mp3", "mp4", "mpeg", "mpga", "m4a", "wav", "webm" };

        // Act
        var exception = new DifyInvalidFileException("Error", "/path", supportedFormats);

        // Assert
        exception.SupportedFormats.Should().HaveCount(7);
        exception.SupportedFormats.Should().Contain("mp3");
        exception.SupportedFormats.Should().Contain("wav");
    }

    [Fact]
    public void InheritsFromDifySDKException()
    {
        // Arrange & Act
        var exception = new DifyInvalidFileException("Error");

        // Assert
        exception.Should().BeAssignableTo<DifySDKException>();
    }

    [Fact]
    public void FilePath_CanBeAbsolutePath()
    {
        // Arrange
        var absolutePath = "/Users/test/Documents/audio.mp3";

        // Act
        var exception = new DifyInvalidFileException("Error", absolutePath);

        // Assert
        exception.FilePath.Should().Be(absolutePath);
    }

    [Fact]
    public void FilePath_CanBeRelativePath()
    {
        // Arrange
        var relativePath = "data/files/audio.mp3";

        // Act
        var exception = new DifyInvalidFileException("Error", relativePath);

        // Assert
        exception.FilePath.Should().Be(relativePath);
    }
}
