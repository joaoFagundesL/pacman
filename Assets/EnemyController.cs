using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
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
    public GhostNodeStatesEnum startGhostNodeState;
    public GhostNodeStatesEnum respawnState;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeStart;

    public MovementController movementController;

    public GameObject startingNode;


    public GameManager gameManager;

    public bool testRespawn = false;

    public bool isFrightened = false;

    public GameObject[] scatterNodes;
    public int scatterNodeIndex;
    public bool readyToLeaveHome = false;

    public bool leftHomeBefore = false;

    public bool isVisible = true;

    public SpriteRenderer ghostSprite;
    public SpriteRenderer eyesSprite;
    public SpriteRenderer frightenedSprite;

    //public Animator animator;
    public Color color;

    void Awake()
    {
        //animator = GetComponent<Animator>();
        ghostSprite = GetComponent<SpriteRenderer>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();

        if (ghostColour == GhostColour.red)
        {
            startGhostNodeState = GhostNodeStatesEnum.startNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            
        } 
        else if (ghostColour == GhostColour.pink)
        {
            startGhostNodeState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            respawnState = GhostNodeStatesEnum.centerNode;


        }
        else if (ghostColour == GhostColour.blue)
        {
            startGhostNodeState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
            respawnState = GhostNodeStatesEnum.leftNode;


        }
        else if (ghostColour == GhostColour.orange)
        {
            startGhostNodeState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
            respawnState = GhostNodeStatesEnum.rightNode;

        }
 
    }

    // Start is called before the first frame update
    void Start()
    {
        int[][] level = new int[][]
        {
              new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 2, 4, 0, 0, 4, 1, 4, 0, 0, 0, 4, 1, 4, 4, 1, 4, 0, 0, 0, 4, 1, 4, 0, 0, 4, 2, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 1, 1, 4, 4, 1, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 0, 0, 0, 0, 0, 0, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4 },
              new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 1, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1, 4 },
              new int[] { 4, 4, 4, 1, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 1, 4, 4, 4 },
              new int[] { 4, 4, 4, 1, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 1, 4, 4, 4 },
              new int[] { 4, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4 },
              new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 },
              new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
        };

        int xp = (int) GetXPacmanMatrixPosition();
        int yp = (int) GetYPacmanMatrixPosition();

        int xr = (int) GetXRedGhostMatrixPosition();
        int yr = (int) GetYRedGhostMatrixPosition();


        Node startNode = new Node(xp, yp);
        Node goalNode = new Node(xr, yr);

        List<Node> path = AStar.FindPath(level, startNode, goalNode);

        if (path != null)
        {
            Debug.Log("Caminho encontrado:");
            foreach (var node in path)
            {
                Debug.Log($"({node.x}, {node.y})");
            }
        }
        else
        {
            Debug.Log("Caminho não encontrado.");
        }

    }

    public void Setup()
    {
        //animator.SetBool("moving", false);

        ghostNodeStates = startGhostNodeState;
        readyToLeaveHome = false;
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;

        movementController.direction = "";
        movementController.lastMovingDirection = "";

        scatterNodeIndex = 0;

        isFrightened = false;

        leftHomeBefore = false;

        if (ghostColour == GhostColour.red)
        {
            readyToLeaveHome = true;
            leftHomeBefore = true;
        }
        else if (ghostColour == GhostColour.pink)
        {
            readyToLeaveHome = true;
        }
        SetVisible(true);
    }

    // Update is called once per frame
    void Update()
    {

        if(ghostNodeStates != GhostNodeStatesEnum.movingInNodes || !gameManager.isPowerPelletRunning){
            isFrightened = false;
        }
        
        //Mostra nossos sprites
        if(isVisible){
            if(ghostNodeStates != GhostNodeStatesEnum.respawning){

                ghostSprite.enabled = true;
            }
            else{
                ghostSprite.enabled = false;
            }
            eyesSprite.enabled = true;
            //frightenedSprite.enabled = false;
        }
        //Esconde nossos sprites
        else{
            ghostSprite.enabled = false;
            eyesSprite.enabled = false;
            //frightenedSprite.enabled = false;
        }  

       if(isFrightened){
            //animator.SetBool("frightened", true);
            frightenedSprite.enabled = true;
            ghostSprite.enabled = false;
            eyesSprite.enabled = false;
        }
        else{
            frightenedSprite.enabled = false;
            if(ghostNodeStates != GhostNodeStatesEnum.respawning){
                ghostSprite.enabled = true;
                eyesSprite.enabled = true;
            }
            else{
                ghostSprite.enabled = false;
            }


            //animator.SetBool("frightened", false);   
            //ghostSprite.color = color;       
        }
        if (!gameManager.gameIsRunning)
        {
            return;
        }



        if (testRespawn)
        {
            ghostNodeStates = GhostNodeStatesEnum.respawning;
            testRespawn = false;
        }

        if (movementController.currentNode.GetComponent<NodeController>().isSideNode) 
        {
            movementController.SetSpeed(1);
        }
        else
        {
            movementController.SetSpeed(1);

        }

    }

    public void SetFrightened(bool newIsFrightened){
        isFrightened = newIsFrightened;
    }

    public void ReachedCenteOfNode(NodeController nodeController)
    {
        if (ghostNodeStates == GhostNodeStatesEnum.movingInNodes)
        {
            leftHomeBefore = true;

            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {
                DetermineGhostScatterModeDirection();
            }
            else if (isFrightened)
            {
                string direction = GetRandomDirection();
                movementController.SetDirection(direction);
            }
            else
            {
                if (ghostColour == GhostColour.red)
                {
                    DetermineRedGhostDirection();
                }
                else if (ghostColour == GhostColour.pink)
                {
                    DeterminePinkGhostDirection();
                }
                else if (ghostColour == GhostColour.blue)
                {
                    DetermineBlueGhostDirection();
                }
                else if (ghostColour == GhostColour.orange)
                {
                    DetermineOrangeGhostDirection();
                }

            }
        }
        else if (ghostNodeStates == GhostNodeStatesEnum.respawning)
        {
            string direction = "";
            // alcancou o centro, move para baixo
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
            }

            // alcancou o centro, ou termina respawn ou move left/right (depende da cor do fantasma)
            else if(transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if (respawnState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeStates = respawnState;
                }
                else if (respawnState == GhostNodeStatesEnum.leftNode)
                {
                    direction = "left";
                }
                else if (respawnState == GhostNodeStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }

            else if (
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
                || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y)
                )
            {
                ghostNodeStates = respawnState;
            }
            else
            {
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }
            movementController.SetDirection(direction);
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

    string GetRandomDirection()
    {
        List<string> possibleDirections = new List<string>();

        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if (nodeController.canMoveDown && movementController.lastMovingDirection != "up")
        {
            possibleDirections.Add("down");
        }
        if (nodeController.canMoveUp && movementController.lastMovingDirection != "down")
        {
            possibleDirections.Add("up");
        }
        if (nodeController.canMoveRight && movementController.lastMovingDirection != "left")
        {
            possibleDirections.Add("right");
        }
        if (nodeController.canMoveLeft && movementController.lastMovingDirection != "right")
        {
            possibleDirections.Add("left");
        }

        string direction = "";
        int randomDirectionIndex = Random.Range(0, possibleDirections.Count - 1);
        direction = possibleDirections[randomDirectionIndex];
        return direction;

    }

    double GetXPacmanMatrixPosition()
    {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;

        double xInicial = -3.74f;
        double x = gameManager.pacman.transform.position.x;

        double possibleX = (xInicial - x) / 0.3;

        if (possibleX < 0)
        {
            possibleX *= -1;
        }

        double xMatrix = Math.Ceiling(possibleX);
        return xMatrix;
    }

    double GetYPacmanMatrixPosition()
    {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;

        double yInicial = -4.21f;
        double y = gameManager.pacman.transform.position.y;

        double possibleY = (yInicial - y) / 0.3;

        if (possibleY < 0)
        {
            possibleY *= -1;
        }

        double yMatrix = Math.Ceiling(possibleY);
        return yMatrix;
    }


    double GetYRedGhostMatrixPosition()
    {
        double y = gameManager.redGhost.transform.position.y;

        double yInicial = -4.21f;
  
        double possibleY = (yInicial - y) / 0.3;

        if (possibleY < 0)
        {
            possibleY *= -1;
        }
        double yMatrix = Math.Ceiling(possibleY);
        return yMatrix;

    }


    double GetXRedGhostMatrixPosition()
    {
        double x = gameManager.redGhost.transform.position.x;

        double xInicial = -3.74f;

        double possibleX = (xInicial - x) / 0.3;

        if (possibleX < 0)
        {
            possibleX *= -1;
        }

        double xMatrix = Math.Ceiling(possibleX);
        return xMatrix;
    }


    void DetermineGhostScatterModeDirection()
    {
        if (transform.position.x == scatterNodes[scatterNodeIndex].transform.position.x && transform.position.y == scatterNodes[scatterNodeIndex].transform.position.y)
        {
            scatterNodeIndex++;

            if (scatterNodeIndex == scatterNodes.Length - 1)
            {
                scatterNodeIndex = 0;
            }
        }

        string direction = GetClosestDirection(scatterNodes[scatterNodeIndex].transform.position);

        movementController.SetDirection(direction);
    }

    void DetermineRedGhostDirection()
    {

        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    void DeterminePinkGhostDirection()
    {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmansDirection == "left")
        {
            target.x -= (distanceBetweenNodes * 2);
        }
        else if (pacmansDirection == "right")
        {
            target.x += (distanceBetweenNodes * 2);
        }
        else if (pacmansDirection == "up")
        {
            target.y += (distanceBetweenNodes * 2);
        }
        else if (pacmansDirection == "down")
        {
            target.y -= distanceBetweenNodes * 2;
        }

        string direction = GetClosestDirection(target);
        movementController.SetDirection(direction);
    }

    void DetermineBlueGhostDirection()
    {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmansDirection == "left")
        {
            target.x -= (distanceBetweenNodes * 2);
        }
        else if (pacmansDirection == "right")
        {
            target.x += (distanceBetweenNodes * 2);
        }
        else if (pacmansDirection == "up")
        {
            target.y += (distanceBetweenNodes * 2);
        }
        else if (pacmansDirection == "down")
        {
            target.y -= distanceBetweenNodes * 2;
        }

        GameObject redGhost = gameManager.redGhost;
        float xDistance = target.x - redGhost.transform.position.x;
        float yDistance = target.y - redGhost.transform.position.y;

        Vector2 blueTarget = new Vector2(target.x + xDistance, target.y + yDistance);
        string direction = GetClosestDirection(blueTarget);
        movementController.SetDirection(direction);

    }

    void DetermineOrangeGhostDirection()
    {
        float distance = Vector2.Distance(gameManager.pacman.transform.position, transform.position);
        float distanceBetweenNodes = 0.35f;

        if (distance < 0)
        {
            distance *= -1;
        }

        if (distance <= distanceBetweenNodes * 8)
        {
            DetermineRedGhostDirection();
        }
        
        else
        {
            DetermineGhostScatterModeDirection();
        }
    }

    string GetClosestDirection(Vector2 target) 
    {
        float shortestDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";

        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if (nodeController.canMoveUp && lastMovingDirection != "down") 
        {
            GameObject nodeUp = nodeController.nodeUp;
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";
            }
        }


        if (nodeController.canMoveDown && lastMovingDirection != "up") 
        {
            GameObject nodeDown = nodeController.nodeDown;
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }
        }

        if (nodeController.canMoveLeft && lastMovingDirection != "right") 
        {
            GameObject nodeLeft = nodeController.nodeLeft;
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";
            }
        }

        if (nodeController.canMoveRight && lastMovingDirection != "left") 
        {
            GameObject nodeRight = nodeController.nodeRight;
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";
            }
        }

        return newDirection;

    }

    public void SetVisible(bool newIsVisible){
        isVisible = newIsVisible;
    } 

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Player" && ghostNodeStates != GhostNodeStatesEnum.respawning){

            //Ser comido
            if (isFrightened){
                gameManager.GhostEaten();
                ghostNodeStates = GhostNodeStatesEnum.respawning;

            }
            //Comer jogador
            else{
                StartCoroutine(gameManager.PlayerEaten());
            }

        }
    }
}


