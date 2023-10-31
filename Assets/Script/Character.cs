using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    protected Animator myAnimator;

    private Vector2 direction;

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
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

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
        myRigidbody.velocity = Direction.normalized * Speed;
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
            myAnimator.SetFloat("x", Direction.x);
            myAnimator.SetFloat("y", Direction.y);


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

    

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("Death");
        }
    }
        
}
