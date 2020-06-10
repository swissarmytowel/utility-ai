using System.Collections.Generic;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Contexts;
using Random = System.Random;

namespace UtilityAI_Base.Selectors.ActionSelectors
{
    public sealed  class RandomActionSelector : ActionSelector
    {
        private readonly Random _engine = new Random(42);
        public override UtilityPick Select(AiContext context, List<AbstractUtilityAction> actions) {
            var action = actions[_engine.Next(0, actions.Count)];
            return new UtilityPick(action, 1f);
        }
    }
}