using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace IcedMango.DifyAi.IntegrationTests.Logging;

/// <summary>
/// Logger provider that outputs to xUnit's ITestOutputHelper.
/// This ensures logs appear in Rider/VS test output window.
/// </summary>
public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _output;
    private readonly LogLevel _minLevel;

    public XunitLoggerProvider(ITestOutputHelper output, LogLevel minLevel = LogLevel.Debug)
    {
        _output = output;
        _minLevel = minLevel;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(_output, categoryName, _minLevel);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Logger implementation that writes to ITestOutputHelper
/// </summary>
public class XunitLogger : ILogger
{
    private readonly ITestOutputHelper _output;
    private readonly string _categoryName;
    private readonly LogLevel _minLevel;

    public XunitLogger(ITestOutputHelper output, string categoryName, LogLevel minLevel)
    {
        _output = output;
        _categoryName = categoryName;
        _minLevel = minLevel;
    }

    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        try
        {
            var levelShort = logLevel switch
            {
                LogLevel.Trace => "TRCE",
                LogLevel.Debug => "DBUG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARN",
                LogLevel.Error => "FAIL",
                LogLevel.Critical => "CRIT",
                _ => "????"
            };

            // Shorten category name for readability
            var shortCategory = ShortenCategoryName(_categoryName);
            var message = formatter(state, exception);
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");

            _output.WriteLine($"[{timestamp}] [{levelShort}] {shortCategory}: {message}");

            if (exception != null)
            {
                _output.WriteLine($"    Exception: {exception.Message}");
            }
        }
        catch (InvalidOperationException)
        {
            // Test may have already completed, ignore
        }
    }

    private static string ShortenCategoryName(string categoryName)
    {
        // Extract the last part of the category name for readability
        var lastDot = categoryName.LastIndexOf('.');
        if (lastDot > 0 && lastDot < categoryName.Length - 1)
        {
            return categoryName[(lastDot + 1)..];
        }
        return categoryName;
    }
}

/// <summary>
/// Extension methods for adding xUnit logging
/// </summary>
public static class XunitLoggerExtensions
{
    /// <summary>
    /// Add xUnit test output logging
    /// </summary>
    public static ILoggingBuilder AddXunit(
        this ILoggingBuilder builder,
        ITestOutputHelper output,
        LogLevel minLevel = LogLevel.Debug)
    {
        builder.AddProvider(new XunitLoggerProvider(output, minLevel));
        return builder;
    }
}

/// <summary>
/// Null scope implementation for ILogger.BeginScope
/// </summary>
internal sealed class NullScope : IDisposable
{
    public static NullScope Instance { get; } = new();
    private NullScope() { }
    public void Dispose() { }
}
