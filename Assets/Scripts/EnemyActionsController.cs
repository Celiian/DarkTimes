using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionsController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Animator animAttack;

    private bool _attacked = false;
    private float _attackDirection;
    private float _attackStrength;

    private int stunned = 0;

    public void TakeHit(float attackDirection, float attackStrength)
    {
        _attackDirection = attackDirection;
        _attackStrength = attackStrength;
        _attacked = true;
    }

    public void UpdateActions()
    {
        if (_attacked)
        {
            _attacked = false;
            stunned = 10;
            float forceMultiplier = _attackStrength / GetComponent<Rigidbody2D>().mass;
            Vector2 attackForce = new Vector2(_attackDirection * forceMultiplier, GetComponent<Rigidbody2D>().velocity.y);

            GetComponent<Rigidbody2D>().AddForce(attackForce, ForceMode2D.Impulse);
        }
        else if (stunned > 0)
        {
            stunned -= 1;
        }
    }
}
