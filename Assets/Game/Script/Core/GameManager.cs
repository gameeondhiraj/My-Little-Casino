using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Seats = new List<GameObject>();

    [Header("Cash Counter")]    
    public float maxCash;
    public float cashCounterSpeed;
    private float currentCash;
    public TextMeshProUGUI cashUI;

    [Header("Level Manager")]
    [Space(5)]
    public int Level;
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI nextLevel;
    

    [Header("Counter")]
    public int SlotMachine;
    public TextMeshProUGUI tSlotMachine;
    public int SectionCount;
    public TextMeshProUGUI tSectionCount;
    public int CustomerCount;
    public int customerRequestedCount;
    public int tableCount;
    public TextMeshProUGUI tableCountText;
    

    [Header("Slider")]
    [Space(10)]
    public Slider customerCount;
    public float USpeed;
    public float DSpeed;

    [Header("Level Progress")]
    [Space(10)]
    public TextMeshProUGUI customerRequestText;
    public GameObject customerRequestUpdated;
    public GameObject LockedSection;
    public int RewardMoney;

    [Header("Table")]
    [Space(10)]
    public bool BJ;
    public bool RL;
    public bool CP;

    [Header("Staff")]
    [Space(10)]
    public int StaffHIreAmount = 450;


    private AudioManager AudioManager;
    void Start()
    {
        Application.targetFrameRate = 60;
        AudioManager = FindObjectOfType<AudioManager>();
        customerRequestedCount = MaxCustomerCount();
        currentCash = maxCash;
    }


    void Update()
    {
        cashCounter();
        counterUpdate();
        LevelUIManager();
        customerSlider();
        upgrade();

        tableCountText.text = tableCount.ToString("N0") + "/ 4";
    }

    void cashCounter()
    {
        currentCash = Mathf.Clamp(currentCash, 0, Mathf.Infinity);

        if (currentCash <= maxCash)
        {
            currentCash += cashCounterSpeed * Time.deltaTime;
            if (currentCash >= maxCash) currentCash = maxCash;
        }
        if (currentCash >= maxCash)
        {
            currentCash -= cashCounterSpeed * Time.deltaTime;
            if (currentCash <= maxCash) currentCash = maxCash;
        }        

        cashUI.text = currentCash.ToString("N0");
    }
    
    void LevelUIManager()
    {
        if (Level >= 0)
        {
            currentLevel.text = (Level + 1).ToString("N0");
            nextLevel.text = (Level + 2).ToString("N0");
        }
        customerRequestText.text = CustomerCount+" / "+customerRequestedCount.ToString("N0");

        customerCount.maxValue = customerRequestedCount;
        customerCount.value = tempCurrentCustomerCount;
    }
    private float tempCurrentCustomerCount;
    void customerSlider()
    {
        CustomerCount = (int)Mathf.Clamp(CustomerCount, 0, Mathf.Infinity);
        if (tempCurrentCustomerCount < CustomerCount)
        {
            tempCurrentCustomerCount += USpeed * Time.deltaTime;
            if (tempCurrentCustomerCount >= CustomerCount)
                tempCurrentCustomerCount = CustomerCount;
        }
        if (tempCurrentCustomerCount > CustomerCount)
        {
            tempCurrentCustomerCount -= DSpeed * Time.deltaTime;
            if (tempCurrentCustomerCount <= CustomerCount)
                tempCurrentCustomerCount = CustomerCount;
        }
        if (tempCurrentCustomerCount == CustomerCount)
            tempCurrentCustomerCount = CustomerCount;
    }
    void counterUpdate()
    {
        SlotMachine = Seats.Count;
        tSlotMachine.text = SlotMachine.ToString("N0");
        tSectionCount.text = SectionCount.ToString("N0") + "/3";
    }

    public void upgrade()
    {
        if(CustomerCount>=customerRequestedCount && !customerRequestUpdated.activeSelf)
        {
            customerRequestUpdated.SetActive(true);
        }
        if (Level == 4 || Level == 9)
        {
            LockedSection.SetActive(true);
        }
        else
        {
            LockedSection.SetActive(false);
        }
    }
    public void UpgradeLevel()
    {
        Level += 1;
        addReward();
        CustomerCount -= customerRequestedCount;
        customerRequestedCount = MaxCustomerCount();
        customerRequestUpdated.SetActive(false);
        AudioManager.source.PlayOneShot(AudioManager.levelUp);
    }

    public void addReward()
    {
        maxCash += Reward();
    }

    int truckCountData;
    public int MaxCustomerCount()
    {
        if (Level == 0)
            truckCountData = 2;

        if (Level == 1)
            truckCountData = 2;

        if (Level == 2)
            truckCountData = 4;

        if (Level == 3)
            truckCountData = 8;
        if (Level == 4)
            truckCountData = 12;

        if (Level == 5)
            truckCountData = 16;

        if (Level == 6)
            truckCountData = 20;

        if (Level == 7)
            truckCountData = 24;

        if (Level == 8)
            truckCountData = 28;

        if (Level == 9)
            truckCountData = 32;

        if (Level == 10)
            truckCountData = 36;

        if (Level == 11)
            truckCountData = 40;

        if (Level == 12)
            truckCountData = 44;

        if (Level == 13)
            truckCountData = 48;

        if (Level == 14)
            truckCountData = 52;

        if (Level == 15)
            truckCountData = 56;

        if (Level >= 16)
            truckCountData = Random.Range(56, 80);
        return truckCountData;
    }

    public int Reward()
    {
        if (Level == 0)
            RewardMoney = 100;

        if (Level == 1)
            RewardMoney = 100;

        if (Level == 2)
            RewardMoney = 250;

        if (Level == 3)
            RewardMoney = 500;

        if (Level == 4)
            RewardMoney = 0;

        if (Level == 5)
            RewardMoney = 1000;

        if (Level == 6)
            RewardMoney = 0;

        if (Level == 7)
            RewardMoney = 1500;

        if (Level == 8)
            RewardMoney = 2000;

        if (Level == 9)
            RewardMoney = 2500;

        if (Level == 10)
            RewardMoney = 0;

        if (Level == 11)
            RewardMoney = 4000;

        if (Level == 12)
            RewardMoney = 5000;

        if (Level == 13)
            RewardMoney = 6000;

        if (Level == 14)
            RewardMoney = 6500;

        if (Level == 15)
            RewardMoney = 7000;

        if (Level == 15)
            RewardMoney = 7500;

        return RewardMoney;
    }


}
