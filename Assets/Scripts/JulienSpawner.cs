using UnityEngine;

public class JulienSpawner : MonoBehaviour
{
    public GameObject IncromateSpawner;
    public float SpawnDelay = 5f;
    public float SpawnDistance = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(Incromate), 0f, SpawnDelay);
    }

    private void Incromate()
    {
        if (IncromateSpawner != null)
        { 
            Vector3 spawnPosition = transform.position + transform.forward * SpawnDistance;
            Instantiate(IncromateSpawner, spawnPosition, transform.rotation);
        }
        else
        {
            Debug.LogWarning("No Incromate spawner found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
