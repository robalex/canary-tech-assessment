namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    internal class DaysPerMonthGenerator
    {
        internal static Dictionary<int, int> GetDaysPerMonth(DateTime start, DateTime end)
        {
            var daysPerMonth = new Dictionary<int, int>();
            for (var date = start; date < end; date = date.AddDays(1)) {
                var month = date.Month;
                if (!daysPerMonth.ContainsKey(month)) {
                    daysPerMonth[month] = 0;
                }
                daysPerMonth[month]++;
            }
            return daysPerMonth;
        }
    }
}
