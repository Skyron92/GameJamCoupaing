using System;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform _transformPlayer;
    private int attackSpeed = 20;
    private int AttackDamaga = 50;
    
    private NavMeshAgent BossAgent;
    private BoxCollider BossCollider;


    private void Awake()
    {
        BossAgent = GetComponent<NavMeshAgent>();
        
        
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ChasePlayer();
    }

    public void ChasePlayer()
    {
        BossAgent.destination = _transformPlayer.position;
    }
}
