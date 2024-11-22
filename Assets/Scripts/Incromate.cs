using UnityEngine;
using UnityEngine.AI;

public class Incromate : MonoBehaviour
{
    NavMeshAgent _agent;

    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
    }
    
    
}
