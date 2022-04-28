using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlCustomer : MonoBehaviour
{
    private moveCustomer moveCustomer;

    public coreSection section;
    public GameObject chips;
    public Transform DestinationToExit;
    public int spwanCount= 5;
    public float delayTime = 1;

    [Header("Wait Timer")]
    [Space(10)]
    public GameObject gWaitTImer;
    public Image iWaitTimer;
    public float fWaitTime;
    private float timerNum;

    private bool bettingComplete;

    public bool isCustomerForTable;
    void Start()
    {
        moveCustomer = GetComponent<moveCustomer>();        
        gWaitTImer.SetActive(false);
        timerNum = fWaitTime;
    }
    
    void Update()
    {
       gWaitTImer.transform.forward = -Camera.main.transform.forward;
        if (!isCustomerForTable)
        {
            spwanChip();
            waitTimerUpdate();
        }        
    }
    //float x = 0.5f;
    void spwanChip()
    {
        if (!bettingComplete && timerNum <= 0)
        {
            for(int i=0;i<= spwanCount; i++)
            {
                GameObject c = Instantiate(chips, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                c.GetComponent<controlChipsObjects>().section = section;
                if (i >= spwanCount - 1)
                {
                    bettingComplete = true;
                    FindObjectOfType<GameManager>().CustomerCount++;
                }
            }
        }
    }
    void waitTimerUpdate()
    {
        if(moveCustomer.startTrading && timerNum > 0)
        {
            LeanTween.delayedCall(0.5f, () =>
            {
                gWaitTImer.SetActive(true);
                timerNum -= Time.deltaTime;
                iWaitTimer.fillAmount = timerNum / fWaitTime;
            });            
        }
        if (bettingComplete && !moveCustomer.DestinationToExit)
        {            
            LeanTween.delayedCall(0.5f, () =>
            {
                if(gWaitTImer.activeSelf)
                    gWaitTImer.SetActive(false);                

                moveCustomer.DestinationToExit = DestinationToExit;                
            });
            
        }
        
    }
}
