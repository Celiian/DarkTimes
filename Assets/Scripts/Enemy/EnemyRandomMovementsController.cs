using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomMovementsController : MonoBehaviour
{

    [SerializeField] private bool m_RandomMovements;

    [SerializeField] private int m_RandomMovementsDistance;

    private System.Random random = new System.Random();

    private EnemyMovementsController _movements;
    private bool _facingLeft;

    private Vector2 _randomLocation = Vector2.zero;


    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
    }


    void MoveRandom()
    {
        if (_movements.getStunned() > 0)
        {
            return;
        }

        if (_randomLocation != Vector2.zero)
        {
            _movements.MoveToLocation(_randomLocation, _movements.m_Speed);

            if (Vector2.Distance(transform.position, _randomLocation) <= 0.1f)
            {
                _randomLocation = Vector2.zero;
            }
        }

        else
        {
            int randomNumber = random.Next(1, 11);
            if (randomNumber < 3)
            {
                int randDistance = random.Next(1, m_RandomMovementsDistance * 2);

                randDistance -= m_RandomMovementsDistance;

                _randomLocation = new Vector2(transform.position.x + randDistance, transform.position.y);
            }


        }
    }

    private void Update()
    {
        if (_movements.wait)
        {
            return;
        }

        if (m_RandomMovements && !_movements._purchasing)
        {
            MoveRandom();
        }
    }


}
