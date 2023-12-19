using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PunchingBallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private float m_speed;

    private bool _attacked = false;
    private float _attackDirection;
    private float _attackStrengh;

    public void takeHit(float attackDirection, float attackStrenght)
    {
        _attackDirection = attackDirection;
        _attackStrengh = attackStrenght;
        _attacked = true;
        
    }

  
    void Update()
    {
        if (_attacked)
        {
            _attacked = false;
            m_rigidbody.velocity = new Vector2(_attackDirection *  _attackStrengh, m_rigidbody.velocity.y);

        }
    }
}
