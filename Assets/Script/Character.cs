using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected Animator myAnimator;

    protected Vector2 direction;

    private Rigidbody2D myRigidbody;

    protected bool isAttacking = false;

    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;
    
    [SerializeField]
    protected Stat health;

    [SerializeField]
    protected float initHealth;


    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        myRigidbody.velocity = direction.normalized * speed;
    }

    public void AnimateMovement(Vector2 direction)
    {

    }

    public void HandleLayers()
    {
        //Check if we are moving or standing still
        if (IsMoving)
        {
            ActivateLayer("Walk");

            //Set animation parameter so that he faces the correct direction
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            StopAttack();

        }
        else if (isAttacking)
        {
            ActivateLayer("Attack");
        }
        else
        {
            //Going to idle if we not are going
            ActivateLayer("Idle");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void StopAttack()
    {
        isAttacking = false;

        myAnimator.SetBool("attack", isAttacking);

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("Death");
        }
    }
        
}
