using System;
using System.Collections.Generic;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.CustomAttributes;
using UtilityAI_Base.ExtensionMethods;
using Random = UnityEngine.Random;

namespace UtilityAI_Base.Selectors.ActionSelectors
{
    [Serializable]
    [ActionSelector("Dual Reasoner")]
    public sealed class DualUtilityReasoner : ActionSelector
    {
        public class UtilityWeights
        {
            public float Weight = 0f;
            public readonly float Rank = 0f;
            public readonly UtilityPick UAction = null;

            public UtilityWeights(float rank, UtilityPick action) {
                Rank = rank;
                UAction = action;
            }
        }

        public override UtilityPick Select(AiContext context, List<AbstractUtilityAction> actions) {
            var utilities = new List<UtilityWeights>();
            foreach (var action in actions) {
                UtilityPick utility = action.EvaluateAbsoluteUtility(context);
                if (utility != null && utility.Score > 0f) {
                    utilities.Add(new UtilityWeights(utility.Score, utility));
                }
            }

            var count = utilities.Count;
            if (count > 0) {
                utilities.Sort((first, second) => first.Rank.CompareTo(second.Rank));

                var std = utilities.GetStd();
                var sum = 0f;

                var minWeight = float.PositiveInfinity;
                var maxWeight = 0f;

                var selected = new List<UtilityWeights>();
                foreach (var u in utilities) {
                    if (u.Rank < std) continue;
                    selected.Add(u);
                    sum += u.Rank;
                }

                foreach (var u in selected) {
                    u.Weight = u.Rank / sum;
                    if (u.Weight > maxWeight) maxWeight = u.Weight;
                    if (u.Weight < minWeight) minWeight = u.Weight;
                }
                // TODO: Weights are equal to 1 on the first run ??  
                var rand = Random.Range(minWeight, maxWeight);
                return utilities.Find(u => u.Weight >= rand).UAction;
            }

            return null;
        }
    }
}