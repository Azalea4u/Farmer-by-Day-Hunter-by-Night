using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAudio_Data", menuName = "ScriptableObjects/DialogueAudio_Data")]
public class DialogueAudio_Data : ScriptableObject
{
    public string id;

    [SerializeField] public AudioClip[] dialogueTypingSoundClips;
    [SerializeField, Range(1, 5)] public int frequencyLevel = 2;
    [SerializeField, Range(-3, 3)] public float minPitch = 0.5f;
    [SerializeField, Range(-3, 3)] public float maxPitch = 2.0f;
    [SerializeField] public bool stopAudioSource;
}
