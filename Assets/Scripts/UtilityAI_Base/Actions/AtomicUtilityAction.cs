using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.ConsiderationQualifiers;
using UtilityAI_Base.Selectors.Factories;

namespace UtilityAI_Base.Actions
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Action", menuName = "UtilityAI/Empty Action")]
    public class AtomicUtilityAction : AbstractUtilityAction
    {
        /// <summary>
        /// List of all considerations needed to evaluate this actions' utility score 
        /// </summary>
        public List<ContextConsideration> considerations = new List<ContextConsideration>();

        [SerializeField] public ConsiderationsQualifier qualifier = new ProductQualifier();
        
        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            qualifier = ConsiderationsQualifierFactory.GetQualifier(qualifierType);
        }

        public override UtilityPick EvaluateAbsoluteUtility(AiContext context) {
            var score = Mathf.Round(_actionWeight * qualifier.Qualify(context, considerations) * 1e+3f) / 1e+3f;
            return new UtilityPick(this, score);
        }

        public override void Execute(AiContext context, UtilityPick pick) {
            _lastInvokedTime = Time.time;
            _invokedTimes++;
            _inExecution = true;
            actionTask?.Invoke(context, pick);
        }
    }
}