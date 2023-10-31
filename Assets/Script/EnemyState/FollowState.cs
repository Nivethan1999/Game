using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

class FollowState : IState
{

    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;   
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (parent.Target != null)
        {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.Speed * Time.deltaTime);
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if(distance <= parent.enemyAttackRange)
            {
                parent.changeState(new AttackState());
            }
        }
        else
        {
            parent.changeState(new IdleState());
        }

      
    }
}