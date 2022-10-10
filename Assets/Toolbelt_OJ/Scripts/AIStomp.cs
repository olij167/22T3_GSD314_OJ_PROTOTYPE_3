//sounds from: https://freesound.org/people/JanKoehl/sounds/85600/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class AIStomp : MonoBehaviour
{
    private AudioSource audioSource;
    private CharacterController controller;

    [SerializeField] private List<AudioClip> footstepSounds;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.velocity.magnitude > 2f && !audioSource.isPlaying)
        {
            //audioSource.volume = Random.Range(0.25f, 0.35f);
            audioSource.pitch = Random.Range(1f, 1.2f);
            audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
        }
    }
}
