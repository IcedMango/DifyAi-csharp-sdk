namespace DifyAi.Dto.Base;

public class DifyApiResult<T>
{
    public string Code { get; set; }
    public bool? Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}