using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource cellSound;
    public AudioSource btnSound;
    public AudioSource buildAcceptSound;
    public AudioSource chestSound;
    public static SoundController instance;
    private void Awake()
    {
        instance = this;
    }
}
