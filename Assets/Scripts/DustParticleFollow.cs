using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleFollow : MonoBehaviour

    
{
    public Transform player;
    // Start is called before the first frame update
  
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
        }
   
    }
}
