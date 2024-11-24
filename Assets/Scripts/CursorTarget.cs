using DG.Tweening;
using UnityEngine;

public class CursorTarget : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float scaleSpeed = 0.5f;
    [SerializeField, Range(0, .1f)] private float scaleTarget = 0.075f;
    [SerializeField, Range(0,10)] private float startTurnPerSecond = 2;
    [SerializeField, Range(0,10)] private float endTurnPerSecond = .5f;
    [SerializeField, Range(0,10)] private float decelerationSpeed = 2;
    
    private void Awake() {
        transform.DOScale(scaleTarget, scaleSpeed);
    }

    private void Update() {
        transform.Rotate(new Vector3(0, startTurnPerSecond * 360 * Time.deltaTime, 0)); 
        if(startTurnPerSecond > endTurnPerSecond) startTurnPerSecond -= Time.deltaTime * decelerationSpeed;
    }
}