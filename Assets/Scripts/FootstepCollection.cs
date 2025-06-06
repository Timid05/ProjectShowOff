using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Footstep Collection", menuName = "Create New Footstep Collection")]

public class FootstepCollection : ScriptableObject
{ 
    public List<AudioClip> walkingSounds = new List<AudioClip>();
    public List<AudioClip> sprintingSounds = new List<AudioClip>();
}
