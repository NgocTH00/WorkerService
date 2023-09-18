namespace SingleService.Infrastructures.Oracle
{
    public interface IDemoDbService
    {
        Task<ResponseDb> UpdateDemoAsync(DateTime? implementationDate, string region, string reportId);
    }
}