public class Node : IComparable<Node>
{
    public int x;
    public int y;
    public int f;  // f = g + h (total estimated cost)
    public int g;  // accumulated cost from the start node
    public int h;  // heuristic (estimated cost to the goal node)
    public Node parent;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.f = 0;
        this.g = 0;
        this.h = 0;
        this.parent = null;
    }

    public int CompareTo(Node other)
    {
        return f.CompareTo(other.f);
    }
}

public class AStar
{
    private static readonly int[][] directions = { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 } };  // Movements: up, down, left, right

    public static List<Node> FindPath(int[][] level, Node start, Node goal)
    {
        SortedSet<Node> openList = new SortedSet<Node>();  // Priority queue for open nodes
        HashSet<Node> closedSet = new HashSet<Node>();     // Set of visited nodes

        openList.Add(start);

        while (openList.Count > 0)
        {
            Node current = openList.Min;
            openList.Remove(current);

            if (current.x == goal.x && current.y == goal.y)
            {
                // Path found, reconstruct and return the path
                return ReconstructPath(current);
            }

            closedSet.Add(current);

            foreach (var dir in directions)
            {
                int newX = current.x + dir[0];
                int newY = current.y + dir[1];

                if (IsValidMove(level, newX, newY))
                {
                    Node neighbor = new Node(newX, newY);
                    if (!closedSet.Contains(neighbor))
                    {
                        int newG = current.g + 1;  // Accumulated cost

                        neighbor.g = newG;
                        neighbor.h = CalculateHeuristic(neighbor, goal);
                        neighbor.f = newG + neighbor.h;
                        neighbor.parent = current;

                        if (!ContainsNode(openList, neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }
        }

        return null;  // Path not found
    }

    private static bool IsValidMove(int[][] level, int x, int y)
    {
        int numRows = level.Length;
        int numCols = level[0].Length;
        return x >= 0 && x < numRows && y >= 0 && y < numCols && level[x][y] != 4;  // Check bounds and not a wall
    }

    private static int CalculateHeuristic(Node current, Node goal)
    {
        // Manhattan distance as heuristic
        return Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y);
    }

    private static bool ContainsNode(SortedSet<Node> set, Node node)
    {
        foreach (var n in set)
        {
            if (n.x == node.x && n.y == node.y)
            {
                return true;
            }
        }
        return false;
    }

    private static List<Node> ReconstructPath(Node current)
    {
        List<Node> path = new List<Node>();
        while (current != null)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();  // Reverse to get the correct path from start to end
        return path;
    }
}
