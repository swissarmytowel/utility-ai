using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Actions.Pickers
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Action w Inputs", menuName = "UtilityAI/Action w Inputs")]
    public class PickerAction : AbstractUtilityAction
    {
        public AiContextVariable evaluatedParamName = AiContextVariable.None;

        public List<InputConsideration> considerations = new List<InputConsideration>();

        private float[] GetSumScores(AiContext context) {
            var vetoIndices = new List<int>();
            var count = (context.GetParameter(evaluatedParamName) as IEnumerable).Cast<object>().Count();
            if (count == 0) return null;
            var averageScores = new float[count];
            
            foreach (var inputConsideration in considerations) {
                if (!inputConsideration.isEnabled) continue;
                var scores = inputConsideration.Evaluate(context, count);
                for (var i = 0; i < scores.Count; i++) {
                    var score = Mathf.Round(scores[i] * 1e+3f) / 1e+3f;
                    if (score == 0 && inputConsideration.canApplyVeto) {
                        vetoIndices.Add(i);
                        continue;
                    }
                    averageScores[i] += score;
                }
            }

            foreach (var vetoIndex in vetoIndices) {
                averageScores[vetoIndex] = 0f;
            }

            return averageScores;
        }
        
        public override UtilityPick EvaluateAbsoluteUtility(AiContext context) {
            if (evaluatedParamName != AiContextVariable.None) {
                var averageScores = GetSumScores(context);
                if (averageScores != null) {
                    var considerationsCount = (float)considerations.Count;
                    var maxIdx = -1;
                    var maxAvg = 0f;
                    for (var i = 0; i < averageScores.Length; i++) {
                        var score = averageScores[i];
                        score /= considerationsCount;
                        if (score > maxAvg) {
                            maxAvg = score;
                            maxIdx = i;
                        }
                    }
                
                    return new UtilityPick(this, ActionWeight * maxAvg, maxIdx);
                }
            }

            return null;
        }

        public override void Execute(AiContext context, UtilityPick pick) {
            _lastInvokedTime = Time.time;
            _invokedTimes++;
            _inExecution = true;
            actionTask?.Invoke(context, pick);
        }
    }
}