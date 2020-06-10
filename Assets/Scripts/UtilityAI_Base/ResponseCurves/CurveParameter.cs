using UnityEngine;

namespace UtilityAI_Base.ResponseCurves
{
    public class CurveParameter
    {
        private float _value;

        public float Value
        {
            get => _value;
            set => _value = Mathf.Clamp(value, MinValue, MaxValue);
        }

        public float MaxValue { get; set; } = 1f;
        public float MinValue { get; set; } = 0f;

        public CurveParameter(float value = 0f, float minValue = 0f, float maxValue = 1f) {
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
        }
    }
}