using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private float m_Strengh;
    [SerializeField] private float m_AttackCoolDown;
    [SerializeField] private float m_AttackRange;
    [SerializeField] private string m_AttackType;


    private bool _attacked = false;
    private bool _attackInCoolDown = false;
    private bool _facingLeft;
    private bool _isPlayerRight;
    private Transform _player;



    private EnemyMovementsController _movements;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
    }


    private void Update()
    {
        if (_movements.getStunned() > 0)
        {
            return;
        }

        var agroController = gameObject.GetComponent<EnemyAggroController>();

        _player = agroController.m_Player;

        _facingLeft = _movements.m_FacingLeft;

        var distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        _isPlayerRight = _player.position.x - transform.position.x >= 0.1f;

        var facingPlayer = _isPlayerRight && !_facingLeft || !_isPlayerRight && _facingLeft;

        Vector2 raycastOrigin = transform.position;

        Vector2 raycastDirection = _isPlayerRight ? Vector2.right : Vector2.left;

        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, raycastDirection, m_AttackRange);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Joueur"))
            {
                if (facingPlayer && !_attackInCoolDown)
                {
                    _movements.m_Anim.SetTrigger(m_AttackType);

                    var controller = hit.collider.GetComponent<PlayerActionsController>();

                    var attackDirection = _movements.m_FacingLeft ? -1 : 1;

                    Vector2 currentVelocity = _movements.m_Rigidbody.velocity;

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



    void coolDown()
    {
        _attackInCoolDown = false;
    }

}
