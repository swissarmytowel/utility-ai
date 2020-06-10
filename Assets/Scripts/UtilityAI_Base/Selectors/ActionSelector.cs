using System.Collections.Generic;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Selectors
{
    public abstract class ActionSelector
    {
        public abstract UtilityPick Select(AiContext context, List<AbstractUtilityAction> actions);
    }
}