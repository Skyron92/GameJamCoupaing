using System;
using Unity.Cinemachine;
using UnityEngine;


public class CameraScript : MonoBehaviour
{  
    private CinemachineThirdPersonFollow thirdPersonFollow;
    private CinemachineRotationComposer rotationComposer;
    private CinemachineCamera CameraRef; 
    [SerializeField] private Transform TargetTransformPlayer;
    [SerializeField] private Transform TargetTranformBoss;
    
    // Positions spécifique pour le zoom joueur 
    private float VerticalArmLenghtPlayerFloat = 3f;
    private float CameraDistancePlayerFloat = 15f;

    // positions spécifique pour le zoom Boss 

    private float VerticalArmLenghtBossFloat = 10f;
    private float CameraDistanceBossFloat = 40f;
    
    //variables déplacements côtés 
    private float LeftScreenPosXFloat = -0.5f;
    private float RightScreenPosXFloat = 0.5f;

   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        CameraRef = GetComponent<CinemachineCamera>();
        
        thirdPersonFollow=GetComponent<CinemachineThirdPersonFollow>();
        rotationComposer= GetComponent<CinemachineRotationComposer>();
        rotationComposer.Composition.ScreenPosition.x = LeftScreenPosXFloat;
        FocusOnPlayer();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FocusOnBoss();
            print("BOSS");
        }
        

    }

    private void FocusOnPlayer()
    {   
        CameraRef.Follow = TargetTransformPlayer;
        thirdPersonFollow.VerticalArmLength = VerticalArmLenghtPlayerFloat;
        thirdPersonFollow.CameraDistance = CameraDistancePlayerFloat;
        

    }

    private void FocusOnBoss()
    {
        CameraRef.Follow = TargetTranformBoss;
        thirdPersonFollow.VerticalArmLength = VerticalArmLenghtBossFloat;
        thirdPersonFollow.CameraDistance = CameraDistanceBossFloat;
        
    }

    
    
    
    
    
    private void CameraMoveRight()
    {
        rotationComposer.Composition.ScreenPosition.x = RightScreenPosXFloat;
    }

    private void CameraMoveLeft()
    {
        rotationComposer.Composition.ScreenPosition.x = LeftScreenPosXFloat;
    }
    
    
}
