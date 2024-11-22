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
    private CinemachineThirdPersonFollow thirdPersonFollow;
    private CinemachineRotationComposer rotationComposer;
    private CinemachineCamera CameraRef; 
    [SerializeField] private Transform TargetTransformPlayer;
    public Transform TargetTranformBoss;
    
   
    // Variables internes
    private bool isTransitioning = false;

    private Transform currentTargetFollow;
    private Transform currentTargetLookAt;

    
    // Positions spécifique pour le zoom joueur 
    private float VerticalArmLenghtPlayerFloat = 1f;
    private float CameraDistancePlayerFloat = 5f;

    // positions spécifique pour le zoom Boss 

    private float VerticalArmLenghtBossFloat = 20f;
    private float CameraDistanceBossFloat = 25f;
    bool isLookingBoss = false;
    
    
    //variables déplacements côtés 
    private float ScreenPosXFloat = -0.15f;

   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        CameraRef = GetComponent<CinemachineCamera>();
        
        thirdPersonFollow=GetComponent<CinemachineThirdPersonFollow>();
        rotationComposer= GetComponent<CinemachineRotationComposer>();
        FocusOnPlayer();
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }

    public void FocusOnPlayer()
    {   
        currentTargetFollow = TargetTransformPlayer;
        currentTargetLookAt = TargetTransformPlayer;
        
        CameraRef.Follow = currentTargetFollow ;
        CameraRef.LookAt = currentTargetLookAt ;
        
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

    public void StartCouroutinedab(Transform targetTransform)
    {
        StartCoroutine(MakeTransition(targetTransform));
    }

    private IEnumerator MakeTransition(Transform targetTransform)
    {
        while (isTransitioning)
        {
            print("transition");
            CameraRef.transform.position = Vector3.Lerp(CameraRef.transform.position,targetTransform.position,20f);
            yield return new WaitForSeconds(3f);
        }
        FocusOnBoss();
    }
   
   
    
    public void CameraMoveHorizontally(float direction) {

        if  (!isLookingBoss)
        {
            rotationComposer.Composition.ScreenPosition.x = ScreenPosXFloat * direction;
        }
        
    }

    public void ChangeFOVWhenRun()
    {
        CameraRef.Lens.FieldOfView = 90f;
    }
    
    
}
