using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlCustomer : MonoBehaviour
{
    [HideInInspector] public controlTable table;
    private moveCustomer moveCustomer;
    public coreSection section;
    public GameObject chips;
    public Transform DestinationToExit;
    public Transform ChipSpwanPosition;
    public int chipSpwanCount= 5;
    public float delayTime = 1;


    [Header("Hair Color")]
    public List<Color> hairColor = new List<Color>();
    public SkinnedMeshRenderer Renderer;

    [Header("Wait Timer")]
    [Space(10)]
    public GameObject gWaitTImer;
    public Image iWaitTimer;
    public float fWaitTime;
    private float timerNum;

    public bool bettingComplete;

    public bool isCustomerForTable;
    private AudioManager AudioManager;
    void Start()
    {
        moveCustomer = GetComponent<moveCustomer>();
        AudioManager = FindObjectOfType<AudioManager>();
        gWaitTImer.SetActive(false);
        timerNum = fWaitTime;

        if (Renderer)
        {
            Renderer.materials[0].color = hairColor[Random.Range(0, hairColor.Count - 1)];

            if (Renderer.materials.Length >= 4)
                Renderer.materials[4].color = hairColor[Random.Range(0, hairColor.Count - 1)];
            if (Renderer.materials.Length >= 5)
                Renderer.materials[5].color = hairColor[Random.Range(0, hairColor.Count - 1)];
        }
       

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
            for (int i=0;i<= chipSpwanCount; i++)
            {
                GameObject c = Instantiate(chips, ChipSpwanPosition.position, Quaternion.identity);
                c.GetComponent<controlChipsObjects>().section = section;
                if (i >= chipSpwanCount - 1)
                {
                    if (!bettingComplete)
                    {
                        FindObjectOfType<GameManager>().CustomerCount++;
                        AudioManager.source.PlayOneShot(AudioManager.chipDespensed);
                        bettingComplete = true;
                    }                    
                }
            }
           
        }
    }

    void waitTimerUpdate()
    {
        if(moveCustomer.startTrading && timerNum > 0 )
        {
            StartCoroutine(WaitTimerUI(1.5f));
        }
        if (bettingComplete && !moveCustomer.DestinationToExit)
        {
            StartCoroutine(attachExitDestination(2.5f));            
        }        
    }

    IEnumerator WaitTimerUI(float t)
    {
        yield return new WaitForSeconds(t);
        gWaitTImer.SetActive(true);
        timerNum -= Time.deltaTime;
        iWaitTimer.fillAmount = timerNum / fWaitTime;
        if(timerNum<=0 && gWaitTImer.activeSelf)
            gWaitTImer.SetActive(false);

    }

    IEnumerator attachExitDestination(float t)
    {
        if (gWaitTImer.activeSelf)
            gWaitTImer.SetActive(false);
        moveCustomer.startTrading = false;
        yield return new WaitForSeconds(t);
        moveCustomer.DestinationToExit = DestinationToExit;
    }
}
