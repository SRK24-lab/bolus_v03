using UnityEngine;
using UnityEngine.UI;

public class SliderImageChanger : MonoBehaviour
{
    public Sprite spriteNormal;
    public Sprite spritePoopMax;

    public Slider imageSlider;
    public Image displayImage;

    private void Start()
    {
        if (imageSlider != null)
            imageSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    public void OnSliderValueChanged()
    {
        if (displayImage == null || imageSlider == null)
            return;

        float sliderValue = imageSlider.value;

        if (sliderValue >= imageSlider.maxValue - 0.01f)
        {
            if (displayImage.sprite != spritePoopMax)
                displayImage.sprite = spritePoopMax;
        }
        else
        {
            if (displayImage.sprite != spriteNormal)
                displayImage.sprite = spriteNormal;
        }
    }
}