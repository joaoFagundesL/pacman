using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

public class EnemyController : MonoBehaviour
{

    public enum GhostNodeStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStatesEnum ghostNodeStates;

    public enum GhostColour
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostColour ghostColour;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeStart;

    public MovementController movementController;

    public GameObject startingNode;

    public bool readyToLeaveHome = false;

    void Awake()
    {
        movementController = GetComponent<MovementController>();

        if (ghostColour == GhostColour.red)
        {
            ghostNodeStates = GhostNodeStatesEnum.startNode;
            startingNode = ghostNodeStart;
        } 
        else if (ghostColour == GhostColour.pink)
        {
            ghostNodeStates = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;

        }
        else if (ghostColour == GhostColour.blue)
        {
            ghostNodeStates = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;

        }
        else if (ghostColour == GhostColour.orange)
        {
            ghostNodeStates = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        movementController.currentNode = startingNode;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReachedCenteOfNode(NodeController nodeController)
    {
        if (ghostNodeStates == GhostNodeStatesEnum.movingInNodes)
        {

        }
        else if (ghostNodeStates == GhostNodeStatesEnum.respawning)
        {

        }
        else
        {
            if (readyToLeaveHome)
            {
                if (ghostNodeStates == GhostNodeStatesEnum.leftNode)
                {
                    ghostNodeStates = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                else if (ghostNodeStates == GhostNodeStatesEnum.rightNode)
                {
                    ghostNodeStates = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("left");

                }
                else if (ghostNodeStates == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeStates = GhostNodeStatesEnum.startNode;
                    movementController.SetDirection("up");

                }
                else if (ghostNodeStates == GhostNodeStatesEnum.startNode)
                {
                    ghostNodeStates = GhostNodeStatesEnum.movingInNodes;
                    movementController.SetDirection("left");

                }
            }
        }
    }
}
