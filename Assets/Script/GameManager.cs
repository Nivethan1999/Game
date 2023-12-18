using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Singleton instance
    public static GameManager Instance { get; private set; }

    private int totalEnemies; // Track the total number of enemies in the scene

    [SerializeField]
    private Player player;

    private NPC currentTarget;

    public Enemy[] enemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene(0); // Replace 0 with the index of your first scene
    }

    void Start()
    {
        // Count the total number of enemies in the scene
        enemies = FindObjectsOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        // Check if all enemies are dead
        if (AreAllEnemiesDead())
        {
            // If all enemies are dead, transition to the next scene
            TransitionToNextScene();
        }
    }

    private bool AreAllEnemiesDead()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                return false; // At least one enemy is still alive
            }
        }
        return true; // All enemies are dead
    }

    // Transition to the next scene
    //private void TransitionToNextScene()
    //{
    //    // You can load the next scene using SceneManager.LoadScene
    //    // For example, assuming the next scene is named "NextScene":
    //    SceneManager.LoadScene("Grass");
    //}

    private void TransitionToNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene exists before loading it
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If there is no next scene, you can restart the game or handle it as needed
            Debug.LogWarning("No next scene available. Restarting the game or handling as needed.");
            // Example: Restart the game by loading the first scene
            SceneManager.LoadScene(0);
        }
    }




    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //If we click left button
        {
            //Raycast from mouse position into the gameworld
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)
            {

                if (currentTarget != null)
                {
                
                    
                    currentTarget.DeSelect();
                    
                }


                currentTarget = hit.collider.GetComponent<NPC>();

                player.MyTarget = currentTarget.Select();

            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }

                currentTarget = null;
                player.MyTarget = null;
            }

        }

        }
    }

