using UnityEngine;
using UnityEngine.AI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;
    
}