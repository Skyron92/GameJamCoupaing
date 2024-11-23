using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] Image fillImage;
    [SerializeField] Gradient gradient;
    [SerializeField, Range(1, 20)] private float strengthAnimation = 20f;
    [SerializeField] private bool shake;
    [SerializeField, Range(1, 10)] private float startAnimationDuration = 2f;
    private RectTransform _rectTransform;
    [HideInInspector] public float maxValue;

    private void Start() {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
        _slider.maxValue = maxValue;
        _slider.DOValue(_slider.maxValue, startAnimationDuration);
        fillImage.color = gradient.Evaluate(1f);
    }
    
    public void SetSliderValue(float value) {
        _slider.DOValue(value, .5f);
        if(shake) _rectTransform.DOShakePosition(1f, strengthAnimation);
        fillImage.DOColor(gradient.Evaluate(ConvertCurrentValueToPercent()), .5f);
    }

    private float ConvertCurrentValueToPercent() {
        return _slider.value / _slider.maxValue;
    }

    public void OutAnimation() {
        _rectTransform.DOMoveY(-50f, 2f);
    }
}
