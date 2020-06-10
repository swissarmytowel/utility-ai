using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Intellect.Interfaces;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.Factories;

namespace UtilityAI_Base.Intellect
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(AiContext))]
    public class NpcIntellect : MonoBehaviour, IIntellect
    {
        #region Public members

        public ActionSelectorType actionSelectorType;
        public ActionSelectorType fallbackSelectorType;

        [SerializeField] public List<AbstractUtilityAction> actions = new List<AbstractUtilityAction>();
        [SerializeField] public List<AbstractUtilityAction> fallBackActions = new List<AbstractUtilityAction>();

        [HideInInspector] public ActionSelector fallbackSelector;
        [HideInInspector] public ActionSelector selector;
        
        #endregion

        #region Private members

        private AiContext _context;
        private NavMeshAgent _navMeshAgent;
        
        private UtilityPick _currentAction = null;
        private UtilityPick _lastAction = null;

        private float modifier = 0f;
        
        private float _inertiaTimer = 0f;
        private float _cooldownTimer = 0f;

        private float _lastUpdated = 0f;
        private float _updateTimesPerSecond = 1;
        private bool _consecutiveActionsAreSame = false;
        private bool _inertiaIsApplied = false;

        private bool _started = false;
        
        #endregion

        #region public fields

        public float UpdateTimesPerSecond
        {
            get => _updateTimesPerSecond;
            set => _updateTimesPerSecond = Mathf.Clamp(value, 0f, 100f);
        }

        #endregion

        private void Awake() {
            _lastUpdated = Time.time + 1f / _updateTimesPerSecond;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _context = GetComponent<AiContext>();
            if (selector == null) selector = ActionSelectorFactory.GetSelector(actionSelectorType);
            
        }

        private void Update() {
            if (Time.time >= _lastUpdated) {
                _lastUpdated = Time.time + 1f / _updateTimesPerSecond;
                // Update context
                Sense();
                // Think about next action to be taken
                Think();
                // Perform selected action
                Act();
            }
        }

        #region Intellect methods

        private void Sense() {
            _context.UpdateContext();
        }

        private void SetCurrentInCooldown() => modifier = 0f;
        
        private IEnumerator AddInertiaToCurrent() {
            if (!_started) {
                _started = true;
                modifier = 2f;
                yield return new WaitForSeconds(_currentAction.UtilityAction.InertiaTime);
                modifier = 1f;
                _started = false;
            }
        }

        private void Think() {
            Profiler.BeginSample("THINK!");

            UtilityPick action = selector.Select(_context, actions);
            if (_currentAction == null) {
                if (action != null) _currentAction = action;
            }
            else {
                _currentAction =  _currentAction.UtilityAction.EvaluateAbsoluteUtility(_context);
                _currentAction.Score *= modifier;
                if (_currentAction.Score == 0) {
                    _currentAction = null;
                }

                if (action == null) return;
                if (_currentAction == null) {
                    _currentAction = action;
                }
                else {
                    _consecutiveActionsAreSame = action == _currentAction;
                    if (!_consecutiveActionsAreSame) {
                        if (action.Score > _currentAction.Score) {
                            _currentAction = action;
                        }
                    } 
                }
            }

            _lastAction = _currentAction;
            Profiler.EndSample();
        }

        private void Act() {
            if (_currentAction != null) {
                _currentAction.UtilityAction.Execute(_context, _currentAction);
                StartCoroutine(AddInertiaToCurrent());
                // Debug.Log(name + ": " +_currentAction.UtilityAction.description + " " + _currentAction.Score);
            }
        }

        #endregion

        public bool IsActive() {
            return true;
        }
    }
}