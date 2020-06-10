using System;
using UnityEngine;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Considerations.Interfaces
{
    [Serializable]
    public abstract class Consideration : IConsideration
    {
        [SerializeField] public string description = "n";
        [SerializeField] protected float weight = 1f;
        [SerializeField] protected Vector2 valueRange = new Vector2(0f, 100f);
        
        public bool canApplyVeto = false;
        public bool isEnabled = false;

        public AiContextVariable evaluatedContextVariable;

        public AnimationCurve utilityCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        public virtual float ApplyCurveAt(float point) {
            return Mathf.Clamp(utilityCurve.Evaluate(point), 0f, 1f);
        }
        
        public virtual float EvaluateAt(float value) {
            var rangedValue = (value - valueRange.x) / (valueRange.y - valueRange.x);
            return ApplyCurveAt(Mathf.Clamp(rangedValue, 0f, 1f));
        }
    }
}