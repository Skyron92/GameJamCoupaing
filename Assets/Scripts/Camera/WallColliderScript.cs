using System;
using Unity.VisualScripting;
using UnityEngine;

public class WallColliderScript : MonoBehaviour
{
    [SerializeField] BrainCamera _brainCamera;
    public bool isVertical;
    public int WallSide;
    public int WallTop;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          _brainCamera.FocusOnWall(isVertical, WallSide, WallTop);
        }
    }
}
