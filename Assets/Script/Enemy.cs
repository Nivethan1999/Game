using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private IState state;

    public float enemyAttackRange { get; set; }

    public float AttackTime { get; set; }

    [SerializeField]
    private float initAggroRange;

    public float MyAggroRange { get; set; }

    //Test
    [SerializeField]
    private int damage;

    private bool canDoDamage = true;

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }


    protected void Awake()
    {
        MyAggroRange = initAggroRange;
        enemyAttackRange = 1;
        changeState(new IdleState());
    }



    protected override void Update()
    {
        if (IsAlive)
        {
            if (!isAttacking)
            {
                AttackTime += Time.deltaTime;
            }

            state.Update();

           
        }
        base.Update();

        if(MyTarget != null && !Player.MyInstance.IsAlive)
        {
            changeState(new IdleState());
        }



    }

    public override Transform Select()
    {
        healthGroup.alpha = 1;

        return base.Select();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public override void TakeDamage(float damage, Transform source)
    {
        SetTarget(source);

        base.TakeDamage(damage, source);

        //OnHealthChanged(health.MyCurrentValue);
    }
   

 

    public void changeState(IState newstate)
    {
        if (state != null)
        {
            state.Exit();
        }

        state = newstate;

        state.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if(MyTarget == null)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        //this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        //OnHealthChanged(health.MyCurrentValue);


    }

    public void DoDamage()
    {
        if (canDoDamage)
        {
            Player.MyInstance.TakeDamage(damage, transform);
            canDoDamage = false;
        }

    }

    public void CanDoDamage()
    {
        canDoDamage = true;
    }

}
