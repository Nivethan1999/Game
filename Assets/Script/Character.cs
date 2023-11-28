using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]


public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public Animator MyAnimator { get; set; }

    private Vector2 direction;

    private Rigidbody2D myRigidbody;

    public bool isAttacking { get; set; }

    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;
    
    [SerializeField]
    protected Stat health;

    [SerializeField]
    protected float initHealth;

    public Transform MyTarget { get; set; }

    public int numberOfEnemmies;

    public float knockbackForce = 0.1f;

    private Rigidbody2D enemyRigidbody;



    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    public bool IsAlive
    {
        get
        {
           return health.MyCurrentValue > 0;
        }
    }
   

    // Start is called before the first frame update
    protected virtual void Start()
    {
 
        health.Initialize(initHealth, initHealth);
        myRigidbody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();

        enemyRigidbody = GetComponent<Rigidbody2D>();
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
        if (IsAlive)
        {
            myRigidbody.velocity = Direction.normalized * Speed;
        }

       
    }

    public void AnimateMovement(Vector2 direction)
    {

    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            //Check if we are moving or standing still
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");

                //Set animation parameter so that he faces the correct direction
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);


            }
            else if (isAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                //Going to idle if we not are going
                ActivateLayer("IdleLayer");
            }

        }
        else
        {
            ActivateLayer("DeathLayer");
            
        }

        
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    

    public virtual void TakeDamage(float damage, Transform source, Vector3 attackDirection)
    {

        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidbody.velocity = Direction;
            MyAnimator.SetTrigger("die");
        }

        Vector3 knockback = -attackDirection.normalized * knockbackForce;
        enemyRigidbody.AddForce(knockback, ForceMode2D.Impulse);


    }

    

}


