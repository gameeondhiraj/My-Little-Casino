using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class controlStaffSpwanner : MonoBehaviour
{
    public coreSection coreSection;
    public List<Transform> StaffPosition = new List<Transform>();
    public GameObject HireUI;
    public GameObject Staff;
    public Transform staffInventory;
    public Transform VaultPosition;
    public Button HireButton;

    public TextMeshProUGUI Amount;
    //public TextMeshProUGUI StaffCount;

    public int HireCount;
    public int maxHireCount = 4;
    public bool isLocked = true;
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
        if (!isLocked) openUI();
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
            staff.GetComponent<moveStaff>().TargetToInitPosition = StaffPosition[Random.Range(0, StaffPosition.Count - 1)];
            if (StaffPosition.Contains(staff.GetComponent<moveStaff>().TargetToInitPosition))
                StaffPosition.Remove(staff.GetComponent<moveStaff>().TargetToInitPosition);
            staff.GetComponent<moveStaff>().TargetToVault = VaultPosition;
            GameManager.maxCash -= GameManager.StaffHIreAmount;
            staff.GetComponent<controlStaff>().section = coreSection;
            HireCount++;
            GameManager.StaffHIreAmount += 120;
            onClickCloseButton();
        }
    }

    public void openUI()
    {
        if (GameManager.maxCash < GameManager.StaffHIreAmount) HireButton.interactable = false;
        if (HireCount > maxHireCount) HireButton.interactable = false;
        if (GameManager.maxCash >= GameManager.StaffHIreAmount && HireCount < maxHireCount) HireButton.interactable = true;


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
            {
                if (GameManager.Level >= 3) isPlayerNear = true;
            }
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
