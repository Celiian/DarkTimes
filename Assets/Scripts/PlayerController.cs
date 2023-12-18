using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private float m_JumpForce;

    [SerializeField]
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private float m_PlayerHeight;

    [SerializeField]
    private LayerMask m_GroundLayer;


    private bool _isGrounded = false;

    private bool _jump = false;



    void Update()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, m_PlayerHeight, m_GroundLayer);
       
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _jump = true;
        }

    }


    private void FixedUpdate()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            m_Rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * m_Speed, m_Rigidbody.velocity.y);
        }

        if (_jump)
        {
            _jump = false;
            m_Rigidbody.AddForce(new Vector2(0, m_JumpForce), ForceMode2D.Impulse);
        }
    }


}
