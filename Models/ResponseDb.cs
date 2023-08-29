namespace WorkerServiceTemplate.Models
{
    public class ResponseDb
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }

        public static ResponseDb FromOracleParameters(
            OracleDynamicParameters parameters, 
            string returnCodeParamName,
            string returnMessParamName)
        {
            var returnMess = parameters.Get<string>(returnMessParamName);
            var returnCode = parameters.Get<int>(returnCodeParamName);

            return new ResponseDb{ ReturnCode = returnCode, ReturnMessage = returnMess };
        }
    }
}
