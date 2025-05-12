using UnityEngine;
using CandyCoded.HapticFeedback;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public bool canMove = false, canVibrate = true;
    public ParticleSystem winPS, losePS;
    public float vibrateGap = 0.1f, vibrateWait = 1f;

    public enum VibrationType
    {
        Warning,
        Start,
        Lose,
        Win,
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Another Player Instance Found");
            Instance.gameObject.SetActive(false);
        }
        Instance = this;
    }
    private void Start()
    {
        transform.position = Vector3.zero;
        //Invoke(nameof(StartGame), 1f);
    }
    public float moveSpeed = 1f, boundaryCheckDist = 0.25f;
    public LayerMask boundaryLayer;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !canMove)
        {
            // Convert mouse position to world point
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Perform 2D raycast
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            // Check if this object was hit
            if (hit.collider != null && hit.transform == transform)
            {
                canMove = true;
                transform.position = Vector3.zero;
                StartCoroutine(WarningVibrate(VibrationType.Start));
            }            
        }
        if (canMove)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.Translate(new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0) * Time.deltaTime * moveSpeed);
        }
        if(Physics2D.CircleCast(transform.position, boundaryCheckDist, Vector3.forward, 0f, boundaryLayer))
        {
            Debug.Log("Close to a boundary");
            StartCoroutine(WarningVibrate(VibrationType.Warning));
        }
    }
    private IEnumerator WarningVibrate(VibrationType vibe)
    {
        if(!PlayerPrefs.HasKey("canVibrate"))
            PlayerPrefs.SetInt("canVibrate", 1);
        if (PlayerPrefs.GetInt("canVibrate") == 0)
            yield break;
        switch(vibe)
        {
            case VibrationType.Warning:
                if (!canVibrate)
                    yield break;
                canVibrate = false;
                HapticFeedback.LightFeedback();
                yield return new WaitForSeconds(vibrateWait);
                canVibrate = true;
                break;
            case VibrationType.Lose:
                HapticFeedback.HeavyFeedback();
                yield return new WaitForSeconds(vibrateGap);
                HapticFeedback.HeavyFeedback();
                yield return new WaitForSeconds(vibrateGap);
                HapticFeedback.HeavyFeedback();
                GameManager.instance.Lose();
                break;
            case VibrationType.Start:
                HapticFeedback.MediumFeedback();
                yield return new WaitForSeconds(vibrateWait);
                HapticFeedback.MediumFeedback();
                break;
            case VibrationType.Win:
                HapticFeedback.LightFeedback();
                yield return new WaitForSeconds(vibrateGap);
                HapticFeedback.LightFeedback();
                yield return new WaitForSeconds(vibrateWait);
                HapticFeedback.MediumFeedback();
                yield return new WaitForSeconds(vibrateGap);
                HapticFeedback.MediumFeedback();
                yield return new WaitForSeconds(vibrateWait);
                HapticFeedback.HeavyFeedback();
                yield return new WaitForSeconds(vibrateGap);
                HapticFeedback.HeavyFeedback();
                GameManager.instance.Win();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            print("Hit boundary!");
            canMove = false;
            //GameManager.instance.Lose();
            losePS.Play();
            //GetComponent<TrailRenderer>().enabled = false;
            StartCoroutine(WarningVibrate(VibrationType.Lose));
        }
        else if(collision.CompareTag("Node"))
        {
            Node.NodeType nodeType = collision.GetComponent<Node>().nodeType;
            switch (nodeType)
            {
                case Node.NodeType.Start:
                    GameManager.instance.StartGame();
                    print("Hit start node!");
                    //StartCoroutine(WarningVibrate(VibrationType.Start));
                break;
                case Node.NodeType.End:
                    canMove = false;
                    winPS.Play();
                    //GameManager.instance.Win();
                    StartCoroutine(WarningVibrate(VibrationType.Win));
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