using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine;

public class Player : Character
{

    [SerializeField]
    private Stat mana;

    private float initHealth = 100;

    private float initMana = 50;


    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    [SerializeField]
    private GameObject[] spellPrefab;



    public Transform MyTarget { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
     
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        //Debug.Log(LayerMask.GetMask("Block"));
        base.Update();

    }

    private void GetInput()
    {
        direction = Vector2.zero; 
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;

        }

        if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow)))
        {
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }

        //if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
        //{
        //    if (!isAttacking && !IsMoving)
        //    {
        //        attackRoutine = StartCoroutine(Attack());
        //    }

        //}
    }

    private IEnumerator Attack(int spellIndex)
    {

        isAttacking = true; // Indicates if we are attacking

        myAnimator.SetBool("attack", isAttacking); // Starts attack animation

        yield return new WaitForSeconds(1); // Hardcoded to cast til 1 seocond without movement

        Spell s = Instantiate(spellPrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();

        s.MyTarget = MyTarget;

        StopAttack(); // Stops attack

    }

    public void CastSpell(int spellIndex)
    {
        Block();

        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight()) // Checks if we are able to attack
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        }

    }

    private bool InLineOfSight()
    {
        if(MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }

        return false;
    }

    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

    public override void StopAttack()
    {


        base.StopAttack();
    }

}
