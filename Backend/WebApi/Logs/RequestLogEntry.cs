namespace WebApi.Logs
{
    public class RequestLogEntry
    {
        public int LogId { get; set; }
        public string? RequestType { get; set; }
        public string? RequestURL { get; set; }
        public string? RequestDetails { get; set; }
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
