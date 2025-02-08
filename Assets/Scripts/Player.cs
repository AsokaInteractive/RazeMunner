using UnityEngine;

public class Player : MonoBehaviour
{
    private bool canMove = false;
    public ParticleSystem winPS;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        //Invoke(nameof(StartGame), 1f);
    }
    private void StartGame()
    {
        canMove = true;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            canMove = true;
        }
        if (canMove)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            print("Hit boundary!");
            canMove = false;
            GameManager.instance.Lose();
        }
        else if(collision.CompareTag("Node"))
        {
            Node.NodeType nodeType = collision.GetComponent<Node>().nodeType;
            switch (nodeType)
            {
                case Node.NodeType.Start:
                    GameManager.instance.StartGame();
                    print("Hit start node!");
                break;
                case Node.NodeType.End:
                    canMove = false;
                    winPS.Play();
                    GameManager.instance.Win();
                    print("Hit end node!");
                break;
                case Node.NodeType.Normal:
                    print("Hit normal node!");
                break;
            }
            GameManager.instance.NodeTouched(collision.GetComponent<Node>());
        }
    }
}