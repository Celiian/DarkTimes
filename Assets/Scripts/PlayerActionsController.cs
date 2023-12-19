using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsController : MonoBehaviour
{
    


    [SerializeField] private float m_PlayerHeight;
    [SerializeField] private float m_SpearAttackStrengh;
    [SerializeField] private float m_SwordAttackStrengh;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private LayerMask m_EnemyLayer;
    [SerializeField] private Animator m_AnimSpear;
    [SerializeField] private Animator m_AnimSword;
    [SerializeField] private GameObject m_GameController;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private BoxCollider2D m_SwordAttackCollider;
    [SerializeField] private BoxCollider2D m_SpearAttackCollider;

    private bool _isGrounded = false;
    private bool _attacking = false;
    private bool _spearMode = false;
    private bool _dead = false;
    private float _horizontalInput;
    private bool _facingLeft;
    private Animator _anim;
    private GameController _gameController;


    private string _groundMode = "Grounded";
    private string _attackMode = "Attack";


    private void Start()
    {

        _gameController = m_GameController.GetComponent<GameController>();
    }


    public void die()
    {
        _dead = true;
        _anim.SetBool("Dead", true);
        m_Player.GetComponent<PlayerMovementController>().die();
    }

    public void UpdateWeaponMode(bool spearMode)
    {
        _spearMode = spearMode;
    }
    

    void declarations()
    {
        _facingLeft = m_Player.GetComponent<PlayerMovementController>().m_FacingLeft;

        _anim = _spearMode ? m_AnimSpear : m_AnimSword;
        _horizontalInput = Input.GetAxis("Horizontal");

        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, m_PlayerHeight, m_GroundLayer);
        _anim.SetBool(_groundMode, Physics2D.Raycast(transform.position, Vector2.down, m_PlayerHeight * 1.6f, m_GroundLayer));
        _attacking = stateInfo.IsName(_attackMode);

    }


    void Update()
    {
        declarations();

        if (_dead)
        {
            return;
        }

        if (!_attacking)
        {
            HandleGroundedInput();
        }
    }  
    
        

    private void HandleGroundedInput()
    {
       
        
        if (Input.GetKeyDown(KeyCode.K))
        { 
            _anim.SetTrigger("Attack");
            if (_anim.GetBool("Move"))
            {
        
                if (_spearMode) {
                    Invoke(nameof(dash), 0.3f);
                }
                else
                {
                    Invoke(nameof(dash), 0.2f);
                }
                
            }
            
            Invoke(nameof(HandleAttacking), 0.5f);
            if (!_spearMode)
            {
                if (_anim.GetBool("Move"))
                {
                    Invoke(nameof(dash), 0.4f);
                }
                Invoke(nameof(HandleAttacking), 1);
            }
        }

    }

    private void dash()
    {
        var attackDirection = _facingLeft ? -1 : 1;

        Vector2 currentVelocity = m_Rigidbody.velocity;

        Vector2 attackForce = new Vector2(attackDirection * Mathf.Abs(currentVelocity.x), currentVelocity.y);

        m_Rigidbody.AddForce(attackForce, ForceMode2D.Impulse);
    }

    private void HandleAttacking()
    {
        if (_dead)
        {
            return;
        }
        _gameController.DecountTimer(3);

        var attackCollider = _spearMode ? m_SpearAttackCollider : m_SwordAttackCollider;


        Collider2D[] colliders = new Collider2D[100];

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(m_EnemyLayer);

        int numColliders = attackCollider.OverlapCollider(contactFilter, colliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider2D collider = colliders[i];

            if (collider.CompareTag("Enemy"))
            {

                var controller = collider.GetComponent<PunchingBallController>();

                var attackDirection = _facingLeft ? -1 : 1;

                Vector2 currentVelocity = m_Rigidbody.velocity;

                var attackStrength = (_spearMode ? m_SpearAttackStrengh : m_SwordAttackStrengh) * (currentVelocity.x > 0 ? currentVelocity.x / 4 : 1);

                controller.takeHit(attackDirection, attackStrength, currentVelocity);
            }
        }

        
    }
}
