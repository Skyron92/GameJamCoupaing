using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class CameraBossScript : MonoBehaviour
{  
    private CinemachineFollow _follow;
    private CinemachineRotationComposer rotationComposer;
    private CinemachineCamera CameraRef; 
    
    public Transform TargetTranformBoss;
    public Transform TargetTransformPlayer;

    // positions spécifique pour le zoom Boss 


    bool isLookingBoss = false;
    
 

   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        CameraRef = GetComponent<CinemachineCamera>();
        
        _follow=GetComponent<CinemachineFollow>();
        rotationComposer= GetComponent<CinemachineRotationComposer>();
        FocusOnBoss();
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }
    public void FocusOnBoss()
    {   
        
        CameraRef.Follow = TargetTransformPlayer;
        CameraRef.LookAt = TargetTranformBoss;
        
        _follow.FollowOffset = new Vector3(0, 25, -10);
        
        isLookingBoss = true;
        rotationComposer.Composition.ScreenPosition.x = 0f;

    }

    
    
    
}
