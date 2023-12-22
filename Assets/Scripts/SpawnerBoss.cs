using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBoss : MonoBehaviour
{

    [SerializeField] private int m_EnemyNumber;
    [SerializeField] private float m_Timing;
    [SerializeField] private GameObject myPrefab;
    [SerializeField] private GameObject m_SpawnerRight;
    [SerializeField] private GameObject m_SpawnerLeft;
    [SerializeField] private Animator m_doorLeft;
    [SerializeField] private Animator m_doorRight;
    [SerializeField] private AudioSource m_Music;
    


    private int _enemyNumber;


    void InvokeEnemy()
    {
        if (_enemyNumber < m_EnemyNumber)
        {
            Instantiate(myPrefab, m_SpawnerLeft.transform.position, Quaternion.identity);
            m_doorLeft.SetTrigger("Open");
            _enemyNumber += 1;

            Instantiate(myPrefab, m_SpawnerRight.transform.position, Quaternion.identity);
            m_doorLeft.SetTrigger("Open");
            _enemyNumber += 1;

            Invoke(nameof(closeDoors), 2);
        }
    }

    void closeDoors()
    {
        m_doorLeft.SetTrigger("Close");
        m_doorRight.SetTrigger("Close");

    }

    private void Start()
    {
        InvokeRepeating(nameof(InvokeEnemy), 0, m_Timing);
        // m_Music.Play();

    }

}
