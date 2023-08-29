namespace WorkerServiceTemplate.Infrastructures.Kafka
{
    public interface IKafkaProduceService
    {
        void Produce<TData>(TData data, string topic, string key);
    }
}