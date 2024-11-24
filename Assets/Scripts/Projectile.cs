using UnityEngine;

public class Projectile : Trap
{
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
        }
    }
}
