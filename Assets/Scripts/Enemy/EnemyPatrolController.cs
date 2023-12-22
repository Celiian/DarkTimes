using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    [SerializeField] public bool m_PatrolEnabled;

    [SerializeField] private float m_PatrolDistance;


    private EnemyMovementsController _movements;
    private bool _facingLeft;

    private Vector2 _originalPosition;
    private Vector2 _patrolTarget;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();

        _originalPosition = transform.position;
        _patrolTarget = _originalPosition - new Vector2(m_PatrolDistance, 0f);

    }


    void Patrol()
    {
        if (_movements.getStunned() > 0)
        {
            return;
        }

        if (m_PatrolEnabled && !_movements._purchasing) {
            if (Mathf.Abs(Vector2.Distance(transform.position, _patrolTarget)) <= 0.3f)
            {
                _patrolTarget = _facingLeft ? _originalPosition + new Vector2(m_PatrolDistance, 0f) : _originalPosition - new Vector2(m_PatrolDistance, 0f);
            }

            _movements.MoveToLocation(_patrolTarget, _movements.m_Speed);
        }
    }

    private void Update()
    {
        if (_movements.wait)
        {
            return;
        }

        _facingLeft = _movements.m_FacingLeft;
        Patrol();
    }
}
