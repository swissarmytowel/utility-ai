namespace UtilityAI_Base.Contexts.Interfaces
{
    /// <summary>
    /// Provides relevant context information for an in-game AI-driven object
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Fills in necessary context fields 
        /// </summary>
        /// <param name="context">current working AI context</param>
        void ProvideContext(IAiContext context);
    }
}