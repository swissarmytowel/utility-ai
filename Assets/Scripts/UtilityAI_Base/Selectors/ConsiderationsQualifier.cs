using System;
using System.Collections.Generic;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Selectors
{
    public enum QualifierType
    {
        Product,
        Average
    }
    
    public class ConsiderationsQualifier
    {
        public virtual float Qualify(AiContext context, List<ContextConsideration> considerations) {
            throw new NotImplementedException();
        }
    }
}