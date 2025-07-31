using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class SoundSlider
{
    public string name;
    public Slider slider;
    public AudioSource audioSource;
    public TextMeshProUGUI volumeText;
}
public class M_soundSlider : MonoBehaviour
{
    [Header("Liste des sliders de sons")]
    public SoundSlider[] soundSliders;

    private void Start()
    {
        foreach (var sound in soundSliders)
        {
            if (sound.slider != null && sound.audioSource != null)
            {
                sound.slider.onValueChanged.AddListener((value) => OnSliderChanged(sound));
                OnSliderChanged(sound); // initialiser volume au départ
            }
            setupSlider(sound);
        }
    }

    private void OnSliderChanged(SoundSlider sound)
    {
        sound.audioSource.volume = sound.slider.value;

        if (sound.volumeText != null)
        {
            int percentage = Mathf.RoundToInt(sound.slider.value * 100f);
            sound.volumeText.text = percentage + " %";
        }
    }

    private void setupSlider(SoundSlider sound)
    {
        sound.slider.gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = sound.name;
    }
}
