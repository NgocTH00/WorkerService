namespace SingleService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {

            StoredProcedures.InitStoredProcedureSetting(configuration);
            services.AddHostedService<ToDoSomethingBackgroundTask>()
                .AddLoggerConfiguration(configuration)
                .AddSingletonServices(configuration)
                .AddConfigureOptions(configuration)
                .AddKafkaConfiguration();
            return services;
        }

        /// <summary>
        /// đăng ký IKafkaProduceService, KafkaProduceService với DI 
        /// và cấu hình ProducerConfig
        /// </summary>
        /// <returns></returns>
        private static IServiceCollection AddKafkaConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaProduceService, KafkaProduceService>(sp => {
                var option = sp.GetRequiredService<IOptions<KafkaSettings>>();
                var logger = sp.GetRequiredService<ILogger<KafkaProduceService>>();
                ProducerConfig producerConfig = new ProducerConfig
                {
                    BootstrapServers          = option.Value.BootstrapServers,
                    Acks                      = option.Value.Acks,
                    RetryBackoffMs            = option.Value.RetryBackoffMs,
                    LingerMs                  = option.Value.LingerMs,
                    MaxInFlight               = option.Value.MaxInFlight,
                    BatchNumMessages          = option.Value.BatchNumMessages,
                    MessageSendMaxRetries     = option.Value.MessageSendMaxRetries,
                    MessageTimeoutMs          = option.Value.MessageTimeoutMs,
                    QueueBufferingMaxMessages = option.Value.QueueBufferingMaxMessages,
                    EnableIdempotence         = option.Value.EnableIdempotence
                };
                return new KafkaProduceService(producerConfig, option.Value, logger);
            });
            return services;
        }

        /// <summary>
        /// ngocth
        /// <para><b>Cấu hình logger và kafka logger</b></para>
        /// </summary>
        /// <param name="configuration"></param>
        private static IServiceCollection AddLoggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(options =>
            {
                var allowConsoleLog = configuration.GetValue(ServiceSettings.AllowConsoleLog, false);
                if (!allowConsoleLog)
                    options.ClearProviders();
                options.AddKafka(configuration.GetSection(ServiceSettings.KafkaLogger));
            });
            return services;
        }

        /// <summary>
        /// đọc các giá trị StoredProcedure, được định nghĩa trong file json
        /// </summary>
        /// <returns></returns>
        private static IServiceCollection AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StoredProcedures>(configuration.GetSection(nameof(StoredProcedures)));
            services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));
            return services;
        }

        /// <summary>
        /// Đăng kí service với lifetime Singleton
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection AddSingletonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDemoDbService, DemoDbService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DemoDbService>>();
                var connectionString = configuration.GetConnectionString(ServiceSettings.Database);
                return new DemoDbService(logger, connectionString);
            });
            return services;
        }
    }
}