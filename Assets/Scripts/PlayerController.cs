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
    private Animator animSpear;

    [SerializeField]
    private Animator animSword;


    private bool _isGrounded = false;

    private bool _jump = false;

    private bool _attacking = false;

    private bool _spearMode = false;

    private bool _sprint = false;

    private SpriteRenderer swordRenderer;

    private SpriteRenderer spearRenderer;

    private void Start()
    {
        swordRenderer = animSword.GetComponent<SpriteRenderer>();
        spearRenderer = animSpear.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if (_spearMode)
        {
            swordRenderer.enabled = false;
            spearRenderer.enabled = true;
        }
        else
        {
            swordRenderer.enabled = true;
            spearRenderer.enabled = false;
        }

        var anim = _spearMode ? animSpear : animSword;

        var horizontalInput = Input.GetAxis("Horizontal");

        var attackMode = "Attack";
        var sprintMode = "Move";
        var GroundMode = "Grounded";
        var JumpMode = "Jump";

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);


        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, m_PlayerHeight, m_GroundLayer);
        anim.SetBool(GroundMode, Physics2D.Raycast(transform.position, Vector2.down, (float)(m_PlayerHeight * 1.6), m_GroundLayer));


        _attacking = stateInfo.IsName(attackMode) ? true : false;
       
        if (!_attacking)
        {
            if (_isGrounded)
            {
                anim.SetBool(sprintMode, Mathf.Abs(m_Rigidbody.velocity.x) > 1);


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _jump = true;
                    anim.SetTrigger(JumpMode);

                }

                if (Input.GetKeyDown(KeyCode.L))
                {

                    _spearMode = !_spearMode;
                    _sprint = false;
                }

                if (Input.GetKeyDown(KeyCode.K))
                {

                    anim.SetTrigger(attackMode);
                    _sprint = false;
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _sprint = true;
                   
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    _sprint = false;

                }

                if (Mathf.Abs(horizontalInput) > 0.1f)
                {

                    var speed = (float)(m_Speed * (_sprint ? 1.7 : 1));

                    m_Rigidbody.velocity = new Vector2(horizontalInput * speed, m_Rigidbody.velocity.y);

                    if (horizontalInput > 0.01f)
                        transform.localScale = Vector3.one;
                    else if (horizontalInput < -0.01f)
                        transform.localScale = new Vector3(-1, 1, 1);

                }

            }
            else
            {
                if (Mathf.Abs(horizontalInput) > 0.1f)
                {

                    var speed = (float)(m_Speed * (_sprint ? 1.7 : 1));

                    m_Rigidbody.velocity = new Vector2(horizontalInput * speed, m_Rigidbody.velocity.y);

                    if (horizontalInput > 0.01f)
                        transform.localScale = Vector3.one;
                    else if (horizontalInput < -0.01f)
                        transform.localScale = new Vector3(-1, 1, 1);

                }

                // anim.SetBool(sprintMode, false);

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
