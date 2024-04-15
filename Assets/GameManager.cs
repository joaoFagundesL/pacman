using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pacman;

    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch = 0;

    public int score;
    public Text scoreText;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeStart;

    // Start is called before the first frame update
    void Awake()
    {
        // para permitir que o ghost possa identificar que h� um node abaixo dele
        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
        score = 0;
        currentMunch = 0;
        siren.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }

    public void CollectedPellet(NodeController nodeController)
    {
        if (currentMunch == 0)
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if (currentMunch == 1) 
        {
            munch2.Play();
            currentMunch = 0;
        }

        AddToScore(10);

    }
}
