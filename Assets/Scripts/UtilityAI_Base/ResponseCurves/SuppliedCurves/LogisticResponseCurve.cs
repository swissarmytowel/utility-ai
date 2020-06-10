using UnityEngine;

namespace UtilityAI_Base.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Logistic sigmoid-style response curve with float datatype
    /// Slope-intercept representation  y = k * 1/(1 + e^-(mx+c)) + b   
    /// </summary>
    public sealed class LogisticResponseCurve : ResponseCurve
    {
        #region public properties

        #endregion

        #region constructors

        public LogisticResponseCurve() {
            SetDefaults();
            responseCurveType = CurveType.Logistic;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) {
            var denominator = 1f + Mathf.Exp(-slope.Value * (parameter - exponent.Value));
            return Mathf.Clamp(verticalShift.Value / denominator + horizontalShift.Value, 0f, 1f);
        }

        public override void SetDefaults() {
            slope = new CurveParameter(20f, 0f, 100f);
            exponent = new CurveParameter(.5f, 0f, 1f);
            verticalShift = new CurveParameter(1f, -1f, 2f);
            horizontalShift = new CurveParameter(0f, -1f, 1f);
        }

        #endregion
    }
}