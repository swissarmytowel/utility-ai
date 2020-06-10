using System;
using UtilityAI_Base.Considerations.Interfaces;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Considerations
{
    /// <summary>
    /// Consideration implementation
    /// All considerations should implement this class instead of an interface 
    /// </summary>
    [Serializable]
    public sealed class ContextConsideration : Consideration
    {
        public ContextConsideration(string description) => this.description = description;

        public ContextConsideration() {
        }

        public float Evaluate(AiContext context) {
            var score = 0f;
            if (evaluatedContextVariable != AiContextVariable.None) {
                var paramValue = (float) context.GetParameter(evaluatedContextVariable);
                score = EvaluateAt(paramValue);;
            }

            return score;
        }
    }
}