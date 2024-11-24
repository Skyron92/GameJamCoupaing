using System;
using Unity.Cinemachine;
using UnityEngine;

public class BrainCamera : MonoBehaviour
{   [SerializeField] private CinemachineCamera playerCamera;  
    [SerializeField] private CinemachineCamera bossCamera;    
    [SerializeField] private CinemachineCamera WallRightCamera;
    [SerializeField] private CinemachineCamera WallLeftCamera;
    [SerializeField] private CinemachineCamera WallDownCamera;
    [SerializeField] private CinemachineCamera WallUpCamera;
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
                player.SetMovementMode(new Vector2Int(WallUpDown, 0));
                SetCameraPriority(WallUpCamera, WallRightCamera, WallDownCamera,WallLeftCamera,bossCamera,playerCamera);
            }
            else
            {
                if (!FirstTopWallHit)
                {   
                    player.SetMovementMode(new Vector2Int(1,0));
                    FocusOnBoss();
                    FirstTopWallHit = true;
                    print("focus on boss psq première fois ");
                }
                else
                {print("deuxième fois donc je focus mur ");
                    player.SetMovementMode(new Vector2Int(WallUpDown, 0));
                    SetCameraPriority(WallDownCamera, WallRightCamera, WallLeftCamera,bossCamera, playerCamera);
                }
              
            }
        
            
        }
        else
        {
            if (WallSide==-1)
            {    player.SetMovementMode(new Vector2Int(0, WallSide));
                print("Cas avec wall right");
                SetCameraPriority(WallRightCamera, WallLeftCamera,bossCamera,playerCamera,WallDownCamera);
            }

            if (WallSide==1)
            {   print("Cas avec wall left");
                player.SetMovementMode(new Vector2Int(0,WallSide));
                SetCameraPriority(WallLeftCamera, WallRightCamera, bossCamera, playerCamera, WallDownCamera);
            }
        }
    }
   
    public void FocusOnPlayer()
    {
        player.SetMovementMode(new Vector2Int(1,0));
        SetCameraPriority(playerCamera, bossCamera, WallRightCamera, WallLeftCamera, WallDownCamera);
        
    }

    
    public void FocusOnBoss()
    {
        player.SetMovementMode(new Vector2Int(1,0));
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
