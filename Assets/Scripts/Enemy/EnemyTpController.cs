using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTpController : MonoBehaviour
{
   

    [SerializeField] public Animator m_AnimatorInitEffect;
    [SerializeField] private float m_EffectTiming;

    private EnemyMovementsController _movements;
    private EnemyAggroController _aggro;

    private Vector2 _targetPosition;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
        _aggro = gameObject.GetComponent<EnemyAggroController>();

    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = m_AnimatorInitEffect.GetCurrentAnimatorStateInfo(0);

        var tp_done = stateInfo.IsName("tpDone");

        if (tp_done)
        {
            tp(_movements._targetPosition, _movements.m_TpDistance, _movements.m_TpCoolDown);
            Appear();

        }
    }


    public void Disapear()
    {
        var spriteRenderer = _movements.m_Anim.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

    }

    private void Appear()
    {
        var spriteRenderer = _movements.m_Anim.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        _movements.wait = false;

    }

    public void tp(Vector2 targetPosition, float tpDistance, float tpCoolDown)
    {
        var direction = _movements.m_FacingLeft ? -1 : 1;

        Vector2 newPosition = new Vector2(targetPosition.x + (tpDistance * direction), targetPosition.y);

        transform.position = newPosition;

        _movements.m_Rigidbody.velocity = new Vector2(0, 0);

        _movements._tpCoolDown = true;
        Invoke(nameof(removeCoolDown), tpCoolDown);
    }

    private void removeCoolDown()
    {
        _movements._tpCoolDown = false;
    }

}
