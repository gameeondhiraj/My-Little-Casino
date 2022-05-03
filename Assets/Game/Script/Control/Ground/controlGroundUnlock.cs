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
    private AudioManager AudioManager;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        AudioManager = FindObjectOfType<AudioManager>();
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
            if (!UnlockedObject.activeSelf)
            {
                Destroy(Instantiate(unlockPartical, spwanPosition.position, Quaternion.identity), 5);
                AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
            }
            LockedObject.SetActive(false);
            UnlockedObject.SetActive(true);
            isLocked = false;
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
