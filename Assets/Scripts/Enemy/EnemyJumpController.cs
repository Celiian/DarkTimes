using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpController : MonoBehaviour
{

    [SerializeField] private float m_Height;
    [SerializeField] private float m_JumpForce;

    [SerializeField] private LayerMask m_GroundLayer;

    private EnemyMovementsController _movements;
    private EnemyAggroController _aggro;
    private bool _isGrounded = false;
    private bool _groundInFront = false;
    private bool _jumped = false;


    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
        _aggro = gameObject.GetComponent<EnemyAggroController>();


    }

    private void Update()
    {

        var direction = _movements.m_FacingLeft ? Vector2.left : Vector2.right;

        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, m_Height, m_GroundLayer);
        if(_isGrounded)
        {
            _jumped = false;

            float currentSpeed = Mathf.Abs(_movements.m_Rigidbody.velocity.x);

            float detectLimit = 0.5f * currentSpeed;

            if (_movements._purchasing)
            {
                var distanceXToPlayer = Mathf.Abs(transform.position.x - _aggro.m_Player.position.x);

                if (Mathf.Abs(detectLimit) > distanceXToPlayer)
                {

                }
                else
                {
                    Vector2 origin = transform.position;

                    _groundInFront = Physics2D.Raycast(origin, direction, detectLimit, m_GroundLayer);
                }
            }
            else
            { 
                Vector2 origin = transform.position;

                _groundInFront = Physics2D.Raycast(origin, direction, detectLimit, m_GroundLayer);
            }

        }

    }


    private void FixedUpdate()
    {
        if (_groundInFront)
        {
            _groundInFront = false;
            _jumped = true;
            _movements.m_Rigidbody.AddForce(new Vector2(0, m_JumpForce), ForceMode2D.Impulse);
        }
    }


}
