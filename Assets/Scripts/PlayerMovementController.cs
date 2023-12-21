using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_JumpForce;
    [SerializeField] private float m_PlayerHeight;
    [SerializeField] private bool m_DoubleJump;
    [SerializeField] public bool m_FacingLeft;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private Animator m_AnimSpear;
    [SerializeField] private Animator m_AnimSword;
    [SerializeField] private GameObject m_GameController;
    [SerializeField] private GameObject m_Player;


    public bool _isGrounded = false;
    private bool _jump = false;
    private bool _jumpedTwice = false;
    private bool _attacking = false;
    private bool _spearMode = false;
    private bool _sprint = false;
    private bool _dead = false;
    private float _horizontalInput;
    private Animator _anim;
    private GameController _gameController;


    private SpriteRenderer _swordRenderer;
    private SpriteRenderer _spearRenderer;
    private PlayerActionsController _playerAction;

    private string _attackMode = "Attack";
    private string _skillMode = "Skill";
    private string _groundMode = "Grounded";
    private string _sprintMode = "Move";
    private string _jumpMode = "Jump";

    private void Start()
    {
        _swordRenderer = m_AnimSword.GetComponent<SpriteRenderer>();
        _spearRenderer = m_AnimSpear.GetComponent<SpriteRenderer>();
        _gameController = m_GameController.GetComponent<GameController>();
        _playerAction = gameObject.GetComponent<PlayerActionsController>();

    }

    public void die()
    {
        _dead = true;
    }

    void declarations()
    {
        var skillAnim = _spearMode ? _playerAction.m_SpearAttackSkill : _playerAction.m_SwordAttackSkill;


        _anim = _spearMode ? m_AnimSpear : m_AnimSword;
        _horizontalInput = Input.GetAxis("Horizontal");

        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, m_PlayerHeight, m_GroundLayer);
        _anim.SetBool(_groundMode, Physics2D.Raycast(transform.position, Vector2.down, m_PlayerHeight * 1.6f, m_GroundLayer));
        _attacking = stateInfo.IsName(_attackMode);

        stateInfo = skillAnim.GetCurrentAnimatorStateInfo(0);
        _attacking = stateInfo.IsName(_skillMode);
    }

    private void UpdateWeaponMode()
    {
        m_Player.GetComponent<PlayerActionsController>().UpdateWeaponMode(_spearMode);

        if (_spearMode)
        {
            
            _swordRenderer.enabled = false;
            _spearRenderer.enabled = true;

            _swordRenderer.GetComponent<PlayerSoundEffect>().pause = true;
            _spearRenderer.GetComponent<PlayerSoundEffect>().pause = false;

        }
        else
        {
            _swordRenderer.enabled = true;
            _spearRenderer.enabled = false;


            _swordRenderer.GetComponent<PlayerSoundEffect>().pause = false;
            _spearRenderer.GetComponent<PlayerSoundEffect>().pause = true;
        }
    }

    void Update()
    {
        if (_dead)
        {
            return;
        }

        declarations();

        UpdateWeaponMode();

                  
        if (!_attacking)
        {
            if (_isGrounded)
            {
                HandleGroundedInput(_anim);

               
                _sprint = Input.GetKey(KeyCode.LeftShift);
                if (_sprint)
                {
                    _anim.speed = 1.7f; 
                }
                else
                {
                    _anim.speed = 1.0f;

                }

                _gameController.AccelerateTimer(_sprint);
            }

            HandleJumpInput(_anim);
            HandleMovement(_horizontalInput);
        }
        
    }


   
    private void HandleGroundedInput(Animator anim)
    {
     
        anim.SetBool(_sprintMode, Mathf.Abs(m_Rigidbody.velocity.x) > 1);
        _gameController.PauseTimer(Mathf.Abs(m_Rigidbody.velocity.x) > 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            _jumpedTwice = false;
            anim.SetTrigger(_jumpMode);
        }
        else if (m_DoubleJump && !_jumpedTwice && Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            anim.SetTrigger(_jumpMode);
            _jumpedTwice = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _spearMode = !_spearMode;
        }

    }


    private void HandleMovement(float horizontalInput)
    {
        var speedMultiplier = _sprint ? 1.7f : 1;
        var speed = m_Speed * speedMultiplier;

        if (horizontalInput != 0)
        {

            m_Rigidbody.velocity = new Vector2(horizontalInput * speed, m_Rigidbody.velocity.y);

        }
        if (horizontalInput > 0.01f)
        {
            m_FacingLeft = false;
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            m_FacingLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJumpInput(Animator anim)
    {
        var jumpMode = "Jump";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isGrounded)
            {
                _jump = true;
                _jumpedTwice = false;
                anim.SetTrigger(jumpMode);
            }
            else if (m_DoubleJump && !_jumpedTwice)
            {
                _jump = true;
                anim.SetTrigger(jumpMode);
                _jumpedTwice = true;
            }
        }
    }


    private void FixedUpdate()
    {
        if (_jump)
        {
            _gameController.DecountTimer(2);
            var force = _jumpedTwice ? m_JumpForce * 1.3f : m_JumpForce;
            _jump = false;

            // Set Y velocity to 0 before adding jump force
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);

            // Add the jump force
            m_Rigidbody.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
        }
    }

}
