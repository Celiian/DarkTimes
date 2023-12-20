using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] public float m_Speed;

    [SerializeField] public float m_aggroRange = 5f;

    [SerializeField] private bool m_PatrolEnabled;

    [SerializeField] private float m_PatrolDistance = 1f;

    [SerializeField] private bool m_RandomMovements;

    [SerializeField] private int m_RandomMovementsDistance;

    [SerializeField] public bool m_FacingLeft;

    [SerializeField] public Animator m_Anim;

    [SerializeField] private Rigidbody2D m_Rigidbody;

    [SerializeField] public Transform m_Player;



    private Vector2 m_OriginalPosition;
    private Vector2 m_PatrolTarget;
    public bool _isPlayerRight;
    public bool _purchasing = false;

    private System.Random random = new System.Random();

    private Vector2 _RandomLocation = Vector2.zero;

    public int _stunned = 0;

    public void stun(float duration)
    {
        _stunned = (int) duration * 60;
    }

    void Start()
    {
        m_OriginalPosition = transform.position;
        m_PatrolTarget = m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
    }

    
    void Update()
    {

        _isPlayerRight = m_Player.position.x -  transform.position.x >= 0.1f;

        UpdateFacingPlayer(_isPlayerRight);

        m_Anim.SetBool("Move", Mathf.Abs(m_Rigidbody.velocity.x) > 0.1f);

        if (_stunned == 0)
        {
            if (!_purchasing)
            {
                if (m_PatrolEnabled)
                {
                    Patrol();
                }
                else if (m_RandomMovements)
                {
                    MoveRandom();
                }
            }
        }
        else if (_stunned > 0)
        {
            _stunned -= 1;
        }
    }


    void MoveRandom()
    {
        if (_RandomLocation != Vector2.zero)
        {
            MoveToLocation(_RandomLocation, m_Speed);

            if (Vector2.Distance(transform.position, _RandomLocation) <= 0.1f)
            {
                _RandomLocation = Vector2.zero;
            }
        }

        else
        {
            int randomNumber = random.Next(1, 11);
            if (randomNumber < 3)
            {
                int randDistance = random.Next(1, m_RandomMovementsDistance * 2);

                randDistance -= m_RandomMovementsDistance;

                _RandomLocation = new Vector2(transform.position.x + randDistance, transform.position.y);
            }


        }
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, m_PatrolTarget) <= 0.1f)
        {
            m_PatrolTarget = !m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
        }

        MoveToLocation(m_PatrolTarget, m_Speed);
    }

    void UpdateFacingPlayer(bool _isPlayerRight)
    {
        if (!m_FacingLeft && _isPlayerRight || m_FacingLeft && !_isPlayerRight)
        {
            //m_IsFacingPlayer = true;
        }
        else
        {
            // m_IsFacingPlayer = false;
        }
    }


    void Flip()
    {
        if (m_FacingLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            m_FacingLeft = false;
        }
        else
        {
            m_FacingLeft = true;
            transform.localScale = Vector3.one;

        }
    }

    public void MoveToLocation(Vector2 targetPosition, float speed)
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

        m_Rigidbody.velocity = new Vector2(direction.x * (m_Speed * speed), m_Rigidbody.velocity.y);
    }


}
