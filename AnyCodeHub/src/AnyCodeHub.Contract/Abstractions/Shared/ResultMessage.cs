namespace AnyCodeHub.Contract.Abstractions.Shared;
public class ResultMessage<T>
{
    public bool IsError { get; set; }
    public string Message { get; set; }
    public string MessageDetail { get; set; }
    public T Data { get; set; }
}
