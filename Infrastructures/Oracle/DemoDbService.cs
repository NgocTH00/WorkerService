namespace SingleService.Infrastructures.Oracle
{
    public class DemoDbService : BaseDbService, IDemoDbService
    {
        public DemoDbService(ILogger<DemoDbService> logger, string connectionString) : base(logger, connectionString) { }

        public async Task<ResponseDb> UpdateDemoAsync(DateTime? implementationDate, string region, string reportId)
        {

            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add(name: "pImplementationDate", dbType: OracleMappingType.Date, direction: ParameterDirection.Input, value: implementationDate ?? null);
            parameters.Add(name: "pRegion", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input, value: reportId, size: 200);
            parameters.Add(name: "pReportId", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input, value: reportId, size: 200);
            parameters.Add(name: "RS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.Add(name: "PMESSAGE", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, value: null, size: 200);
            parameters.Add(name: "PERRORCODE", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            var restut = await ExecuteSqlQueryAsync(conn => conn.ExecuteAsync(StoredProcedures.DemoUpdate, parameters, commandType: CommandType.StoredProcedure));
            var response = ResponseDb.FromOracleParameters(parameters, "PERRORCODE", "PMESSAGE");
            return response;
        }

        ////lấy dữ liệu tài khoản ngân hàng cuối ngày từ csdl thông qua sp
        //public async Task<IEnumerable<AccBalanceDetails>> GetAccountBalanceAsync(string spName, DateTime? implementationDate, string region, string reportId)
        //{
        //    OracleDynamicParameters parameters = new OracleDynamicParameters();
        //    parameters.Add(name: StoredProcedure.ImplementationDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input, value: implementationDate ?? null);
        //    parameters.Add(name: StoredProcedure.Region, dbType: OracleMappingType.Varchar2, size: 200, direction: ParameterDirection.Input, value: reportId);
        //    parameters.Add(name: StoredProcedure.ReportId, dbType: OracleMappingType.Varchar2, size: 200, direction: ParameterDirection.Input, value: reportId);
        //    parameters.Add(name: StoredProcedure.RefCursor, dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        //    var restut = await ExecuteSqlQueryAsync(conn => conn.QueryAsync<AccBalanceDetails>(spName, parameters, commandType: CommandType.StoredProcedure));
        //    return restut;
        //}
    }
}
