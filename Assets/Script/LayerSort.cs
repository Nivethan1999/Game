using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LayerSort : MonoBehaviour
{
    private SpriteRenderer parent;

    private List<Obstacle> obstacles = new List<Obstacle>();

    void Start()
    {
        parent = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeOut();

            if (obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder -1 < parent.sortingOrder)
            {
                parent.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;
            }
            
            obstacles.Add(o);
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeIn();
            obstacles.Remove(o);
            if (obstacles.Count == 0)
            {
                parent.sortingOrder = 5000; // Sorting layer
            }
            else
            {
                obstacles.Sort();
                parent.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
        }   
    }
}
