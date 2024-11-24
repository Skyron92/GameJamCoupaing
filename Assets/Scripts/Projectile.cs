using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Projectile : Trap
{
    [SerializeField] private GameObject FloorVFX;
    [SerializeField] private GameObject HitVFX;
    private float delayVFX = 3f;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Hitable")) {
            
            var target = other.gameObject.GetComponent<IHitable>();
            Attack(target, damage);
            Destroy(GetComponent<Rigidbody>());
            HitVFX.SetActive(true);
            print("active hit vfx");
            DestroyAfterVFX(HitVFX);
            print("Rentre collision joueur");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            DestroyWhenDontHit(delayVFX);
        }
    }
    public void DestroyWhenDontHit(float delay)
    {
        StartCoroutine(DestroySequenceWithDelay(delay));
    }

    private IEnumerator DestroySequenceWithDelay(float delay)
    {
        // Attendre le délai spécifié avant de lancer les VFX
        yield return new WaitForSeconds(delay);

        // Active les particules et commence le scaling
        FloorVFX.SetActive(true);
        transform.DOScale(0f, 1f).SetEase(Ease.Linear);

        // Lance la destruction après la fin des VFX
        StartCoroutine(DestroyAfterVFX(FloorVFX));
    }

    private IEnumerator DestroyAfterVFX(GameObject vfx)
    {
        // Vérifie si un système de particules est attaché
        ParticleSystem particleSystem = vfx.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            // Attend la durée des particules
            yield return new WaitForSeconds(particleSystem.main.duration);
        }

        // Détruit le GameObject du VFX après sa fin
        Destroy(gameObject);
        print("destroyed vfx");
    }
}
