using System;
using System.Collections.Generic;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Selectors.ConsiderationQualifiers
{
    [Serializable]
    public class ProductQualifier : ConsiderationsQualifier
    {
        public override float Qualify(AiContext context, List<ContextConsideration> considerations) {
            var product = 1f;
            foreach (var consideration in considerations) {
                if (consideration.isEnabled) product *= consideration.Evaluate(context);
            }
            var modificationFactor = 1f - 1f / considerations.Count;
            var makeUpValue = (1f - product) * modificationFactor;
            return product + makeUpValue * product;
        }
    }
}