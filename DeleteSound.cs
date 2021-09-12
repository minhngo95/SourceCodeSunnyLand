using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSound : MonoBehaviour
{
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }

}
