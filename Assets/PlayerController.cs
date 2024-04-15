using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    MovementController movementController;

    public GameObject startNode;

    public Vector2 startPos;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startPos = new Vector2(-0.06f, -0.65f);
        movementController = GetComponent<MovementController>();
        movementController.lastMovingDirection = "left";

        startNode = movementController.currentNode;
    }

    public void Setup()
    {
        movementController.currentNode = startNode;
        movementController.direction = "";
        movementController.lastMovingDirection = "";
        transform.position = startPos;
    }

    public void Stop()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager.gameIsRunning)
        {
            return;
        }

        if(Input.GetKey(KeyCode.LeftArrow)){
            movementController.SetDirection("left");
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            movementController.SetDirection("right");
        }
        if(Input.GetKey(KeyCode.UpArrow)){
            movementController.SetDirection("up");
        }
        if(Input.GetKey(KeyCode.DownArrow)){
            movementController.SetDirection("down");
        }
    } 

    public void Death(){
        //animator.SetBool("moving", false);
        //animator.SetBool("dead", true);
    }
}
