using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{

    [SerializeField] AudioSource m_FootStep1;
    [SerializeField] AudioSource m_FootStep2;

    [SerializeField] AudioSource m_SwordSlash1;


    [SerializeField] AudioSource m_SwordSlash2;

    [SerializeField] AudioSource m_SpearSlash1;

    [SerializeField] AudioSource m_SpearSlashTakeBack;

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

    public void SwordSlashSound1()
    {
        if (!pause)
        {
            m_SwordSlash1.Play();
        }
    }

    public void SwordSlashSound2()
    {
        if (!pause)
        {
            m_SwordSlash2.Play();
        }
    }

    public void SpearSlashSound1()
    {
        if (!pause)
        {
            m_SpearSlash1.Play();
        }
    }

    public void SpearSlashTakeBackSound()
    {
        if (!pause)
        {
            m_SpearSlashTakeBack.Play();
        }
    }
}
