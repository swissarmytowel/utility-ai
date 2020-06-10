using System;
using System.Collections.Generic;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Selectors.ConsiderationQualifiers
{
    [Serializable]
    public class AverageQualifier : ConsiderationsQualifier
    {
        public override float Qualify(AiContext context, List<ContextConsideration> considerations) {
            var averageScore = 0f;
            foreach (var consideration in considerations) {
                if (consideration.isEnabled) {
                    var score = consideration.Evaluate(context);
                    if (consideration.canApplyVeto && score == 0f) {
                        return 0f;
                    }

                    averageScore += score;
                }
            }
            return averageScore / considerations.Count;
        }
    }
}