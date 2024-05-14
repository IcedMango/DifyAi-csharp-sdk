namespace DifyAi.Dto.Base;

public class DifyApiResult<T>
{
    public int? Code { get; set; }
    public bool? Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}