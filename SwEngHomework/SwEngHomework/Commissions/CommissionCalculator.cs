using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SwEngHomework.Commissions
{
    public class CommissionCalculator : ICommissionCalculator
    {
        public IDictionary<string, double> CalculateCommissionsByAdvisor(string jsonInput)
        { 
            var jsonObj = JObject.Parse(jsonInput);
            var advisors = ((JArray)jsonObj["advisors"]).ToObject<List<Advisor>>();
            var accounts = ((JArray)jsonObj["accounts"]).ToObject<List<Account>>();

            var output = new Dictionary<string, double>();

            var advisorAccounts = advisors.GroupJoin(accounts,
                adv => adv.Name,
                acct => acct.Advisor,
                (advisor, account) => new
                {
                    advisor.Name,
                    advisor.Level,
                    Accounts = account.Select(ad => ad).ToList(),
                }).ToList()
                .Select(p => new
                {
                    p.Name,
                    CommissionTotal = Math.Round(p.Accounts.Sum(x => CalculateAdvisorCommission(p.Level, x.PresentValue)),2)
                }).ToList();

            output = advisorAccounts.ToDictionary(x => x.Name, y => y.CommissionTotal);
            return output;

            // TODO: your implementation
            // throw new NotImplementedException();
        }

        public double CalculateAdvisorCommission(string? level, double presentValue)
        {
            switch (level)
            {
                case "Senior":
                    return GetBasisPointsMultiplier(presentValue) * 1;

                case "Experienced":
                    return GetBasisPointsMultiplier(presentValue) * .5;

                case "Junior":
                    return GetBasisPointsMultiplier(presentValue) * .25;

                default: return 0;
            }
        }

        public double GetBasisPointsMultiplier(double presentValue)
        {
            if (presentValue < 50000)
                return (presentValue * .0005);

            if (presentValue >= 50000 && presentValue < 100000)
                return (presentValue * .0006);

            if (presentValue >= 100000)
                return (presentValue * .0007);

            return 0;
        }
    }
    
    public class Advisor
    {
        public string? Name { get; set; }
        public string? Level { get; set; }
    }

    public class Account
    {
        public string? Advisor { get; set; }
        public double PresentValue { get; set; }
    }
}
