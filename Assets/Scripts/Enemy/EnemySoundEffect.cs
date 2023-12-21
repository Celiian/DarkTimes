using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEffect : MonoBehaviour
{

    [SerializeField] AudioSource m_EnemyFootStep1;

    [SerializeField] AudioSource m_EnemyFootStep2;

    [SerializeField] AudioSource m_EnemyAttack1;

    [SerializeField] AudioSource m_EnemyHit;

    [SerializeField] AudioSource m_EnemyDeath;


    public bool pause = false;

    public void EnemyWalkSound1()
    {
        if (!pause)
        {
            m_EnemyFootStep1.Play();
        }
    }

    public void EnemyWalkSound2()
    {
        if (!pause)
        {
            m_EnemyFootStep2.Play();
        }
    }

    public void m_EnemyAttack1Sound()
    {
        if (!pause)
        {
            m_EnemyAttack1.Play();
        }
    }

    public void m_EnemyHitSound()
    {
        if (!pause)
        {
            m_EnemyHit.Play();
        }
    }

    public void m_EnemyDeathSound()
    {
        if (!pause)
        {
            m_EnemyDeath.Play();
        }
    }
}
