using UnityEngine;
using UnityEngine.AI;

namespace Units.Behaviours
{
    public class TerrestrialEnemyMovingBehaviour : IMovable
    {
        private readonly float _minRequiredDistanceToTarget;
        
        private Vector3 _destinationPositoin;
        
        public bool TargetWasReached { get; private set; }
        
        public bool IsMoving => _agent.velocity.sqrMagnitude != 0;

        private readonly NavMeshAgent _agent;

        private bool _targetIsSetted;

        private float GetDistanceBetweenTargetAndAgent()
        {
            return Vector3.Distance(_agent.transform.position, _destinationPositoin);
        }

        public TerrestrialEnemyMovingBehaviour(NavMeshAgent agent, float minRequiredDistanceToTarget)
        {
            _agent = agent;
            _minRequiredDistanceToTarget = minRequiredDistanceToTarget;
            
        }
        
        private Vector3 GetMovingPosition(ITarget targetTransform)
        {
            const int maxDistance = 20;
            NavMesh.SamplePosition(targetTransform.Transform.position, out var hit,
                maxDistance, NavMesh.AllAreas);

            _destinationPositoin = hit.position;
            return _destinationPositoin;
        }
        
        
        public void MoveTo(ITarget target)
        {
            if (target.IsDestroyed) return;
            
            Reset();
            _agent.SetDestination(GetMovingPosition(target));
            _targetIsSetted = true;
        }

        private void Reset()
        {
            _destinationPositoin = Vector3.zero;
            TargetWasReached = false;
            _targetIsSetted = false;
        }

        public void OnUpdate()
        {
            if (_targetIsSetted && GetDistanceBetweenTargetAndAgent() <= _minRequiredDistanceToTarget)
            {
                _agent.ResetPath();
                _agent.velocity = Vector3.zero;
                TargetWasReached = true;
            }
        }
    }
}