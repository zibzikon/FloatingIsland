using UnityEngine;
using UnityEngine.AI;

namespace Units.Behaviours
{
    public class TerrestrialEntityMovingBehaviour : IMovable
    {
        private readonly float _minRequiredDistanceToTarget;
        
        private Vector3 _destinationPosition;
        
        public bool TargetWasReached { get; private set; }
        
        public bool IsMoving => _agent.velocity.sqrMagnitude != 0;

        private readonly NavMeshAgent _agent;
        
        private ITarget _settedTarget;

        private float GetDistanceBetweenTargetAndAgent()
        {
            return Vector3.Distance(_agent.transform.position, _destinationPosition);
        }

        public TerrestrialEntityMovingBehaviour(NavMeshAgent agent, float minRequiredDistanceToTarget)
        {
            _agent = agent;
            _minRequiredDistanceToTarget = minRequiredDistanceToTarget;
            
        }
        
        private Vector3 GetMovingPosition(ITarget targetTransform)
        {
            const int maxDistance = 20;
            NavMesh.SamplePosition(targetTransform.Transform.Position, out var hit,
                maxDistance, NavMesh.AllAreas);

            _destinationPosition = hit.position;
            return _destinationPosition;
        }
        
        
        public void SetTarget(ITarget target)
        {
            if (target.IsDestroyed) return;
            
            Reset();
            _settedTarget = target;
            SetDestination();
            _settedTarget.Destroyed += OnTargetDestroyed;
            _settedTarget.PositionChanged += OnTargetPositionChanged;
        }

        public void MoveTo(Vector3 position)
        {
            var transform = _agent.transform;
            transform.LookAt(new Vector3(position.x, transform.position.y ,position.z));
            _agent.SetDestination(position);
        }

        private void OnTargetPositionChanged()
        {
            SetDestination();
        }
        
        private void SetDestination()
        {
            MoveTo(GetMovingPosition(_settedTarget));
        }
        
        public void Reset()
        {
            _destinationPosition = Vector3.zero;
            TargetWasReached = false;
            if (_settedTarget != null)
            {
                _settedTarget.PositionChanged -= OnTargetPositionChanged;
                _settedTarget.Destroyed -= OnTargetDestroyed;
            }
            _settedTarget = null; 
        }

        private void OnTargetDestroyed(object sender)
        {
            Reset();
        }
        
        public void OnUpdate()
        {
            if (_settedTarget != null && GetDistanceBetweenTargetAndAgent() <= _minRequiredDistanceToTarget)
            {
                if (_settedTarget.IsDestroyed)
                {
                    Reset();
                    return;
                }
                
                _agent.ResetPath();
                _agent.velocity = Vector3.zero;
                TargetWasReached = true;
            }
        }
    }
}