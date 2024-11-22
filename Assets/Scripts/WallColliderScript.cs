using System;
using Unity.VisualScripting;
using UnityEngine;

public class WallColliderScript : MonoBehaviour
{
    [SerializeField] CameraScript cameraScript;

    
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
          
           //cameraScript.FocusOnWalls(ZoneDeVisionRef);
           cameraScript.FocusOnPlayer();

        }
    }
}
