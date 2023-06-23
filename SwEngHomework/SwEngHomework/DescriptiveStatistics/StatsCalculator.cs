using System.Text.RegularExpressions;

namespace SwEngHomework.DescriptiveStatistics
{
    public class StatsCalculator : IStatsCalculator
    {
        public Stats Calculate(string semicolonDelimitedContributions)
        {
            var stats = new Stats { Average = 0, Median = 0, Range = 0 };

            //string[] contributionValues = semicolonDelimitedContributions.Split(";");
            var contributionList = new List<string>(semicolonDelimitedContributions.Split(';')).ToList();

            var listOfDoubles = new List<double>();

            foreach (string contribution in contributionList)
            {
                // TODO: Might be better to do a RegEx here to allow for more flexibility
                var newString = contribution.Replace("," , string.Empty).Replace("$", string.Empty);

                var isValueAValidDouble = Double.TryParse(newString, out double value);

                if(!isValueAValidDouble)
                {
                    break;
                }

                listOfDoubles.Add(value);
            }

            if(listOfDoubles.Count == 0 ) { return stats; }

            stats.Average = Math.Round(listOfDoubles.Average(),2);
            stats.Median = Math.Round(CalculateMedian(listOfDoubles), 2);
            stats.Range = Math.Round(listOfDoubles.Max() - listOfDoubles.Min(), 2);


            return stats;


            // TODO: your implementation
            //throw new NotImplementedException();
        }

        public double CalculateMedian(List<double> entries)
        {
            double median;

            var orderedList = entries.OrderBy(p => p).ToList();
            if (orderedList.Count % 2 == 0)
            {
                var firstValue = orderedList[(orderedList.Count / 2) - 1];
                var secondValue = orderedList[(orderedList.Count / 2)];
                median = (firstValue + secondValue) / 2;
            }
            else
            {
                median = orderedList[(orderedList.Count / 2)]; ;
            }
            return median;
        }
    }
}
