using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider soundSlider;

    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        soundSlider.minValue = 0f;
        soundSlider.maxValue = 1f;

        soundSlider.value = audioSource.volume;

        soundSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float value)
    {
        audioSource.volume = value;
    }
}
