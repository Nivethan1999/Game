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

    private float initMana = 50;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    private SpellBook spellBook;

    private Vector3 min, max;

    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }


    // Start is called before the first frame update
    protected override void Start()
    {

        spellBook = GetComponent<SpellBook>();
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        //Debug.Log(LayerMask.GetMask("Block"));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x,min.x,max.x),Mathf.Clamp(transform.position.y,min.y,max.y),transform.position.z);

        base.Update();

    }

    private void GetInput()
    {
        Direction = Vector2.zero; 
        
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
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }

        if(IsMoving)
        {
            StopAttack();
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(int spellIndex)
    {

        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);

        isAttacking = true; // Indicates if we are attacking

        MyAnimator.SetBool("attack", isAttacking); // Starts attack animation

        yield return new WaitForSeconds(1); // Hardcoded to cast til 1 seocond without movement

        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget, newSpell.MyDamage, transform);
        }

        

        

        StopAttack(); // Stops attack

    }

    public void CastSpell(int spellIndex)
    {
        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isAttacking && !IsMoving && InLineOfSight()) // Checks if we are able to attack
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


    public void StopAttack()
    {



        isAttacking = false;

        MyAnimator.SetBool("attack", isAttacking);

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }

}
