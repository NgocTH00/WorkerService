namespace WorkerServiceTemplate.Infrastructures.Kafka
{
    public class KafkaProduceService : IKafkaProduceService
    {
        private readonly IProducer<string, string>    _producer;
        private readonly ILogger<KafkaProduceService> _logger;
        private readonly int                          _queueBufferingMaxMessages;
        private int _counter = 0;


        public KafkaProduceService(
            ProducerConfig producerConfig,
            KafkaSettings kafka,
            ILogger<KafkaProduceService> logger)
        {
            _producer                  = new ProducerBuilder<string, string>(producerConfig).Build();
            _logger                    = logger;
            _queueBufferingMaxMessages = kafka.QueueBufferingMaxMessages;
        }

        /// <summary>
        /// Produce data lên kafka dưới dạng json 
        /// nếu data null thì không Produce
        /// nếu lỗi thì log ra lỗi, data và tên topic
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="topic">tên topic</param>
        /// <param name="key">key của message</param>
        public void Produce<TData>(TData data, string topic, string key)
        {
            if (data is null) return;
            var message = GetMessage(data, key);
            
            try
            {
                _producer.Produce(topic, message, ProducerFlushTrigger);
            }
            catch (Exception ex)
            {
                var errorMessage = $"ex={ex};\n\r data={message.Value}; topic ={topic}";
                _logger.LogError(errorMessage);
            }
        }

        private Message<string, string> GetMessage<TData>(TData data, string key)
        {
            var messageValue = JsonSerializer.Serialize(data, data.GetType());
            var message      = new Message<string, string> { Value = messageValue };
            message.Key      = key ?? message.Key;
            return message;
        }

        private void ProducerFlushTrigger(DeliveryResult<string, string> result)
        {
            if (result.Status == PersistenceStatus.NotPersisted)
                _producer.Flush();

            if (++_counter == _queueBufferingMaxMessages)
            {
                _counter = 0;
                _producer.Flush();
            }
        }
    }
}
