using UnityEngine;

namespace UtilityAI_Base.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Exponential-style response curve
    /// Slope-intercept representation  y = m(x - c) ^ k + b  
    /// </summary>
    public sealed class QuadraticResponseCurve : ResponseCurve
    {
        #region public properties

        #endregion

        #region constructors

        public QuadraticResponseCurve() {
            SetDefaults();
            responseCurveType = CurveType.Quadratic;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) =>
            Mathf.Clamp(
                slope.Value * Mathf.Pow(Mathf.Clamp(parameter - horizontalShift.Value, 0f, 1f), 
                    exponent.Value) + verticalShift.Value, 0f, 1f);

        public override void SetDefaults() {
            slope = new CurveParameter(1f, -50f, 50f);
            verticalShift = new CurveParameter(0f,  -1f, 2f);
            horizontalShift = new CurveParameter(0f, -1f, 2f);
            exponent = new CurveParameter(2f, -1f, 10f);
        }

        #endregion
    }
}