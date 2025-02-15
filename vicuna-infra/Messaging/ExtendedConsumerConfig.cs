using Confluent.Kafka;

namespace vicuna_infra.Messaging
{
    /// <summary>
    ///     Extended consumer configuration options that include the group ID, topic, and bootstrap servers.
    /// </summary>
    public class ExtendedConsumerConfig : ConsumerConfig
    {
        /// <summary>
        ///     The Kafka topic to consume messages from.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        ///     A comma-separated list of Kafka broker addresses.
        /// </summary>
        public new string BootstrapServers { get; set; }
    }
}