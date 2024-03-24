namespace Eric.Jansen.Application.Models;

public class QueueMessage<T> where T : class?
{
    public QueueMessage(T? message)
    {
        Message = message;
        Date = DateTime.UtcNow;
        MessageType = message?.GetType()?.FullName;
    }


    public DateTime Date { get; init; }

    public string? CultureName { get; set; }

    public string? IpAddress { get; set; }

    public T? Message { get; set; }

    public string? MessageType { get; set; }

    public string? UserAgent { get; set; }

}
