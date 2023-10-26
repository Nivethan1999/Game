using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell
{
    [SerializeField]
    private string name;


    [SerializeField]
    private string damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

   

    [SerializeField]
    private GameObject spellPrefab;

    public string MyName { get => name; }
    public string MyDamage { get => damage;  }
    public Sprite MyIcon { get => icon;  }
    public float MySpeed { get => speed; }
    public GameObject MySpellPrefab { get => spellPrefab; }
}
