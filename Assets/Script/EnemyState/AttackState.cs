using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;

    private float coolDown = 1; // Hard coded cooldown til 1

    private float extraRange = .1f;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        
    }


    void IState.Update()
    {
        Debug.Log("Attack");

        if (parent.AttackTime >= coolDown && !parent.isAttacking)
        {
            parent.AttackTime = 0;
            parent.StartCoroutine(Atack());
        }

        if(parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if(distance >= parent.enemyAttackRange+extraRange && !parent.isAttacking)
            {
                parent.changeState(new FollowState());
            }
        }
        else
        {
            parent.changeState(new IdleState());
        }
    }

    public IEnumerator Atack()
    {
        parent.isAttacking = true;
        parent.MyAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);
        parent.isAttacking = false;
    }
}
