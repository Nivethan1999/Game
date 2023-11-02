using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IdleState : IState
{

    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        Debug.Log("Ilde");

        if (parent.Target != null)
        {
            parent.changeState(new FollowState());
        }
    }
}
