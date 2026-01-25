namespace N5.Permissions.Domain.Interfaces;

public class OperationMessage
{
    public Guid Id { get; set; }
    public string NameOperation { get; set; } = string.Empty;
}


public interface IKafkaProducer
{
    Task SendAsync(OperationMessage message);
}