using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class controlTable : MonoBehaviour
{
    public List<GameObject> Customer = new List<GameObject>();
    public List<Transform> chipSpwanPositionList = new List<Transform>();
    public GameObject chips;
    //public Transform chipSpwanPos;

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
    private AudioManager AudioManager;
    void Start()
    {
        timerNum = fWaitTime;
        CustomerSpwan = GetComponent<CustomerSpwan>();
        AudioManager = FindObjectOfType<AudioManager>();
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
        foreach (GameObject g in Customer)
        {
            if (g == null)
                Customer.Remove(g);
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
            
            StartCoroutine(WaitTImerUI(1));
        }
        if (timerNum <= 0)
        {
            if (gWaitTImer.activeSelf)
                gWaitTImer.SetActive(false);
        }
    }
    bool isBettingComplete;

    IEnumerator WaitTImerUI(float t)
    {
        yield return new WaitForSeconds(t);       
        if (!gWaitTImer.activeSelf) gWaitTImer.SetActive(true);
        timerNum -= Time.deltaTime;
        iWaitTimer.fillAmount = timerNum / fWaitTime;
    }
    void SpwanChip()
    {
        if(!isBettingComplete && timerNum <= 0)
        {
            for (int i = 0; i <= spwanCount; i++)
            {
                GameObject c = Instantiate(chips, chipSpwanPositionList[Random.Range(0,chipSpwanPositionList.Count-1)].position, Quaternion.identity);

                c.GetComponent<controlChipsObjects>().section = CustomerSpwan.section;
                c.GetComponent<controlChipForStack>().seatChipController = GetComponent<controlChipSpwanner>();
                c.GetComponent<controlChipsObjects>().force = 5f;

                if (i >= spwanCount - 1)
                {
                    if (!isBettingComplete)
                    {
                        AudioManager.source.PlayOneShot(AudioManager.chipDespensed);
                        FindObjectOfType<GameManager>().CustomerCount += customerLimit;
                        resetTable();
                        isBettingComplete = true;
                        StartCoroutine(brakeLoop(1f));
                    }                   
                }
            }
        }
    }

    IEnumerator brakeLoop(float t)
    {
        yield return new WaitForSeconds(t);
        if (gWaitTImer.activeSelf)
        {
            gWaitTImer.SetActive(false);           
        }
        timerNum = fWaitTime;
        isBettingComplete = false;
    }
    void resetTable()
    {       
        for (int i = 0; i <= Customer.Count - 1; i++)
        {
            Customer[i].GetComponent<moveCustomer>().DestinationToExit = Customer[i].GetComponent<controlCustomer>().DestinationToExit;
            Customer[i].GetComponent<moveCustomer>().startTrading = false;                        
            Customer[i].GetComponent<controlCustomer>().bettingComplete = true;                        
        }
    }
}
