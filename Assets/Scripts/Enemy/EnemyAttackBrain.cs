using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBrain : MonoBehaviour
{

    [SerializeField] private float m_AttacksCoolDown = 1f;
    [SerializeField] private GameController m_GameController;

    private EnemyAttackController[] _attacks;


    private Transform _player;
    private EnemyAttackController[] _attackInRange = null;
    private EnemyAttackController[] _attackNotInCoolDown = null;
    private EnemyAttackController[] _attacksWithMaxDamage = null;

    private float _distanceToPlayer;
    private bool _globalCoolDown = false;

    private EnemyMovementsController _movements;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();

        _attacks  = gameObject.GetComponents<EnemyAttackController>();

        if (m_GameController == null)
        {
            m_GameController = FindFirstObjectByType<GameController>();
        }
        var agroController = gameObject.GetComponent<EnemyAggroController>();

        _player = agroController.m_Player;
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerActionsController>().transform;
        }

    }

    private void Update()
    {
        if(m_GameController.m_countdownTime == 0)
        {
            return;
        }
        if (_movements.wait)
        {
            return;
        }

        
        _distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        checkRange();
    }


    private void checkRange()
    {
        if (!_globalCoolDown) {
            return;
                }
        foreach (var attack in _attacks)
        {
            if (_distanceToPlayer <= attack.m_AttackRange)
            {
                _attackInRange = new EnemyAttackController[] { attack };
            }
        }
        checkCoolDown();
    }

    private void checkCoolDown()
    {
        foreach (var attack in _attackInRange)
        {
            if (!attack._attackInCoolDown)
            {
                _attackNotInCoolDown = new EnemyAttackController[] { attack };
            }
        }
        checkDamage();
    }


    private void checkDamage()
    {
        
        float maxDamage = float.MinValue;

        foreach (var attack in _attackNotInCoolDown)
        {
            if (attack.m_Strengh > maxDamage)
            {
                maxDamage = attack.m_Strengh;
                _attacksWithMaxDamage = new EnemyAttackController[] { attack };
            }
            else if (attack.m_Strengh == maxDamage)
            {
                EnemyAttackController[] newAttackArray = new EnemyAttackController[_attacksWithMaxDamage.Length + 1];
                for (int i = 0; i < _attacksWithMaxDamage.Length; i++)
                {
                    newAttackArray[i] = _attacksWithMaxDamage[i];
                }
                newAttackArray[_attacksWithMaxDamage.Length] = attack;
                _attacksWithMaxDamage = newAttackArray;
            }
        }

        if (_attacksWithMaxDamage != null && _attacksWithMaxDamage.Length > 0)
        {
            int randomIndex = Random.Range(0, _attacksWithMaxDamage.Length);
            EnemyAttackController chosenAttack = _attacksWithMaxDamage[randomIndex];

            chosenAttack.enabled = true;
            _globalCoolDown = true;

            Invoke(nameof(resetCoolDown), m_AttacksCoolDown);
        }
    }


  private void resetCoolDown()
    {
        _globalCoolDown = false;
    }

}
