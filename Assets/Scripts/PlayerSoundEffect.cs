using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{

    [SerializeField] AudioSource m_FootStep1;
    [SerializeField] AudioSource m_FootStep2;

    public bool pause = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WalkSound1()
    {
        if (!pause)
        {
            m_FootStep1.Play();
        }
    }

    public void WalkSound2()
    {
        if (!pause)
        {
            m_FootStep2.Play();
        }
    }

}
