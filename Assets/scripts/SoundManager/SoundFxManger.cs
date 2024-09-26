using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxManger : MonoBehaviour
{
    public static SoundFxManger instance;
    [SerializeField] private AudioSource m_AudioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(m_AudioSource, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource, clipLength);

    }


}
