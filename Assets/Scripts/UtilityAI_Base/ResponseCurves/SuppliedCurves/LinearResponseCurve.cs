using System;
using UnityEngine;

namespace UtilityAI_Base.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Linear-style response curve with float datatype
    /// Slope-intercept representation  y = m(x - c) + b  
    /// </summary>
    /// 
    [Serializable]
    public sealed class LinearResponseCurve : ResponseCurve
    {
        #region constructors

        /// <summary>
        /// Default preset constructor
        /// </summary>
        public LinearResponseCurve() {
            SetDefaults();
            responseCurveType = CurveType.Linear;
        }

        public LinearResponseCurve(float slope, float exponent, float verticalShift, float horizontalShift) {
            this.slope = new CurveParameter(slope);
            this.exponent = new CurveParameter(exponent);
            this.verticalShift = new CurveParameter(verticalShift);
            this.horizontalShift = new CurveParameter(horizontalShift);
            responseCurveType = CurveType.Linear;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) =>
            Mathf.Clamp(slope.Value * (parameter - horizontalShift.Value) + verticalShift.Value, 0f, 1f);

        public override void SetDefaults() {
            slope = new CurveParameter(1f, -50f, 50f);
            verticalShift = new CurveParameter(1f,  0f, 2f);
            horizontalShift = new CurveParameter(1f, 0f, 2f);
            exponent = new CurveParameter(1f);
        }

        #endregion
    }
}