using Unity.Cinemachine;
using UnityEngine;

public class BrainCamera : MonoBehaviour
{   [SerializeField] private CinemachineCamera playerCamera;  
    [SerializeField] private CinemachineCamera bossCamera;    
    [SerializeField] private CinemachineCamera WallRightCamera;
    [SerializeField] private CinemachineCamera WallLeftCamera;

    private CinemachineBrain brain;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        FocusOnPlayer();
    }


    public void FocusOnWall(bool IsVertical, int WallSide)
    {
        if (IsVertical)
        {
            FocusOnPlayer();
        }
        else
        {
            if (WallSide==-1)
            {
                print("Cas avec wall right");
                SetCameraPriority(WallRightCamera, WallLeftCamera,bossCamera,playerCamera);
            }

            if (WallSide==1)
            {   print("Cas avec wall left");
                SetCameraPriority(WallLeftCamera, WallRightCamera, bossCamera, playerCamera);
            }
        }
    }
   
    public void FocusOnPlayer()
    {
        SetCameraPriority(playerCamera, bossCamera, WallRightCamera, WallLeftCamera);
        
    }

    
    public void FocusOnBoss()
    {
        SetCameraPriority(bossCamera, playerCamera, WallRightCamera, WallLeftCamera);
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
