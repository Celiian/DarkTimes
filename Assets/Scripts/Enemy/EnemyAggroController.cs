using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroController : MonoBehaviour
{
    [SerializeField] private float m_AggroRange;
    [SerializeField] private float m_DistanceToStop;
    [SerializeField] private float m_Height;
    [SerializeField] private bool m_PermanentPurchase;
    [SerializeField] public bool m_CheatPurchase;
    [SerializeField] public bool m_Big;
    [SerializeField] public Transform m_Player;
    [SerializeField] private LayerMask m_PlayerLayer;

    private System.Random random = new System.Random();
    private EnemyMovementsController _movements;
    public bool _isPlayerRight;
    public bool _playerJumpOver = false;
    public bool playerFound;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
        if (m_Player == null)
        {
            m_Player = FindObjectOfType<PlayerActionsController>().transform;
        }
    }


    private void Update()
    {
        if(_movements.getStunned() > 0)
        {
            return;
        }

        if (m_Player.GetComponent<PlayerActionsController>()._dead)
        {
            return;
        }

        if (m_CheatPurchase)
        {
            int rand = random.Next(1, 100);

            rand =- 50;

            m_DistanceToStop += (rand / 100);            
        }

        var distanceToPlayer = Vector2.Distance(transform.position, m_Player.position);


        var distanceXToPlayer = Mathf.Abs(transform.position.x - m_Player.position.x);

        _isPlayerRight = m_Player.position.x - transform.position.x >= 0.1f;

        var speed = _movements.m_Speed * 2;

        if (m_CheatPurchase)
        {
            if (distanceToPlayer > m_DistanceToStop)
            { 
                _movements.MoveToLocation(m_Player.position, speed);
            }
            return;
        }

        
        Vector2 raycastOrigin = transform.position;

        if (m_Big)
        {
            raycastOrigin.y = m_Player.position.y;
        }

        Vector2 raycastDirection = _movements.m_FacingLeft ? Vector2.left : Vector2.right;

      

        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, raycastDirection, m_AggroRange);

        playerFound = false;
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Joueur"))
            {
                if ((_isPlayerRight && !_movements.m_FacingLeft) || m_PermanentPurchase)
                {
                    playerFound = true;

                    _movements._purchasing = true;
                    if (distanceToPlayer > m_DistanceToStop)
                    {
                        _movements.MoveToLocation(m_Player.position, speed);
                    }
                }
                return;
            }

        }
        if (!playerFound)
        {

            if (distanceToPlayer < 2f && !_playerJumpOver && (_isPlayerRight == !_movements.m_FacingLeft))
            {
                _playerJumpOver = Physics2D.Raycast(transform.position, Vector2.up, m_AggroRange, m_PlayerLayer);
            }


            if (_playerJumpOver)
            {
                if (_isPlayerRight == _movements.m_FacingLeft)
                {
                    _movements.Flip();
                    _playerJumpOver = false;
                }
            }

            if (distanceXToPlayer > m_AggroRange && !m_PermanentPurchase)
            {
                _movements._purchasing = false;
            }
        }
    }
}
