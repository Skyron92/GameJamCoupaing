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
    [SerializeField] GameObject projectilePrefab; // Préfabriqué du projectile
    [SerializeField] Transform projectileSpawnPoint; // Point de spawn du projectile
    private int projectileSpeed = 3;
    public Animator animator;

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
            animator.SetBool("IsWalking", true);
            StartCoroutine(ChasePlayerCoroutine());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("OnExit");
            isChasing = false;
            animator.SetBool("IsWalking", false);
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
        // Vérifie que le prefab et le spawn point sont configurés
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
         
          
            // Calcule la direction vers le joueur
            Vector3 directionToPlayer = (_transformPlayer.position - projectileSpawnPoint.position).normalized;
            
            Vector3 spawnPosition = projectileSpawnPoint.position + directionToPlayer * 2f;
            
            // Instancie le projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            
            // Ajoute une force au projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {   print("ajoute force projectile");
                rb.useGravity = false;
                animator.SetTrigger("IsAttacking");
                rb.AddForce(directionToPlayer * projectileSpeed, ForceMode.Impulse);
               
            }

            // Détruire le projectile après un temps pour éviter les fuites de mémoire
            Destroy(projectile, 4f);
        }
        else
        {
            Debug.LogWarning("Projectile prefab or spawn point is not assigned!");
        }
    }

    private IEnumerator ChasePlayerCoroutine()
    {
        float timeSinceLastEvent = 0f;
        float eventInterval = UnityEngine.Random.Range(5f, 10f); 
        float attackPauseDuration = 3f; 

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
                eventInterval = UnityEngine.Random.Range(5f, 10f);
                isAttacking = true;
            }

            // Attends une seconde avant de recommencer (petites étapes)
            yield return new WaitForSeconds(1f);
        }
    }

   
}
