using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Contexts
{
    [Serializable]
    public abstract class AiContext : MonoBehaviour, IAiContext
    {
        [HideInInspector] public IAgent owner;
        [HideInInspector]  public GameObject target;
        [HideInInspector] public Vector3 StartingPoint;

        [SerializeField] protected int bufferSize = 50;
        protected Collider[] Colliders;
        
        public abstract object GetParameter(AiContextVariable param);

        public abstract void UpdateContext();
        
        #region REFLECTION
        
        public object this[string paramName]
        {
            get => GetType().GetProperties()
                .First(p => p.GetCustomAttribute(typeof(NpcContextVar)) != null && p.Name == paramName)
                .GetValue(this, null);
            set => GetType().GetProperties()
                .First(p => p.GetCustomAttribute(typeof(NpcContextVar)) != null && p.Name == paramName)
                .SetValue(this, value, null);
        }
        
        #endregion
    }
}