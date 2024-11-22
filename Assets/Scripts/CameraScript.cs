using System;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;


public class CameraScript : MonoBehaviour
{  
    private CinemachineThirdPersonFollow thirdPersonFollow;
    private CinemachineRotationComposer rotationComposer;
    private CinemachineCamera CameraRef; 
    [SerializeField] private Transform TargetTransformPlayer;
    [SerializeField] private Transform TargetTranformBoss;
    [SerializeField] private Collider ColliderBoss;
    
    
    // Positions spécifique pour le zoom joueur 
    private float VerticalArmLenghtPlayerFloat = 1f;
    private float CameraDistancePlayerFloat = 5f;

    // positions spécifique pour le zoom Boss 

    private float VerticalArmLenghtBossFloat = 10f;
    private float CameraDistanceBossFloat = 15f;
    bool isLookingBoss = false;
    
    //variables déplacements côtés 
    private float ScreenPosXFloat = 0.3f;

   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        CameraRef = GetComponent<CinemachineCamera>();
        
        thirdPersonFollow=GetComponent<CinemachineThirdPersonFollow>();
        rotationComposer= GetComponent<CinemachineRotationComposer>();
        rotationComposer.Composition.ScreenPosition.x = ScreenPosXFloat;
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
        CameraRef.LookAt = TargetTransformPlayer;
        thirdPersonFollow.VerticalArmLength = VerticalArmLenghtPlayerFloat;
        thirdPersonFollow.CameraDistance = CameraDistancePlayerFloat;
        isLookingBoss=false;

    }

    public void FocusOnBoss()
    {
        CameraRef.LookAt = TargetTranformBoss;
        thirdPersonFollow.VerticalArmLength = VerticalArmLenghtBossFloat;
        thirdPersonFollow.CameraDistance = CameraDistanceBossFloat;
        isLookingBoss = true;
        rotationComposer.Composition.ScreenPosition.x = 0f;

    }
    
    public void CameraMoveHorizontally(float direction) {

        if  (!isLookingBoss)
        {
            rotationComposer.Composition.ScreenPosition.x = ScreenPosXFloat * direction;
        }
        
    }
    
}
