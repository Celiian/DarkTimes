using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PunchingBallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private float m_speed;

    private bool _attacked = false;
    private float _attackDirection;
    private float _attackStrength;
    

    public void takeHit(float attackDirection, float attackStrength, Vector2 attackSpeed)
    {
        _attackDirection = attackDirection;
        _attackStrength = attackStrength;
        _attacked = true;

        ApplyHit(m_rigidbody, attackSpeed);
    }


    void ApplyHit(Rigidbody2D rb, Vector2 attackSpeed)
    {
        if (_attacked)
        {
            _attacked = false;

            Vector2 force = CalculateImpulseForce(_attackStrength, attackSpeed, rb.mass);

            force.x = Mathf.Abs(force.x) * _attackDirection;

            m_rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }

    Vector2 CalculateImpulseForce(float strength, Vector2 attackSpeed, float mass)
    {

        if(attackSpeed.x == 0)
        {
            attackSpeed.x = 1;
        }

        float deltaVx = (strength * attackSpeed.x) / mass;
        float deltaVy = deltaVx / (10 * mass);
        return new Vector2(deltaVx, deltaVy);
    }


}
