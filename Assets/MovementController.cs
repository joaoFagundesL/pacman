using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public GameManager gameManager;

    public GameObject currentNode;
    public float speed = 3f; // velocidade de movimentação

    public string direction = "";
    public string lastMovingDirection = ""; // caso o usuário mova-se para uma direção inválida,
                                            // é utilizada a ultima direção válida como valor.
    public bool canWarp = true;

    public bool isGhost = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

        // movimentação é com base no tempo do relógio para que computadores mais potentes não atualizem de forma diferente dos demais
        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;
        if(
            (direction == "left" && lastMovingDirection == "right")
            || (direction == "right" && lastMovingDirection == "left")
            || (direction == "up" && lastMovingDirection == "down")
            || (direction == "down" && lastMovingDirection == "up")
        ){
            reverseDirection = true;
        }

        //Quando chegar no centro do node

        if((transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y) || reverseDirection)
        {

            if (isGhost)
            {
                GetComponent<EnemyController>().ReachedCenteOfNode(currentNodeController);
            }

            // Se chegar no centro do warp na esquerda, vai para a direita
            if(currentNodeController.isWarpLeftNode && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovingDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            // Se está no centro do warp da direita, vai para esquerda
            else if(currentNodeController.isWarpRightNode && canWarp) {
                currentNode = gameManager.leftWarpNode;
                direction = "right";
                lastMovingDirection = "right";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            // se não, encontra o proximo node que irá se mover
            else
            {
                if (currentNodeController.isGhostStartingNode && direction == "down" &&
                    (!isGhost || GetComponent<EnemyController>().ghostNodeStates != EnemyController.GhostNodeStatesEnum.respawning))
                {
                    direction = lastMovingDirection;
                }

                //Pegar o próximo node
                GameObject newNode = currentNodeController.GetNodeFromDirection(direction);
                if (newNode != null)
                {
                    currentNode = newNode;
                    lastMovingDirection = direction;
                }
                else
                {
                    direction = lastMovingDirection;
                    newNode = currentNodeController.GetNodeFromDirection(direction);
                    if (newNode != null)
                    {
                        currentNode = newNode;
                    }
                }
            }
        }
        else
        {
            canWarp = true;
        }
    }

    public void SetDirection(string newDirection){
        direction = newDirection;
    }
}
