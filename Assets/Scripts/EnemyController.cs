using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private LayerMask m_PlayerLayer;

    [SerializeField]
    private bool m_FacingLeft;


    void Update()
    {

        var vector = m_FacingLeft ? Vector2.left : Vector2.right;


        var seing_player = Physics2D.Raycast(transform.position, vector, 100, m_PlayerLayer);

        Debug.Log("Seing player");

        if(seing_player.collider != null)
        {
            m_Rigidbody.velocity = new Vector2(vector.x * m_Speed, m_Rigidbody.velocity.y);

        }

    }
}
