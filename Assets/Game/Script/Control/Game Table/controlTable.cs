using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class controlTable : MonoBehaviour
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
        gWaitTImer.transform.forward = -Camera.main.transform.forward;
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
            if(Customer.Count>=customerLimit)
                startTrade = Customer.TrueForAll(Customer => Customer.GetComponent<moveCustomer>().startTrading == true);
        }
       /* if (Customer.Count > 0)
        {
            for(int i = 0; i <= Customer.Count - 1; i++)
            {
                if (Customer[i] == null)
                {
                    Customer.Remove(Customer[i]);
                }
                if (i >= Customer.Count - 1) i = 0;
            }            
        }*/
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
        if (!isBettingComplete && startTrade && timerNum > 0)
        {
            gWaitTImer.SetActive(true);
            timerNum -= Time.deltaTime;
            iWaitTimer.fillAmount = timerNum / fWaitTime;            
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
                GameObject c = Instantiate(chips, chipSpwanPos.position, Quaternion.identity);
                c.GetComponent<controlChipsObjects>().section = CustomerSpwan.section;

                if (i >= spwanCount - 1)
                {
                    if (!isBettingComplete)
                    {
                        timerNum = fWaitTime;
                        FindObjectOfType<GameManager>().CustomerCount += customerLimit;
                        resetTable();
                        LeanTween.delayedCall(1f, () =>{
                            if (gWaitTImer.activeSelf)
                                gWaitTImer.SetActive(false);
                            isBettingComplete = false;
                        });
                        isBettingComplete = true;
                    }                   
                }
            }
        }
    }

    void resetTable()
    {       
        for (int i = 0; i <= Customer.Count - 1; i++)
        {
            Customer[i].GetComponent<moveCustomer>().DestinationToExit = Customer[i].GetComponent<controlCustomer>().DestinationToExit;
            Customer[i].GetComponent<moveCustomer>().startTrading = false;                        
        }
    }
}
