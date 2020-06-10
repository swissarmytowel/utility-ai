namespace UtilityAI_Base.ResponseCurves.Interfaces
{
    /// <summary>
    /// Interface for a response curve used in calculating consideration utility score
    /// Utility is ALWAYS a float value between 0.0 and 1.0
    /// Response curve always take a float parameter which should be normalized to 0.0 to 1.0 beforehand
    /// </summary>
    public interface IResponseCurve
    {
        /// <summary>
        /// Evaluate curve at point parameter
        /// </summary>
        /// <param name="parameter">point at which curve should be evaluated</param>
        float EvaluateAt(float parameter);

        /// <summary>
        /// Curve function representation (E.g. y = mx + b)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        float CurveFunction(float parameter);
    }
}