using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    // Start is called before the first frame update

    public SpriteRenderer MySpriteRenderer { get; set; }

    private Color defaultColor;
    private Color fadedColor;

    public int CompareTo(Obstacle other)
    {
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
            return -1;
        return 0;
    }

    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = MySpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }
}
