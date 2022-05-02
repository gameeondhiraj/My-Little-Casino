using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class controlStaffSpwanner : MonoBehaviour
{
    public List<Transform> StaffPosition = new List<Transform>();
    public GameObject HireUI;
    public GameObject Staff;
    public Transform staffInventory;
    public Button HireButton;

    public TextMeshProUGUI Amount;
    //public TextMeshProUGUI StaffCount;

    public int HireCount;
    public int maxHireCount = 4;
    public bool isPlayerNear;
    public bool isClickedOnCloseButton;
    private GameManager GameManager;

    void Start()
    {
        HireUI.SetActive(false);
        GameManager = FindObjectOfType<GameManager>();
        if (HireCount != 0)
            hire();
    }

    // Update is called once per frame
    void Update()
    {
        Amount.text = GameManager.StaffHIreAmount.ToString("N0");

        if (Input.GetKeyDown(KeyCode.P))
            Hire();

        openUI();
    }


    void hire() { 
    }

    public void onClickCloseButton()
    {
        if (HireUI.activeSelf)
            HireUI.GetComponent<Animator>().Play("Game Table Exit");
        isClickedOnCloseButton = true;
    }
    public void Hire()
    {
        if(HireCount < maxHireCount && GameManager.maxCash>= GameManager.StaffHIreAmount)
        {
            GameObject staff = Instantiate(Staff, staffInventory.position, Quaternion.identity, staffInventory);
            staff.GetComponent<controlStaff>().initialPosition = StaffPosition[Random.Range(0, StaffPosition.Count - 1)];
            if (StaffPosition.Contains(staff.GetComponent<controlStaff>().initialPosition))
                StaffPosition.Remove(staff.GetComponent<controlStaff>().initialPosition);
            GameManager.maxCash -= GameManager.StaffHIreAmount;
            HireCount++;
            GameManager.StaffHIreAmount += 150;
        }
    }

    public void openUI()
    {
        if (GameManager.maxCash < GameManager.StaffHIreAmount) HireButton.interactable = false;
        if (GameManager.maxCash >= GameManager.StaffHIreAmount) HireButton.interactable = true;


        if (isPlayerNear && !isClickedOnCloseButton)
            HireUI.SetActive(true);

        if (HireUI.activeSelf && !isPlayerNear)
            HireUI.GetComponent<Animator>().Play("Game Table Exit");
    }
    private void OnTriggerStay(Collider other)
    {
        try
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<movePlayer>().direction.magnitude < 0.1f)
                isPlayerNear = true;
        }
        catch
        {

        }
    
    }
    private void OnTriggerExit(Collider other)
    {
        if (isPlayerNear)
            isPlayerNear = false;
        if (isClickedOnCloseButton)
            isClickedOnCloseButton = false;
    }
}
