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
    private float m_AttackRange;

    [SerializeField]
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private float m_PlayerHeight;

    [SerializeField]
    private LayerMask m_GroundLayer;

    [SerializeField]
    private LayerMask m_EnemyLayer;

    [SerializeField]
    private Animator animSpear;

    [SerializeField]
    private Animator animSword;

    [SerializeField]
    private bool m_Double_jump;



    private bool _isGrounded = false;

    private bool _jump = false;

    private bool _jumped_twice = false;

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

                if (Input.GetKeyDown(KeyCode.L))
                {

                    _spearMode = !_spearMode;
                }

                if (Input.GetKeyDown(KeyCode.K))
                {

                    anim.SetTrigger(attackMode);
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {

                    _sprint = true;
                }
                else
                {
                    _sprint = false;
                }
                                              
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_isGrounded)
                {
                    _jump = true;
                    _jumped_twice = false;
                    anim.SetTrigger(JumpMode);
                }
                else if (m_Double_jump)
                {
                    if (!_jumped_twice)
                    {
                        _jump = true;
                        anim.SetTrigger(JumpMode);
                        _jumped_twice = true;
                    }
                }
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
            Vector2 rayDirection = horizontalInput > 0 ? Vector2.right : Vector2.right;

            var AttackRange = _spearMode ? (float)( m_AttackRange * 1.6 ):( m_AttackRange);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, AttackRange, m_EnemyLayer);

            if (hit.collider != null)
            {
                Debug.Log("Touching enemy");
            }

        }
    }


    private void FixedUpdate()
    {
        if (_jump)
        {

            var force = _jumped_twice ? (float)(m_JumpForce * 1.1) : m_JumpForce;

            _jump = false;
            m_Rigidbody.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
        }
    }


}
