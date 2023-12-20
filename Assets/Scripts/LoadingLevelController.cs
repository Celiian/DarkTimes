using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLevelController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D doorTrigger;
    [SerializeField] 
    private LayerMask m_PlayerLayer;
    public string LevelToLoad;
    
    

    void LoadLevel()
    {

        Collider2D[] colliders = new Collider2D[100];

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(m_PlayerLayer);

        int numColliders = doorTrigger.OverlapCollider(contactFilter, colliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider2D collider = colliders[i];

            Debug.Log(collider.tag);
            if (collider.CompareTag("Joueur"))
            {
                SceneManager.LoadScene(LevelToLoad);
                break;
            }
        }




        
    }

    void Update()
    {
        LoadLevel();

    }
}
