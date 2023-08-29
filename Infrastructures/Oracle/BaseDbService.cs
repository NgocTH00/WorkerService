namespace WorkerServiceTemplate.Infrastructures.Oracle
{
    public class BaseDbService
    {
        private ILogger _logger;
        private string _connectionString;

        public BaseDbService(ILogger logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public void LogInputSP(string storedProcedure, params (string Key, object Value)[] parameters)
        {
            string logContent = "stored_procedure=" + storedProcedure;
            if (parameters.Any())
            {
                string paramsContent = string.Join(",", parameters.Select(x => x.Key + "=" + (x.Value ?? "null")));
                logContent += "\r\n parameters: " + paramsContent;
            }
            _logger.LogInformation(logContent);
        }

        protected async Task<TResult> ExecuteSqlQueryAsync<TResult>(Func<OracleConnection, Task<TResult>> func)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                return await func(conn);
            }
        }

        protected async Task ExecuteSqlQueryAsync(Func<OracleConnection, Task> func)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                await func(conn);
            }
        }
    }
}
