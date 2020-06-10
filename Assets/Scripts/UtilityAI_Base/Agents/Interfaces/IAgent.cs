using UnityEngine;

namespace UtilityAI_Base.Agents.Interfaces
{
    /// <summary>
    /// Interface for Character Agents visible by AI
    /// </summary>
    public interface IAgent
    {
        /// <summary>
        /// Check if agent is active
        /// </summary>
        /// <returns>true Tf Agent active, false otherwise</returns>
        bool IsActive();

        /// <summary>
        /// Get current position in a world
        /// </summary>
        /// <returns>current agent position</returns>
        Vector3 GetCurrentWorldPosition();

        /// <summary>
        /// Get agents current velocity
        /// </summary>
        /// <returns>Agent velocity</returns>
        Vector3 GetCurrentVelocity();
    }
}