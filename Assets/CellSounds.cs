using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip select;
    [SerializeField] private AudioClip flag;
    [SerializeField] private AudioClip flagError;

    private void Awake()
    {
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
    }
    public void ExplosionSFX()
    {
        audioSource.PlayOneShot(explosion);
    }
    public void SelectSFX()
    {
        audioSource.PlayOneShot(select);
    }
    public void FlagSFX()
    {
        audioSource.PlayOneShot(flag);
    }
    public void FlagErrorSFX()
    {
        audioSource.PlayOneShot(flagError, 0.2f);
    }
}
