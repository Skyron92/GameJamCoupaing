using UnityEngine;

public class BossColiderScript : MonoBehaviour


{ 
   
    [SerializeField] private BrainCamera _brainCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _brainCamera.FocusOnBoss();
        }
    }
}