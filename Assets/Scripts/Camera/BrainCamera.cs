using System;
using Unity.Cinemachine;
using UnityEngine;

public class BrainCamera : MonoBehaviour
{   [SerializeField] private CinemachineCamera playerCamera;  
    [SerializeField] private CinemachineCamera bossCamera;    
    [SerializeField] private CinemachineCamera WallRightCamera;
    [SerializeField] private CinemachineCamera WallLeftCamera;
    [SerializeField] private CinemachineCamera WallDownCamera;
    bool FirstTopWallHit=false;
    
    private CinemachineBrain brain;
    [SerializeField] PlayerController player;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        FocusOnPlayer();
    }


    public void FocusOnWall(bool IsVertical, int WallSide, int WallUpDown)
    {
        if (IsVertical)
        {
            if (WallUpDown > 0)
            {
                FocusOnPlayer();
            }
            else
            {
                if (!FirstTopWallHit)
                {
                    player.SetMovementMode(new Tuple<bool, int>(IsVertical, WallSide));
                    FocusOnBoss();
                    FirstTopWallHit = true;
                }
                else
                {
                    SetCameraPriority(WallDownCamera, WallRightCamera, WallLeftCamera,bossCamera, playerCamera);
                }
              
            }
        
            
        }
        else
        {
            if (WallSide==-1)
            {    player.SetMovementMode(new Tuple<bool, int>(IsVertical, WallSide));
                print("Cas avec wall right");
                SetCameraPriority(WallRightCamera, WallLeftCamera,bossCamera,playerCamera,WallDownCamera);
            }

            if (WallSide==1)
            {   print("Cas avec wall left");
                player.SetMovementMode(new Tuple<bool, int>(IsVertical, WallSide));
                SetCameraPriority(WallLeftCamera, WallRightCamera, bossCamera, playerCamera, WallDownCamera);
            }
        }
    }
   
    public void FocusOnPlayer()
    {
        player.SetMovementMode(new Tuple<bool, int>(true,0));
        SetCameraPriority(playerCamera, bossCamera, WallRightCamera, WallLeftCamera, WallDownCamera);
        
    }

    
    public void FocusOnBoss()
    {
        player.SetMovementMode(new Tuple<bool, int>(true,0));
        SetCameraPriority(bossCamera, playerCamera, WallRightCamera, WallLeftCamera, WallDownCamera
        
        );
    }

    
    private void SetCameraPriority(CinemachineCamera highPriorityCamera, params CinemachineCamera[] lowPriorityCameras)
    {
        
        highPriorityCamera.Priority = 10;

        foreach (var camera in lowPriorityCameras)
        {
            camera.Priority = 0;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
