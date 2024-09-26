using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManger : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;



    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSoundFxVolume(float volume)
    {
        m_AudioMixer.SetFloat("SoundFxVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }
}
