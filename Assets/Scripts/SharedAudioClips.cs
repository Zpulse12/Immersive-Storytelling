using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SharedAudioClips", menuName = "ScriptableObjects/SharedAudioClips")]
public class SharedAudioClips : ScriptableObject
{
    public AudioClip[] audioClips;
}