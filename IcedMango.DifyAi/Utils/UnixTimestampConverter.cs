using Newtonsoft.Json;

namespace DifyAi.Utils;

public class UnixTimestampConverter : DateTimeConverterBase
{
    private static readonly DateTime Epoch = new (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteRawValue(UnixTimestampFromDateTime((DateTime)value).ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return reader.Value == null
            ? null
            : TimeFromUnixTimestamp((long)reader.Value);
    }

    private static DateTime TimeFromUnixTimestamp(long unixTimestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimestamp.ToString())).UtcDateTime;
    }

    public static long UnixTimestampFromDateTime(DateTime date)
    {
        var unixTimestamp = date.Ticks - Epoch.Ticks;
        unixTimestamp /= TimeSpan.TicksPerSecond;
        return unixTimestamp;
    }
}