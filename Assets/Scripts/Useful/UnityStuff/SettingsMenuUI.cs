using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValueText;

    void Start()
    {
        musicToggle.isOn = AudioManager.Instance.IsMusicEnabled();
        volumeSlider.value = AudioManager.Instance.GetCurrentVolume();
        UpdateVolumeText(volumeSlider.value);
        
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        AudioManager.Instance.SetMusicEnabled(isOn);
    }

    private void OnVolumeSliderChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
        UpdateVolumeText(value);
    }

    private void UpdateVolumeText(float value)
    {
        volumeValueText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}