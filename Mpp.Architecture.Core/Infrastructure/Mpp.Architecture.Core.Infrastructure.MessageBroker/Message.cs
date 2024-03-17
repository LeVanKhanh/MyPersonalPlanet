namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

public class Message
{
    public Message()
    {
        Id = Guid.NewGuid().ToString();
        CreationDate = DateTime.UtcNow;
        MessageToken = string.Empty;
    }

    public string Id { get; set; }

    public DateTime CreationDate { get; private set; }

    public bool HasError { get; set; }

    public string MessageToken { get; set; }
}
