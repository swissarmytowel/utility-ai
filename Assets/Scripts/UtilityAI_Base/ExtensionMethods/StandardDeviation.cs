using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Selectors.ActionSelectors;

namespace UtilityAI_Base.ExtensionMethods
{
    public static class StandardDeviation
    {
        public static float GetStd(this IEnumerable<DualUtilityReasoner.UtilityWeights> values)
        {
            float standardDeviation = 0;
            var enumerable = values.ToArray();
            var count = enumerable.Length;
            if (count > 1)
            {
                var avg = enumerable.Average(d => d.Rank);
                var sum = enumerable.Sum(d => (d.Rank - avg) * (d.Rank - avg));
                standardDeviation = Mathf.Sqrt(sum / count);
            }
            return standardDeviation;
        }
    }
}