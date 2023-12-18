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

    [SerializeField]
    private Animator anim;


    private bool _isGrounded = false;

    private bool _jump = false;

    private bool _attacking = false;


    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, m_PlayerHeight, m_GroundLayer);
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // Use layer 0 for the base layer.

        _attacking = stateInfo.IsName("Attack 1") ? true : false;

        if (!_attacking)
        {
            if (_isGrounded)
            {
                anim.SetBool("Sprint", Mathf.Abs(m_Rigidbody.velocity.x) > 1);


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _jump = true;

                }

                if (Input.GetKeyDown(KeyCode.E))
                {

                    anim.SetTrigger("Attack 1");

                }

                if (Mathf.Abs(horizontalInput) > 0.1f)
                {

                    m_Rigidbody.velocity = new Vector2(horizontalInput * m_Speed, m_Rigidbody.velocity.y);

                    if (horizontalInput > 0.01f)
                        transform.localScale = Vector3.one;
                    else if (horizontalInput < -0.01f)
                        transform.localScale = new Vector3(-1, 1, 1);

                }

            }
            else
            {
                anim.SetBool("Sprint", false);

            }
        }
    }


    private void FixedUpdate()
    {
        

        if (_jump)
        {
            _jump = false;
            m_Rigidbody.AddForce(new Vector2(0, m_JumpForce), ForceMode2D.Impulse);
        }
    }


}
