using System;
using System.Collections.Generic;
using UtilityAI_Base.Considerations.Interfaces;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Considerations
{
    [Serializable]
    public class InputConsideration : Consideration
    {
        public List<float> Evaluate(AiContext context, int len = 1) {
            var pick = new List<float>();
            if (evaluatedContextVariable != AiContextVariable.None) {
                if (context.GetParameter(evaluatedContextVariable) is List<float> evaluatedParams) {
                    for (var i = 0; i < evaluatedParams.Count; i++) {
                        pick.Add(EvaluateAt( evaluatedParams[i]));
                    }
                }
                else {
                    if (context.GetParameter(evaluatedContextVariable) is float evaluatedParam) {
                        pick = new List<float>();
                        var score = EvaluateAt(evaluatedParam);
                        for (var i = 0; i < len; i++) {
                            pick.Add(score);
                        }
                    }
                }
            }
            return pick;
        }
    }
}