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

    void Start()
    {
        moveCustomer = GetComponent<moveCustomer>();        
        gWaitTImer.SetActive(false);
        timerNum = fWaitTime;
    }
    
    void Update()
    {
       gWaitTImer.transform.forward = -Camera.main.transform.forward;
        spwanCash();

       waitTimerUpdate();
    }
    //float x = 0.5f;
    void spwanCash()
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

        /*if (moveCustomer.startTrading && x > 0)
            x -= Time.deltaTime;
        if (moveCustomer.startTrading && x <= 0)
        {
            for(int i=0;i<= spwanCount; i++)
            {
                GameObject c = Instantiate(chips, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                c.GetComponent<controlChipsObjects>().section = section;
                if(i>= spwanCount)
                    x = delayTime;
            }                        
        }*/
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
                gWaitTImer.SetActive(false);
                
                moveCustomer.DestinationToExit = DestinationToExit;                
            });
            
        }
        
    }
}
