using Unity.Cinemachine;
using UnityEngine;

public class BrainCamera : MonoBehaviour
{[SerializeField] private CinemachineCamera playerCamera;  
    [SerializeField] private CinemachineCamera bossCamera;    

    private CinemachineBrain brain;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        FocusOnPlayer();
    }

   
    public void FocusOnPlayer()
    {
        SetCameraPriority(playerCamera, bossCamera);
        
    }

    
    public void FocusOnBoss()
    {
        SetCameraPriority(bossCamera, playerCamera);
    }

    
    private void SetCameraPriority(CinemachineCamera highPriorityCamera, CinemachineCamera lowPriorityCamera)
    {
        
        highPriorityCamera.Priority = 10;

    
        lowPriorityCamera.Priority = 0;
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
