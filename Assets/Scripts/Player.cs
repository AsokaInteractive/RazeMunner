using UnityEngine;
using CandyCoded.HapticFeedback;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public bool canMove = false, canVibrate = true;
    public ParticleSystem winPS;
    public float vibrateGap = 0.1f, vibrateWait = 1f;

    public enum VibrationType
    {
        Warning,
        Start,
        Lose,
        Collision
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Another Player Instance Found");
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        transform.position = Vector3.zero;
        //Invoke(nameof(StartGame), 1f);
    }
    public float moveSpeed = 1f;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !canMove)
        {
            canMove = true;
            transform.position = Vector3.zero;
        }
        if (canMove)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.Translate(new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0) * Time.deltaTime * moveSpeed);
        }
        if(Physics2D.CircleCast(transform.position, 0.5f, Vector3.forward, 0f, 1 << 6))
        {
            Debug.Log("Close to a boundary");
            StartCoroutine(WarningVibrate(VibrationType.Collision));
        }
    }
    private IEnumerator WarningVibrate(VibrationType vibe)
    {
        if(!canVibrate)
            yield break;
        switch(vibe)
        {
            case VibrationType.Collision:
                canVibrate = false;
                HapticFeedback.LightFeedback();
                yield return new WaitForSeconds(vibrateWait);
                canVibrate = true;
                break;
            case VibrationType.Lose:
                HapticFeedback.MediumFeedback();
                yield return new WaitForSeconds(vibrateWait);
                canVibrate = true;
                break;
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
            //GameManager.instance.NodeTouched(collision.GetComponent<Node>());
        }
    }
}