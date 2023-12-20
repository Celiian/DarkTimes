using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionsController : MonoBehaviour
{
    // [SerializeField] private Animator anim;
    [SerializeField] private float m_Strengh;

    [SerializeField] public int m_HitPoint;

    [SerializeField] private float m_AttackCoolDown;

    [SerializeField] private Rigidbody2D m_Rigidbody;

    [SerializeField] private GameObject m_Self;

    [SerializeField] private GameObject m_GameController;


    private bool _attacked = false;
    private bool _attackInCoolDown = false;
    
    private float _attackDirection;
    private float _attackStrength;
    private Animator _anim;
    private EnemyMovementController _movementController;
    private GameController _gameController;

    private bool _facingLeft;

    private Transform _player; 

    private void Start()
    {
        _gameController = m_GameController.GetComponent<GameController>();

        _movementController = m_Self.GetComponent<EnemyMovementController>();

        _anim = _movementController.m_Anim;
    }

    public void takeHit(float attackDirection, float attackStrength)
    {
        if(m_HitPoint == 0)
        {
            return;
        }

        _attackDirection = attackDirection;
        _attackStrength = attackStrength;
        _attacked = true;

        ApplyHit(m_Rigidbody);
    }


    void ApplyHit(Rigidbody2D rb)
    {

        m_HitPoint -= 1;
        _anim.SetTrigger("Hit");

        m_Self.GetComponent<EnemyMovementController>().stun(1.5f);

        _attacked = false;

        Vector2 force = CalculateImpulseForce(_attackStrength, rb.mass);

        force.x = Mathf.Abs(force.x) * _attackDirection;

        m_Rigidbody.AddForce(force, ForceMode2D.Impulse);

        if(m_HitPoint == 0)
        {
            _anim.SetTrigger("Death");
        }
    }
    

    Vector2 CalculateImpulseForce(float strength, float mass)
    {


        float deltaVx = strength / mass;
        float deltaVy = 0;
        return new Vector2(deltaVx, deltaVy);
    }


    private void Update()
    {
        if(_movementController._stunned > 0 || _gameController.m_countdownTime == 0)
        {
            return;
        }

        if(m_HitPoint == 0)
        {
            return;
        }
        
        _facingLeft = _movementController.m_FacingLeft;
        _player = _movementController.m_Player;

        var distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        var isPlayerRight = _movementController._isPlayerRight;

        var speed = _movementController.m_Speed * 2;

        var facingPlayer = isPlayerRight && !_facingLeft || !isPlayerRight && _facingLeft;

        Vector2 raycastOrigin = transform.position;

        Vector2 raycastDirection = isPlayerRight ? Vector2.right : Vector2.left;

        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, raycastDirection, _movementController.m_aggroRange);

        var playerFound = false;

        foreach(var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Joueur"))
            {
                playerFound = true;
                _movementController._purchasing = true;
                if (distanceToPlayer > 1.2)
                {
                    _movementController.MoveToLocation(_player.position, speed);
                }
                if (distanceToPlayer <= 1.3)
                {
                    if (facingPlayer && !_attackInCoolDown)
                    {
                        _anim.SetTrigger("Attack");

                        var controller = hit.collider.GetComponent<PlayerActionsController>();

                        var attackDirection = _facingLeft ? -1 : 1;

                        Vector2 currentVelocity = m_Rigidbody.velocity;

                        var velocity = currentVelocity.x > 2f ? currentVelocity.x / 2f : 1f;

                        var attackStrength = (m_Strengh) * velocity;

                        controller.takeHit(attackDirection, attackStrength);
                        _attackInCoolDown = true;
                        Invoke(nameof(coolDown), m_AttackCoolDown);

                        break;
                    }
                }
            }

        }
        if (!playerFound)
        {
            _movementController._purchasing = false;
        }

        


    }


    void coolDown()
    {
        _attackInCoolDown = false;
    }

}
