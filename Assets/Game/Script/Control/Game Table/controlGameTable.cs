using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class controlGameTable : MonoBehaviour
{
    public bool isPlayerHere;
    public GameObject GameTableUI;
    public GameObject Table;
    public GameObject SpwanParticalEffect;
    public Transform baseT;

    [Header("Prefab Object")]
    [Space(5)]
    public Transform SpwanPosition;
    public Transform DestinationForExit;
    public coreSection Section;

    [Header("List of Table")]
    public GameObject BLACKJACK;
    public GameObject ROULETTE;
    public GameObject CRAPS;

    public string whichTableIsIt;
    private bool isClickedOnCloseButton;
    private GameManager GameManager;

    [Header("Buttons")]
    public Button Black;
    public Button Roul;
    public Button Craps;

    private float BCost = 500;
    public TextMeshProUGUI BJText;
    private float RCost = 1000;
    public TextMeshProUGUI RLText;
    private float CCost = 1500;
    public TextMeshProUGUI CPText;


    public GameObject Border;
    private AudioManager AudioManager;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        if (whichTableIsIt.Length > 0)
        {
            //Spwan Table from GameManager
        }
        if (!GameManager.BJ) Roul.interactable = false;
        if (!GameManager.RL) Craps.interactable = false;
    }

    
    void Update()
    {
        CheckForTabel();
        buttonCheck();
        BJText.text = BCost.ToString("N0");
        RLText.text = RCost.ToString("N0");
        CPText.text = CCost.ToString("N0");

    }

    void buttonCheck()
    {
        if (GameManager.maxCash < BCost) Black.interactable = false; else { Black.interactable = true; }
        if (GameManager.maxCash < RCost) Roul.interactable = false; else if(GameManager.BJ && GameManager.maxCash >= RCost) { Roul.interactable = true; }
        if (GameManager.maxCash < CCost) Craps.interactable = false; else if(GameManager.RL && GameManager.maxCash >= CCost) { Craps.interactable = true; }

    }
    void CheckForTabel()
    {
        if (!Table)
        {
            foreach(Transform obj in baseT)
            {
                Table = obj.gameObject;
                whichTableIsIt = Table.name;
            }
        }
    }

    public void sBLACKJACK()
    {
        if(GameManager.maxCash>= BCost  && !GameManager.BJ)
        {
            GameObject BJ = Instantiate(BLACKJACK, baseT);
            BJ.transform.localPosition = new Vector3(0, 0, 0);
            BJ.GetComponent<CustomerSpwan>().spwanPosition = SpwanPosition;
            BJ.GetComponent<CustomerSpwan>().section = Section;
            BJ.GetComponent<CustomerSpwan>().DestinationForExit = DestinationForExit;
            Destroy(Instantiate(SpwanParticalEffect, BJ.transform.position + new Vector3(-1.5f, 0.5f, 0), Quaternion.identity), 3);
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
            AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
            GameManager.tableCount++;
            GameManager.BJ = true;
            Roul.interactable = true;
            GameManager.maxCash -= BCost;
            Border.SetActive(false);
        }
        
    }
    public void sROULETTE()
    {
        if (GameManager.maxCash >= RCost && GameManager.BJ)
        {
            GameObject RL = Instantiate(ROULETTE, baseT);
            RL.transform.localPosition = new Vector3(0, 0, 0);
            RL.GetComponent<CustomerSpwan>().spwanPosition = SpwanPosition;
            RL.GetComponent<CustomerSpwan>().section = Section;
            RL.GetComponent<CustomerSpwan>().DestinationForExit = DestinationForExit;
            Destroy(Instantiate(SpwanParticalEffect, RL.transform.position + new Vector3(-1.5f, 0.5f,0), Quaternion.identity), 3);
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
            AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
            GameManager.tableCount++;
            GameManager.RL = true;
            Craps.interactable = true;
            GameManager.maxCash -= RCost;
            Border.SetActive(false);
        }
            
    }
    public void sCRAPS()
    {
        if (GameManager.maxCash >= CCost && GameManager.RL)
        {
            GameObject CP = Instantiate(CRAPS, baseT);
            CP.transform.localPosition = new Vector3(0, 0, 0);
            CP.GetComponent<CustomerSpwan>().spwanPosition = SpwanPosition;
            CP.GetComponent<CustomerSpwan>().DestinationForExit = DestinationForExit;
            CP.GetComponent<CustomerSpwan>().section = Section;
            Destroy(Instantiate(SpwanParticalEffect, CP.transform.position + new Vector3(-1.5f, 0.5f, 0), Quaternion.identity), 3);
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
            AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
            GameManager.tableCount++;
            GameManager.CP = true;
            GameManager.maxCash -= CCost;
            Border.SetActive(false);
        }            
    }

    public void closeGameTableUI()
    {
        isClickedOnCloseButton = true;
        if (GameTableUI.activeSelf)
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isClickedOnCloseButton && other.gameObject.CompareTag("Player") && other.GetComponentInParent<movePlayer>().direction.magnitude < 0.1f)
        {
            isPlayerHere = true;
            if (!Table) GameTableUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isPlayerHere)
            isPlayerHere = false;

        if (GameTableUI.activeSelf)
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");

        if (isClickedOnCloseButton)
            isClickedOnCloseButton = false;
    }
}
