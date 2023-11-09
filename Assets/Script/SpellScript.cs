using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;


    public Transform MyTarget { get; private set; }

    private Transform source;

    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

    }

    public void Initialize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target; 
        this.damage = damage;
        this.source = source;
    }

    private void FixedUpdate()
    {
        if(MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position;

            myRigidbody.velocity = direction.normalized * speed;

            float angle = (float)Math.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidbody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
