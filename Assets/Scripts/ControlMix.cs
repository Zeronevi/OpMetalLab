using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControlMix : MonoBehaviour
{
    [SerializeField] AudioMixer mixer = null;
    [SerializeField] string masterName = "MasterVolume";
    [SerializeField] string musicName = "MusicVolume";
    [SerializeField] string effectsName = "EffectsVolume";

    [SerializeField] Slider masterSlider= null;
    [SerializeField] Slider musicSlider = null;
    [SerializeField] Slider effectsSlider = null;

    public void updateSliders()
    {
        float aux = 0;
        mixer.GetFloat(masterName, out aux);
        masterSlider.value = Mathf.Pow(10, aux / 20);

        mixer.GetFloat(musicName, out aux);
        musicSlider.value = Mathf.Pow(10, aux / 20);

        mixer.GetFloat(effectsName, out aux);
        effectsSlider.value = Mathf.Pow(10, aux / 20);
    }

    public void SetMasterVolume()
    { 
        mixer.SetFloat(masterName, Mathf.Log10(masterSlider.value) * 20);
    }

    public void SetMusicVolume()
    {
        mixer.SetFloat(musicName, Mathf.Log10(musicSlider.value) * 20);
    }

    public void SetEffectsVolume()
    {
        mixer.SetFloat(effectsName, Mathf.Log10(effectsSlider.value) * 20);
    }

    private void OnEnable()
    {
        updateSliders();
    }

}
