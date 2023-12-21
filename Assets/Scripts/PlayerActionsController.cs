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
    [SerializeField] public Animator m_SwordAttackSkill;
    [SerializeField] public Animator m_SpearAttackSkill;

    private bool _isGrounded = false;
    private bool _attacking = false;
    private bool _spearMode = false;
    public bool _dead = false;
    private float _horizontalInput;
    private bool _facingLeft;
    private Animator _anim;
    private GameController _gameController;

    private bool _attacked = false;
    private float _attackDirection;
    private float _attackStrength;


    private string _groundMode = "Grounded";
    private string _attackMode = "Attack";

    public void takeHit(float attackDirection, float attackStrength, float timing)
    {
        _attackDirection = attackDirection;
        _attackStrength = attackStrength;
        _attacked = true;

        Invoke(nameof(ApplyHit), timing);
    }


    void ApplyHit()
    {
        if (_attacked)
        {
            _attacked = false;

            _gameController.DecountTimer(5);

            Vector2 force = CalculateImpulseForce(_attackStrength, m_Rigidbody.mass);

            force.x = Mathf.Abs(force.x) * _attackDirection;

            m_Rigidbody.AddForce(force, ForceMode2D.Impulse);

            HitRender(true);

            Invoke("HitRenderDelayed", 0.3f);
        }
    }

    private void HitRenderDelayed()
    {
        HitRender(false);
    }


    private void HitRender(bool render)
    {
        var spriteRenderer = _anim.GetComponent<SpriteRenderer>();
        Material newMaterial = new Material(spriteRenderer.material);
        newMaterial.color = render ? Color.red : Color.white;
        spriteRenderer.material = newMaterial;
    }

    Vector2 CalculateImpulseForce(float strength, float mass)
    {


        float deltaVx = strength / mass;
        float deltaVy = 0;
        return new Vector2(deltaVx, deltaVy);
    }


    private void Start()
    {

        _gameController = m_GameController.GetComponent<GameController>();
    }


    public void die()
    {
        _dead = true;
        m_Rigidbody.velocity = new Vector2(0, 0);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
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
                Invoke(nameof(dash), 0.2f);
            }
            
            Invoke(nameof(HandleAttacking), 0.25f);
            if (!_spearMode)
            {
                Invoke(nameof(HandleAttacking), 0.5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            var skillAnim = _spearMode ? m_SpearAttackSkill : m_SwordAttackSkill;

            _anim.SetTrigger("Attack");
            skillAnim.SetTrigger("Skill");
            Invoke(nameof(HandleAttacking), 0.25f);
            Invoke(nameof(HandleAttacking), 0.30f);
            if (!_spearMode)
            {
                Invoke(nameof(HandleAttacking), 0.40f);
                Invoke(nameof(HandleAttacking), 0.45f);
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

                var controller = collider.GetComponent<EnemyHitController>();

                var attackDirection = _facingLeft ? -1 : 1;

                Vector2 currentVelocity = m_Rigidbody.velocity;

                var attackStrength = (_spearMode ? m_SpearAttackStrengh : m_SwordAttackStrengh) * (currentVelocity.x > 0 ? currentVelocity.x / 8 : 1);

                controller.takeHit(attackDirection, attackStrength);
            }
        }

        
    }
}
