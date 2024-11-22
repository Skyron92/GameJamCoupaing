using UnityEngine;

public class BossColiderScript : MonoBehaviour


{ 
   
    [SerializeField] private CameraScript _cameraScript;
    
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
        _cameraScript.StartCouroutinedab(_cameraScript.TargetTranformBoss);
    }
}
