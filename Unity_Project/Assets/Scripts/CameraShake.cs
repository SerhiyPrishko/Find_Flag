using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }
}
