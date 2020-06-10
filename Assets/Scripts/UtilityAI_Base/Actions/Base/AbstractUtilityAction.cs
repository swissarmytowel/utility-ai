using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Selectors;

namespace UtilityAI_Base.Actions.Base
{
    [Serializable]
    public class ActionTask : UnityEvent<AiContext, UtilityPick>
    {
    }
    
    public sealed class Pick<T>
    {
        public float Utility { get; set; }
        public T Target { get; set; }

        public Pick(float utility, T target) {
            Utility = utility;
            Target = target;
        }
    }
    
    /// <summary>
    /// Action base class
    /// All actions should inherit this
    /// </summary>
    public abstract class AbstractUtilityAction : ScriptableObject
    {
        public ActionTask actionTask;
        /// <summary>
        /// How much time should pass before action can be invoked again 
        /// </summary>
        protected float _cooldownTime = 0f;
        protected float _inertiaTime = 0f;
        
        protected float _lastInvokedTime = 0f;

        public float CooldownTime
        {
            get => _cooldownTime;
            set => _cooldownTime = Mathf.Clamp(value, 0f, 1e+10f);
        }

        public float InertiaTime
        {
            get => _inertiaTime;
            set => _inertiaTime = Mathf.Clamp(value, 0f, 100f);
        }
        
        /// <summary>
        /// Action weight to be applied after Utility calculation. Adjusting weights make this action
        /// Less/more probable to be executed 
        /// </summary>
        protected float _actionWeight = 1f;
        public float _originalActionWeight = 1f;

        public float ActionWeight
        {
            get => _actionWeight;
            set
            {
                _actionWeight = Mathf.Clamp(value, 0f, 100f);
            }
        }

        /// <summary>
        /// Action description
        /// </summary>
        public string description = "New Action Description";

        /// <summary>
        /// Max number of times current action can be invoked in a row
        /// </summary>
        public int maxConsecutiveInvocations = 0;

        protected int _invokedTimes = 0;

        /// <summary>
        /// Action is being executed 
        /// </summary>
        protected bool _inExecution = false;

        /// <summary>
        /// Qualifier to calculate utility of ALL considerations for this action
        /// </summary>
        [FormerlySerializedAs("qualifierTypeType")]
        public QualifierType qualifierType;


        /// <summary>
        /// Callback for inertia coroutine
        /// Used to add weight to action, so it wont be disrupted unless found action with much higher priority
        /// </summary>
        /// <returns> WaitForSeconds coroutine object </returns>
        public IEnumerator AddInertia() {
            _actionWeight *= 4f;
            yield return new WaitForSeconds(_inertiaTime);
            _actionWeight = _originalActionWeight;
        }

        /// <summary>
        /// Callback for cooldown coroutine
        /// Used to add "trace" to action, so it wont be called consecutively 
        /// </summary>
        /// <returns> WaitForSeconds coroutine object </returns>
        public IEnumerator SetInCooldown() {
            var oldWeight = _actionWeight;
            _actionWeight = 0f;
            yield return new WaitForSeconds(_cooldownTime);
            _actionWeight = oldWeight;
        }

        /// <summary>
        ///  Evaluate absolute (raw) utility score of performing action from Considerations utilities
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        /// <returns>Absolute (raw) utility score of performing this action</returns>
        public abstract UtilityPick EvaluateAbsoluteUtility(AiContext context);

        /// <summary>
        /// Execute current action in current context
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        /// <param name="pick"> utility pick</param>
        public abstract void Execute(AiContext context, UtilityPick pick);

        public virtual bool IsInExecution() => _inExecution;

        public virtual bool CanBeInvoked() => !_inExecution && CooldownTime >= _lastInvokedTime &&
                                              _invokedTimes <= maxConsecutiveInvocations;
    }
}