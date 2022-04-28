using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlGameTable : MonoBehaviour
{
    public bool isPlayerHere;
    public GameObject GameTableUI;
    public GameObject Table;
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

    
    void Start()
    {
        if (whichTableIsIt.Length > 0)
        {
            //Spwan Table from GameManager
        }
    }

    
    void Update()
    {
        CheckForTabel();
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
        GameObject BJ = Instantiate(BLACKJACK, baseT);
        BJ.transform.position = new Vector3(0, 0, 0);
        BJ.GetComponent<CustomerSpwan>().spwanPosition = SpwanPosition;
        BJ.GetComponent<CustomerSpwan>().section = Section;
        GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
    }
    public void sROULETTE()
    {
        GameObject RL = Instantiate(ROULETTE, baseT);
        RL.transform.position = new Vector3(0, 0, 0);
        RL.GetComponent<CustomerSpwan>().spwanPosition = SpwanPosition;
        RL.GetComponent<CustomerSpwan>().section = Section;
        GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
    }
    public void sCRAPS()
    {
        GameObject CP = Instantiate(CRAPS, baseT);
        CP.transform.position = new Vector3(0, 0, 0);
        CP.GetComponent<CustomerSpwan>().spwanPosition = SpwanPosition;
        CP.GetComponent<CustomerSpwan>().section = Section;
        GameTableUI.GetComponent<Animator>().Play("Game Table Exit");
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
