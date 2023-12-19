using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private bool m_FacingLeft;

    [SerializeField]
    private float m_PatrolDistance = 1f;

    private Vector2 m_OriginalPosition;
    private Vector2 m_PatrolTarget;

    private System.Random random = new System.Random();

    private bool _RandomLocationSet = false;
    private Vector2 _RandomLocation = Vector2.zero;

    void Start()
    {
        m_OriginalPosition = transform.position;
        m_PatrolTarget = m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
    }

    public void UpdateMovement()
    {
        // Call movement-related logic here
        Patrol();
        MoveRandom();
    }

    void MoveRandom()
    {
        if (_RandomLocationSet)
        {
            MoveToLocation(_RandomLocation, 1);

            if (Vector2.Distance(transform.position, _RandomLocation) <= 0.1f)
            {
                _RandomLocationSet = false;
            }
        }
        else
        {
            int randomNumber = random.Next(1, 11);
            if (randomNumber < 3)
            {
                int randDistance = random.Next(1, 3 * 2);
                randDistance -= 3;

                _RandomLocation = new Vector2(transform.position.x + randDistance, transform.position.y);
                _RandomLocationSet = true;
            }
        }
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, m_PatrolTarget) <= 0.1f)
        {
            m_PatrolTarget = !m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
        }

        MoveToLocation(m_PatrolTarget, 1);
    }

    void MoveToLocation(Vector2 targetPosition, float speed)
    {
        var isTargetRight = targetPosition.x - transform.position.x > 0;

        if (isTargetRight && m_FacingLeft)
        {
            Flip();
        }
        else if (!isTargetRight && !m_FacingLeft)
        {
            Flip();
        }

        var direction = (targetPosition - (Vector2)transform.position).normalized;

        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * (m_Speed * speed), GetComponent<Rigidbody2D>().velocity.y);
    }

    void Flip()
    {
        m_FacingLeft = !m_FacingLeft;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
}
