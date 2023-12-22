using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private int m_EnemyNumber;
    [SerializeField] private LayerMask m_PlayerLayer;
    [SerializeField] private BoxCollider2D m_Collider;
    [SerializeField] private GameObject myPrefab;
    [SerializeField] private GameObject[] m_Spawners;
    [SerializeField] private Transform m_Player;
    [SerializeField] private AudioSource m_Music;

    public int _enemyNumber;
    private bool triggered = false;


    void InvokeSpider()
    {
        if (_enemyNumber < m_EnemyNumber)
        {
            List<KeyValuePair<GameObject, float>> spawnersWithDistance = new List<KeyValuePair<GameObject, float>>();

            foreach (GameObject spawner in m_Spawners)
            {
                float distance = Vector2.Distance(m_Player.transform.position, spawner.transform.position);
                spawnersWithDistance.Add(new KeyValuePair<GameObject, float>(spawner, distance));
            }

            spawnersWithDistance.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));


            for (int i = 0; i < Mathf.Min(2, spawnersWithDistance.Count); i++)
            {
                GameObject closestSpawner = spawnersWithDistance[i].Key;
                Instantiate(myPrefab, closestSpawner.transform.position, Quaternion.identity);
                _enemyNumber += 1;


                if (_enemyNumber >= m_EnemyNumber)
                {
                    break;
                }
            }
        }
    }


 void Update()
    {
        if (triggered)
        {
            return;
        }

        Collider2D[] colliders = new Collider2D[10];

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(m_PlayerLayer);

        int numColliders = m_Collider.OverlapCollider(contactFilter, colliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider2D collider = colliders[i];

            if (collider.CompareTag("Joueur"))
            {
                triggered = true;
                InvokeRepeating(nameof(InvokeSpider), 0, 0.5f);
                m_Music.Play();
            }
        }
    }

}
