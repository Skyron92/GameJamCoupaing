using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{

   [SerializeField] private RectTransform button;
   [SerializeField, Range(0f, 1000f)] private float targetPos;
   
   public void Reset() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }

   private void Awake() {
      transform.DOMoveY(targetPos, .5f).SetEase(Ease.OutBounce);
   }

   public void Scale(float scale) {
      button.DOScale(scale, .5f).SetEase(Ease.OutBounce);
   }
}
