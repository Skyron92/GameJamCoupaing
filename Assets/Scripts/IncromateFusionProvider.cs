using DG.Tweening;
using UnityEngine;

public class IncromateFusionProvider : MonoBehaviour
{
    [HideInInspector] public int level = 1;
    public GameObject incromatePrefab;

    private Incromate _incromate;

    public void SetIncromate(Incromate i) => _incromate = i;
    
    [HideInInspector] public bool canMerge;

    public void Initialize(int _level) {
        level = _level;
        transform.DOScale(level * .3f, .5f).SetEase(Ease.OutBack);
        transform.position = new Vector3(transform.position.x, level / 2, transform.position.z);
        _incromate.Health = level;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Incromate")) {
            // J'ai le doit de merge ?
            if(!canMerge) return;
            // Ok alors est-ce nous sommes compatibles, et est-ce que tu as le droit de merge aussi ?
            var otherIncromate = other.gameObject.GetComponent<IncromateFusionProvider>();
            if (otherIncromate.level != level && !otherIncromate.canMerge) return;
            // Ok, dans ce cas je nous interdis de merge désormais, je m'occupe de tout
            otherIncromate.canMerge = canMerge = false;
            // Je te fais disparaître
            otherIncromate.transform.DOScale(0f, 1f).SetEase(Ease.Linear).onComplete += () => {
                Destroy(otherIncromate.gameObject);
            };
            // Et je fais spawn notre fusion
            var pos = transform.position + (otherIncromate.transform.position - transform.position) / 2;
            transform.DOScale(0f, .5f).SetEase(Ease.InBack).onComplete += () => {
                Merge(pos);
            };
        }
    }

    void Merge(Vector3 position) {
        var newIncromate= Instantiate(incromatePrefab, position, Quaternion.identity);
        newIncromate.GetComponent<IncromateFusionProvider>().Initialize(level + 1);
        newIncromate.GetComponent<Incromate>().SetPlayerAndBindMovement(_incromate.Player);
        Destroy(gameObject);
    }

    public void Split() {
        if(level <= 2) return;
        Spawn();
        Spawn();
    }

    private void Spawn() {
        Vector3 pos = transform.position + new Vector3(Random.Range(-.5f, .5f),0,Random.Range(-.5f, .5f));
        var instance = Instantiate(incromatePrefab, pos, Quaternion.identity);
        var fusion = instance.GetComponent<IncromateFusionProvider>();
        fusion.Initialize(Random.Range(1, level - 1));
    }
}