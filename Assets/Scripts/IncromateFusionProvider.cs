using DG.Tweening;
using UnityEngine;

public class IncromateFusionProvider : MonoBehaviour
{
    [HideInInspector] public int level = 1;
    [SerializeField] GameObject incromatePrefab;
    
    [HideInInspector] public bool isMerging;

    public void Initialize(int _level) {
        level = _level;
        transform.DOScale(level, .5f).SetEase(Ease.OutBack);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Incromate")) {
            var otherIncromate = other.gameObject.GetComponent<IncromateFusionProvider>();
            if (otherIncromate.level == level) {
                otherIncromate.isMerging = true;
                if (!isMerging) {
                    otherIncromate.enabled = false;
                    var pos = transform.position + (otherIncromate.transform.position - transform.position) / 2;
                    transform.DOScale(0f, .5f).SetEase(Ease.InBack).onComplete += () => {
                        Merge(pos); 
                        Destroy(gameObject);
                    };
                    return;
                }
                transform.DOScale(0f, 1f).SetEase(Ease.Linear).onComplete += () => {
                    Destroy(gameObject);
                };
            }
        }
    }

    void Merge(Vector3 position) {
        var newIncromate= Instantiate(incromatePrefab, position, Quaternion.identity);
        newIncromate.GetComponent<IncromateFusionProvider>().Initialize(level + 1);
    }
}