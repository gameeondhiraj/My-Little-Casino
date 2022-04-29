using UnityEngine;
using TMPro;

public class controlGroundUnlock : MonoBehaviour
{
    [Header("Locked")]
    [Space(10)]
    public GameObject LockedObject;
    public GameObject unlockPartical;
    public Transform spwanPosition;
    public TextMeshProUGUI cashCounter;
    public float unlockingAmount;
    public int reduceAmountSpeed = 10;

    [Header("Unlocked")]
    [Space(10)]
    public GameObject UnlockedObject;

    [Header("Progress")]
    [Space(5)]
    public bool isPlayerNear;
    public bool isLocked;

    private GameManager GameManager;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        unlocked();
    }
    void unlocked()
    {
        if (!isLocked || unlockingAmount <= 0)
        {
            UnlockedObject.SetActive(true);
            LockedObject.SetActive(false);
        }
    }


    void Update()
    {
        moneyManagement();
    }
    void moneyManagement()
    {
        cashCounter.text = unlockingAmount.ToString("N0");
        if (isLocked && unlockingAmount <= 0)
        {
            isLocked = false;
            Destroy(Instantiate(unlockPartical, transform.position, Quaternion.identity), 5);
            LockedObject.SetActive(false);
            UnlockedObject.SetActive(true);
        }

        if (isLocked && isPlayerNear)
            moneyReducer();
    }
    void moneyReducer()
    {
        if (unlockingAmount > 0 && GameManager.maxCash > 0)
        {
            unlockingAmount -= reduceAmountSpeed * Time.deltaTime;
            GameManager.maxCash -= reduceAmountSpeed * Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        try
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<movePlayer>().direction.magnitude < 0.1f) isPlayerNear = true;
        }
        catch
        {
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isPlayerNear)
            isPlayerNear = false;
    }

}
