using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitController : MonoBehaviour
{


    [SerializeField] public int m_HitPoint;

    [SerializeField] private float m_TimeReward;

    [SerializeField] private float m_StunTime;

    [SerializeField] private GameController m_GameController;

    private float _attackDirection;
    private float _attackStrength;
    private EnemySpawner _enemySpawner;


    private EnemyMovementsController _movements;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
        if(m_GameController == null)
        {
            m_GameController = FindFirstObjectByType<GameController>();
        }
        _enemySpawner = FindFirstObjectByType<EnemySpawner>();
    }

    public void takeHit(float attackDirection, float attackStrength)
    {
        if (_movements.wait)
        {
            return;
        }


        if (m_HitPoint == 0)
        {
            return;
        }

        _attackDirection = attackDirection;
        _attackStrength = attackStrength;

        ApplyHit(_movements.m_Rigidbody);
    }


    void ApplyHit(Rigidbody2D rb)
    {

        m_HitPoint -= 1;
        _movements.m_Anim.SetTrigger("Hit");

        gameObject.GetComponent<EnemyMovementsController>().stun(m_StunTime);

        Vector2 force = CalculateImpulseForce(_attackStrength, rb.mass);

        force.x = Mathf.Abs(force.x) * _attackDirection;

        _movements.m_Rigidbody.AddForce(force, ForceMode2D.Impulse);

        if (m_HitPoint == 0)
        {
            if (_enemySpawner != null)
            {
                _enemySpawner._enemyNumber -= 1;
            }
            _movements.m_Anim.SetTrigger("Death");
            m_GameController.addTime(m_TimeReward);
        }
    }


    Vector2 CalculateImpulseForce(float strength, float mass)
    {
        float deltaVx = strength / mass;
        float deltaVy = 2f;
        return new Vector2(deltaVx, deltaVy);
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = _movements.m_Anim.GetCurrentAnimatorStateInfo(0);

        var dead = stateInfo.IsName("DeadEnemy");

        if (dead)
        {
            Destroy(gameObject);
        }

    }

}
