using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        
    }


    void IState.Update()
    {
        if(parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if(distance >= parent.enemyAttackRange)
            {
                parent.changeState(new FollowState());
            }
        }
        else
        {
            parent.changeState(new IdleState());
        }
    }
}