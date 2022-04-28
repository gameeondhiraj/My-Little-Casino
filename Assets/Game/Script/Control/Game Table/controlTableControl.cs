using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class controlTableControl : MonoBehaviour
{
    public List<GameObject> Customer = new List<GameObject>();
    public GameObject chips;
    public Transform chipSpwanPos;

    [Header("Wait Timer")]
    [Space(10)]
    public GameObject gWaitTImer;
    public Image iWaitTimer;
    public float fWaitTime;
    private float timerNum = 1;

    [Space(10)]
    public int customerLimit;
    public int spwanCount;

    bool startTrade;
    private CustomerSpwan CustomerSpwan;
    void Start()
    {
        timerNum = fWaitTime;
        CustomerSpwan = GetComponent<CustomerSpwan>();
    }

    // Update is called once per frame
    void Update()
    {
        customerCheck();
        SpwanChip();
        waitTimerUpdate();
    }
    void customerCheck()
    {
        if (CustomerSpwan)
        {
            foreach (Transform t in CustomerSpwan.spwanPosition)
            {
                if (!Customer.Contains(t.gameObject))
                    Customer.Add(t.gameObject);
            }

            //checkIfAllTrue
            if(Customer.Count>0)
                startTrade = Customer.TrueForAll(Customer => Customer.GetComponent<moveCustomer>().startTrading == true);
        }
        
    }

    public bool checkIfAllTrue()
    {
        foreach(var b in Customer)
        {
            if (true == b.GetComponent<moveCustomer>().startTrading)
                return true;
        }
        return false;
    }
    void waitTimerUpdate()
    {
        if (startTrade && timerNum > 0)
        {
            LeanTween.delayedCall(0.5f, () =>
            {
                 gWaitTImer.SetActive(true);
            timerNum -= Time.deltaTime;
            iWaitTimer.fillAmount = timerNum / fWaitTime;
            });
        }
        if (timerNum <= 0)
        {
           LeanTween.delayedCall(0.5f, () =>
            {
                if (gWaitTImer.activeSelf)
                    gWaitTImer.SetActive(false);
            });
        }
    }
    bool isBettingComplete;
    void SpwanChip()
    {
        if(!isBettingComplete && timerNum <= 0)
        {
            for (int i = 0; i <= spwanCount; i++)
            {
                GameObject c = Instantiate(chips, chipSpwanPos.position + new Vector3(0, 2, 0), Quaternion.identity);
                c.GetComponent<controlChipsObjects>().section = CustomerSpwan.section;
                if (i >= spwanCount - 1)
                {
                    isBettingComplete = true;
                    FindObjectOfType<GameManager>().CustomerCount += customerLimit;
                }
            }
        }
    }
}
