using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class LoreScript : MonoBehaviour {

    [SerializeField] private TMP_Text m_TextField;
    [SerializeField] private List<string> m_Text;
    [SerializeField] private float m_WaitTime;
    [SerializeField] private List<GameObject> m_Triggers;
    [SerializeField] private GameObject m_EndTrigger;
    [SerializeField] private PlayerMovementController m_Player;
    [SerializeField] private LayerMask m_PlayerLayer;

    private void Start()
    {
        m_TextField.enabled = false;
    }

    void Update()
    {
        int index = 0;
        int indexToRemove = -1;
        foreach(var trigger in m_Triggers)
        {
            var collider = trigger.GetComponent<BoxCollider2D>();

            Collider2D[] colliders = new Collider2D[10];

            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.SetLayerMask(m_PlayerLayer);

            int numColliders = collider.OverlapCollider(contactFilter, colliders);

            for (int i = 0; i < numColliders; i++)
            {
                Collider2D playerCollider = colliders[i];

                if (playerCollider.CompareTag("Joueur"))
                {
                    m_TextField.text = m_Text[index];
                    m_TextField.enabled = true;
                    indexToRemove = index;

                    m_Player._wait = true;
                    Invoke(nameof(stopWait), m_WaitTime);
                }
            }
            index++;
        }

        if(indexToRemove >= 0) { 
            m_Triggers.RemoveAt(indexToRemove);
            m_Text.RemoveAt(indexToRemove);
        }

        checkEnd();
    }

    void stopWait() {
        m_Player._wait = false;
    }


    void checkEnd()
    {
        var collider = m_EndTrigger.GetComponent<BoxCollider2D>();

        Collider2D[] colliders = new Collider2D[10];

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(m_PlayerLayer);

        int numColliders = collider.OverlapCollider(contactFilter, colliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider2D playerCollider = colliders[i];

            if (playerCollider.CompareTag("Joueur"))
            {
                m_TextField.enabled = false;
            }
        }
    }
}
