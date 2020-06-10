using UnityEngine;
using UtilityAI_Base.Contexts;

namespace UtilityAI_Base.Sensors
{
    public sealed class RadialSensor : MonoBehaviour
    {
        #region public fields

        public float visibilityRadius;
        public int colliderBufferSize;

        public LayerMask visibleLayers;

        public AiContext context;

        #endregion

        #region private fields

        private Collider[] _detectedCollidersBuffer;
        private int _detectedCollidersCount;

        #endregion
        
        #region interface methods implementations
 
        #endregion
        
        #region Unity functions

        private void Awake() {
            colliderBufferSize = colliderBufferSize == 0 ? 10 : colliderBufferSize;
            _detectedCollidersBuffer = new Collider[colliderBufferSize];
        }

        private void Update() {
        }

        private void FixedUpdate() {
            _detectedCollidersCount = Physics.OverlapSphereNonAlloc(transform.position, visibilityRadius,
                _detectedCollidersBuffer, visibleLayers);
        }

        #endregion

        #region user defined context actions
        
        #endregion
    }
}