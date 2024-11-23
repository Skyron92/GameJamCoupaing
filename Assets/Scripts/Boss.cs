using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = Unity.Mathematics.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform _transformPlayer;
    private int attackSpeed = 20;
    private int AttackDamaga = 50;
    
    private NavMeshAgent BossAgent;
    private BoxCollider BossCollider;
    private bool isChasing = false;
    private bool isAttacking = false;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("onEnter  ");
            isChasing = true;
            StartCoroutine(ChasePlayerCoroutine());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("OnExit");
            isChasing = false;
            BossAgent.destination = transform.position; 
            StopCoroutine(ChasePlayerCoroutine()); 
        }
    }
    
    public void ChasePlayer()
    {
       
            // Définit la destination comme étant la position du joueur
            BossAgent.destination = _transformPlayer.position;
        
    }

  
    private void ThrowProjectile()
    {
        print("LANCE PROJECTILE ");
    }

    private IEnumerator ChasePlayerCoroutine()
    {
        float timeSinceLastEvent = 0f;
        float eventInterval = UnityEngine.Random.Range(3f, 8f); 
        float attackPauseDuration = 2f; 

        while (isChasing)
        {
            if (!isAttacking)
            {
                ChasePlayer(); 
            }
            else
            {
                BossAgent.destination = transform.position; 
                ThrowProjectile(); 
                isAttacking = false;

                // Pause supplémentaire après l'attaque
                yield return new WaitForSeconds(attackPauseDuration);
            }

            // Incrémenter progressivement le temps écoulé
            timeSinceLastEvent += 1f; // Simule une seconde écoulée

            // Si assez de temps s'est écoulé, déclenche l'événement
            if (timeSinceLastEvent >= eventInterval)
            {
                // Réinitialise le temps écoulé et génère un nouvel intervalle
                timeSinceLastEvent = 0f;
                eventInterval = UnityEngine.Random.Range(3f, 8f);
                isAttacking = true;
            }

            // Attends une seconde avant de recommencer (petites étapes)
            yield return new WaitForSeconds(1f);
        }
    }

   
}
