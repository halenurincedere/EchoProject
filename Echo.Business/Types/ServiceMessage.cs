namespace Echo.Business.Shared
{
    // Basic service response wrapper for indicating success/failure and returning a message
    public class ServiceMessage
    {
        // Indicates whether the operation was successful
        public bool IsSucceed { get; set; }

        // Provides a descriptive message about the result
        public string Message { get; set; } = string.Empty;
    }

    // Generic version of ServiceMessage that includes a return data payload
    public class ServiceMessage<T> : ServiceMessage
    {
        // Holds the data returned by the operation
        public T Data { get; set; } = default!;
    }
}