using System.Collections.Generic;

namespace SwEngHomework.Commissions
{
    public interface ICommissionCalculator
    {
        IDictionary<string, double> CalculateCommissionsByAdvisor(string jsonInput);
    }
}
