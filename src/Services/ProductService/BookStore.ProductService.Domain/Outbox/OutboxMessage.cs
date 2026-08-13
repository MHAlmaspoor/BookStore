namespace BookStore.ProductService.Domain.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public DateTime OccuredOnUtc { get; private set; }
    public string Type { get; private set; }=string.Empty;
    public string Content { get; private set; }=string.Empty;
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set;}
    public DateTime? LastAttemptedOnUtc { get; private set; }
    public DateTime? NextAttemptOnUtc { get; private set; }    public bool IsPermanentlyFailed { get; private set; }

    public OutboxMessage()
    {

    }

    public OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
    {
        Id=id;
        OccuredOnUtc=occurredOnUtc;
        Type=type;
        Content=content;
    }

    public void MarkAsProcessed()
    {
        ProcessedOnUtc=DateTime.UtcNow;
        Error=null;
        NextAttemptOnUtc = null;
    }

    public void MarkAsRetry(string error, DateTime nextAttemptOnUtc)
    {
        RetryCount++;
        Error = error;
        LastAttemptedOnUtc = DateTime.UtcNow;
        NextAttemptOnUtc = nextAttemptOnUtc;
    }



    public void MarkAsFailed(string error)
    {
        Error = error;
        LastAttemptedOnUtc = DateTime.UtcNow;
        IsPermanentlyFailed = true;

    }
}
