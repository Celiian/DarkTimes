using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroController : MonoBehaviour
{
    [SerializeField] private float m_AggroRange;
    [SerializeField] private float m_DistanceToStop;
    [SerializeField] private float m_Height;
    [SerializeField] public Transform m_Player;
    [SerializeField] private LayerMask m_PlayerLayer;


    private EnemyMovementsController _movements;
    public bool _isPlayerRight;
    public bool _playerJumpOver = false;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
    }


    private void Update()
    {
        if(_movements.getStunned() > 0)
        {
            return;
        }

        var distanceToPlayer = Vector2.Distance(transform.position, m_Player.position);


        var distanceXToPlayer = Mathf.Abs(transform.position.x - m_Player.position.x);

        _isPlayerRight = m_Player.position.x - transform.position.x >= 0.1f;

        var speed = _movements.m_Speed * 2;

        Vector2 raycastOrigin = transform.position;

        Vector2 raycastDirection = _movements.m_FacingLeft ? Vector2.left : Vector2.right;

        if (distanceToPlayer < 2f && !_playerJumpOver && (_isPlayerRight == !_movements.m_FacingLeft))
        {
            _playerJumpOver = Physics2D.Raycast(transform.position, Vector2.up, 5f, m_PlayerLayer);
        }

      
        if (_playerJumpOver)
        {
            if(_isPlayerRight == _movements.m_FacingLeft)
            {
                _movements.Flip();
                _playerJumpOver = false;
            }
        }


        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, raycastDirection, m_AggroRange);

       

        var playerFound = false;
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Joueur"))
            {
                playerFound = true;


                _movements._purchasing = true;
                if (distanceToPlayer > 1.2f)
                {
                    _movements.MoveToLocation(m_Player.position, speed);
                }
                return;
            }

        }
        if (!playerFound && distanceXToPlayer > m_AggroRange)
        {
            _movements._purchasing = false;
        }
        else
        {
            _movements.MoveToLocation(m_Player.position, speed);
        }
    }
}