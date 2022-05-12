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

    private GameObject Player;
    private GameManager GameManager;
    private AudioManager AudioManager;
    private controlMoneyUI controlMoney;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        controlMoney = FindObjectOfType<controlMoneyUI>();
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
    float x = 0.15f;
    void moneyReducer()
    {
        if (unlockingAmount > 0 && GameManager.maxCash > 0)
        {
            unlockingAmount -= reduceAmountSpeed * Time.deltaTime;
            GameManager.maxCash -= reduceAmountSpeed * Time.deltaTime;
            if (unlockingAmount > 5)
            {
                if (x > 0)
                    x -= Time.deltaTime;
                if (x <= 0)
                {
                    moneyDeductationUI();
                    x = 0.15f;
                }
            }
        }
    }

    public Transform t;
    void moneyDeductationUI()
    {
        GameObject UI = controlMoney.CashUI[controlMoney.CashUI.Count - 1].gameObject;
        controlMoney.CashUI.Remove(controlMoney.CashUI[controlMoney.CashUI.Count - 1]);
        UI.SetActive(true);

        UI.transform.position = Camera.main.WorldToScreenPoint(Player.transform.position + new Vector3(0, 1f, 0));
        controlMoney.SlotMachinePosition.position = Camera.main.WorldToScreenPoint(t.position);

        LeanTween.moveLocal(UI, controlMoney.SlotMachinePosition.localPosition, .5f).setOnComplete(() =>
        {
            //UI.transform.position = Camera.main.WorldToScreenPoint(Player.transform.position + new Vector3(0, 2, 0));
            UI.SetActive(false);
            controlMoney.CashUI.Add(UI.transform);
        });
    }
    private void OnTriggerStay(Collider other)
    {
        try
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<movePlayer>().direction.magnitude < 0.1f)
            {
                isPlayerNear = true;
                Player = other.gameObject;
            }
            }
        catch
        {
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isPlayerNear)
        {

            Player = null;
            isPlayerNear = false;
        }
            
    }

}
