using System;
using System.Collections.Generic;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Selectors.ActionSelectors
{
    /// <summary>
    /// Highest scored action wins and shall be executed
    /// </summary>
    [Serializable]
    [ActionSelector("Highest Score Wins")]
    public sealed class HighestScoreWins : ActionSelector
    {
        public override UtilityPick Select(AiContext context, List<AbstractUtilityAction> actions) {
            var maxUtility = 0f;
            UtilityPick highestScoreAction = null;
            for (var i = 0; i < actions.Count; i++) {
                if (actions[i] != null) {
                    var utility = actions[i].EvaluateAbsoluteUtility(context);
                    if (utility.Score > 0 && utility.Score >= maxUtility) {
                        maxUtility = utility.Score;
                        highestScoreAction = utility;
                    }
                }
            }

            return highestScoreAction;
        }
    }
}