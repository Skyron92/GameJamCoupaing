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
    private CinemachineThirdPersonFollow thirdPersonFollow;
    private CinemachineRotationComposer rotationComposer;
    private CinemachineCamera CameraRef; 
    
    public Transform TargetTranformBoss;
    public Transform TargetTransformPlayer;

    // positions spécifique pour le zoom Boss 

    private float VerticalArmLenghtBossFloat = 20f;
    private float CameraDistanceBossFloat = 25f;
    bool isLookingBoss = false;
    
 

   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        CameraRef = GetComponent<CinemachineCamera>();
        
        thirdPersonFollow=GetComponent<CinemachineThirdPersonFollow>();
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
        thirdPersonFollow.VerticalArmLength = VerticalArmLenghtBossFloat;
        thirdPersonFollow.CameraDistance = CameraDistanceBossFloat;
        isLookingBoss = true;
        rotationComposer.Composition.ScreenPosition.x = 0f;

    }

    
    
    
}
