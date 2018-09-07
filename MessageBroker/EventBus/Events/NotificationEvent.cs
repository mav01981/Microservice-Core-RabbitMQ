namespace MicroServiceOne
{
    public class NotificationEvent: IntegrationEvent
    {
        public int RecordId { get; private set; }

        public string Message { get; private set; }

        public NotificationEvent(int recordId, string message)
        {
            RecordId = recordId;
            Message = message;
        }
    }
}
