using System;
using UtilityAI_Base.Selectors.ActionSelectors;

namespace UtilityAI_Base.Selectors.Factories
{
    public enum ActionSelectorType
    {
        Random,
        DualUtility,
        HighestScoreWins
    }

    public static class ActionSelectorFactory
    {
        private static readonly RandomActionSelector RandomActionSelector = new RandomActionSelector();
        private static readonly DualUtilityReasoner DualUtilityReasoner = new DualUtilityReasoner();
        private static readonly HighestScoreWins HighestScoreWins = new HighestScoreWins();

        public static ActionSelector GetSelector(ActionSelectorType actionSelectorType) {
            switch (actionSelectorType) {
                case ActionSelectorType.Random:
                    return RandomActionSelector;
                case ActionSelectorType.DualUtility:
                    return DualUtilityReasoner;
                case ActionSelectorType.HighestScoreWins:
                    return HighestScoreWins;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionSelectorType), actionSelectorType, null);
            }
        }
    }
}