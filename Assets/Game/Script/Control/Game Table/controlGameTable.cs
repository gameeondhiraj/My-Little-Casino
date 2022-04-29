using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public coreSection Section;

    [Header("List of Table")]
    public GameObject BLACKJACK;
    public GameObject ROULETTE;
    public GameObject CRAPS;

    public string whichTableIsIt;

    private GameManager GameManager;


    private float BCost = 500;
    public TextMeshProUGUI BJText;
    private float RCost = 1000;
    public TextMeshProUGUI RLText;
    private float CCost = 1500;
    public TextMeshProUGUI CPText;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();

        if (whichTableIsIt.Length > 0)
        {
            //Spwan Table from GameManager
        }
    }

    
    void Update()
    {
        CheckForTabel();
        BJText.text = BCost.ToString("N0");
        RLText.text = RCost.ToString("N0");
        CPText.text = CCost.ToString("N0");
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
            Destroy(Instantiate(SpwanParticalEffect, BJ.transform.position + new Vector3(-1.5f, 0.5f, 0), Quaternion.identity), 3);
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
            GameManager.BJ = true;
            GameManager.maxCash -= BCost;
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
            Destroy(Instantiate(SpwanParticalEffect, RL.transform.position + new Vector3(-1.5f, 0.5f,0), Quaternion.identity), 3);
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
            GameManager.RL = true;
            GameManager.maxCash -= RCost;
        }
            
    }
    public void sCRAPS()
    {
        if (GameManager.maxCash >= CCost && GameManager.RL)
        {
            GameObject CP = Instantiate(CRAPS, baseT);
            CP.transform.localPosition = new Vector3(0, 0, 0);
            CP.GetComponent<CustomerSpwan>().spwanPosition = SpwanPosition;
            CP.GetComponent<CustomerSpwan>().section = Section;
            Destroy(Instantiate(SpwanParticalEffect, CP.transform.position + new Vector3(-1.5f, 0.5f, 0), Quaternion.identity), 3);
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
            GameManager.CP = true;
            GameManager.maxCash -= CCost;
        }            
    }

    public void closeGameTableUI()
    {
        if (GameTableUI.activeSelf)
            GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && other.GetComponentInParent<movePlayer>().direction.magnitude < 0.1f)
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
    }
}
