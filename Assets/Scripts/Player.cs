using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            print("Hit boundary!");
            GameManager.instance.Lose();
        }
        else if(collision.CompareTag("Node"))
        {
            Node.NodeType nodeType = collision.GetComponent<Node>().type;
            switch (nodeType)
            {
                case Node.NodeType.Start:
                    GameManager.instance.StartGame();
                    print("Hit start node!");
                break;
                case Node.NodeType.End:
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
