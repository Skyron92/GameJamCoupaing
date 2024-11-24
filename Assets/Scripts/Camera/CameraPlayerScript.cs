using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class CameraScript : MonoBehaviour
{  
    private CinemachineFollow _follow;
    private CinemachineRotationComposer rotationComposer;
    
    private CinemachineCamera CameraRef; 
    [SerializeField] private Transform TargetTransformPlayer;
    public Transform TargetTranformBoss;
    
   
    // Variables internes
  

 

    
    // Positions spécifique pour le zoom joueur 
    private float VerticalArmLenghtPlayerFloat = 1f;
    private float CameraDistancePlayerFloat = 5f;

    

    
    
    //variables déplacements côtés 
    private float ScreenPosXFloat = -0.50f;

   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        CameraRef = GetComponent<CinemachineCamera>();
        
        _follow=GetComponent<CinemachineFollow>();
        rotationComposer= GetComponent<CinemachineRotationComposer>();
        FocusOnPlayer();
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }
    
    
    public void FocusOnPlayer()
    {
       
            CameraRef.Follow = TargetTransformPlayer ;
            CameraRef.LookAt = TargetTransformPlayer ;

            _follow.FollowOffset = new Vector3(0, 1, -8);
    }
    
    
    
    public void CameraMoveHorizontally(float direction) {

        
            rotationComposer.Composition.ScreenPosition.x = ScreenPosXFloat * direction;
     
        
    }

    public void ChangeFOVWhenRun()
    {
        CameraRef.Lens.FieldOfView = 90f;
    }
    
    
}
