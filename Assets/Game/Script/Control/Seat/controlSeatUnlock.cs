using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class controlSeatUnlock : MonoBehaviour
{
    [Header("Locked")]
    [Space(10)]
    public GameObject LockedObject;
    public GameObject unlockPartical;
    public TextMeshProUGUI cashCounter;
    public float unlockingAmount;
    public int reduceAmountSpeed = 10;

    [Header("Unlocked")]
    [Space(10)]
    public GameObject UnlockedObject;

    [Header("Progress")]
    [Space(5)]
    public bool isPlayerNear;
    public bool isPlayerLeft;

    public GameObject image;
    public GameObject image1;


    private GameObject Player;
    private GameManager GameManager;
    private AudioManager AudioManager;
    private controlMoneyUI controlMoney;
    private controlSeat controlSeat;


    void Start()
    {
        controlSeat = this.GetComponent<controlSeat>();
        GameManager = FindObjectOfType<GameManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        controlMoney = FindObjectOfType<controlMoneyUI>();
        unlocked();
    }

    void unlocked()
    {
        if (!controlSeat.isLocked || unlockingAmount <= 0)
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
        if (controlSeat.isLocked && unlockingAmount <= 0)
        {
            Destroy(Instantiate(unlockPartical, transform.position, Quaternion.identity), 5);
            LockedObject.SetActive(false);
            UnlockedObject.SetActive(true);
            AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
            controlSeat.isLocked = false;
        }

        if (controlSeat.isLocked && isPlayerNear)
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
    void moneyDeductationUI()
    {
        GameObject UI = controlMoney.CashUI[controlMoney.CashUI.Count - 1].gameObject;
        controlMoney.CashUI.Remove(controlMoney.CashUI[controlMoney.CashUI.Count - 1]);
        UI.SetActive(true);

        UI.transform.position = Camera.main.WorldToScreenPoint(Player.transform.position + new Vector3(0, 1f, 0));
        controlMoney.SlotMachinePosition.position = Camera.main.WorldToScreenPoint(transform.position);

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
                Player = other.gameObject;
                isPlayerNear = true;
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
