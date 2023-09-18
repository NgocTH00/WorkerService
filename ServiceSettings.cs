namespace SingleService
{
    public class ServiceSettings
    {
        public const string Database        = "Demo";
        public const string KafkaLogger     = "KafkaLogger";
        public const string AllowConsoleLog = "AllowConsoleLog";
    }

    public class KafkaSettings
    {
        public string BootstrapServers          { get; set; }
        public Acks   Acks                      { get; set; }
        public int    RetryBackoffMs            { get; set; }
        public int    LingerMs                  { get; set; }
        public int    MaxInFlight               { get; set; }
        public int    BatchNumMessages          { get; set; }
        public int    MessageSendMaxRetries     { get; set; }
        public int    MessageTimeoutMs          { get; set; }
        public int    QueueBufferingMaxMessages { get; set; }
        public bool   EnableIdempotence         { get; set; }
        public Topic  Topics                    { get; set; }

        public class Topic
        {
            public string TopicDemo { get; set; }
        }
    }

    public class StoredProcedures
    {
        public static string DemoUpdate { get; private set; }

        public static void InitStoredProcedureSetting(IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(StoredProcedures));

            DemoUpdate = section[nameof(DemoUpdate)];
        }
    }
}