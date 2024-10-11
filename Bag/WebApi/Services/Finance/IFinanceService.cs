using WebApi.Helpers.Finance;

namespace WebApi.Services.Finance;  

public interface IFinanceService
{
    Task<List<DailyPrice>> GetDailyPrices(string symbol, string fromDate, string toDate);

}
