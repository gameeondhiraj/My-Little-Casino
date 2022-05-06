using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class controlStaffDeskUnlocker : MonoBehaviour
{
    [Header("Locked")]
    [Space(10)]
    public GameObject LockedObject;
    public GameObject unlockPartical;
    public TextMeshProUGUI cashCounter;
    public float unlockingAmount;
    public int reduceAmountSpeed = 10;

    [Header("Unlocked")]
    [Space(10)]
    public GameObject UnlockedObject;

    [Header("Progress")]
    [Space(5)]
    public bool isPlayerNear;
    public bool isStaffDeskOne;

    [Header("Cinemachine")]
    [Space(10)]
    public Animator cinemachineCamera;
    public string sCamera;
    public float transitionDelay = 1f;


    private GameManager GameManager;
    private AudioManager AudioManager;
    private controlStaffSpwanner controlStaffDesk;

    void Start()
    {
        controlStaffDesk = GetComponent<controlStaffSpwanner>();
        GameManager = FindObjectOfType<GameManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        unlocked();
    }
    void unlocked()
    {
        if (!controlStaffDesk.isLocked || unlockingAmount <= 0)
        {
            UnlockedObject.SetActive(true);
            LockedObject.SetActive(false);
        }
    }

    void Update()
    {
        if(!isStaffDeskOne)
            moneyManagement();
        if (isStaffDeskOne)
            unlock();
    }

   void unlock()
    {
        cashCounter.text = "Reach level 4 to unlock";
        if (controlStaffDesk.isLocked && unlockingAmount <= 0)
        {
            StartCoroutine(unlockDesk(transitionDelay));
        }
        if(!controlStaffDesk.isLocked && unlockingAmount <= 0 && !UnlockedObject.activeSelf)
        {
            LockedObject.SetActive(false);
            UnlockedObject.SetActive(true);
        }

        if (GameManager.Level >= 3)
            unlockingAmount = 0;
    }

    IEnumerator unlockDesk(float t)
    {
        cinemachineCamera.Play(sCamera);
        yield return new WaitForSeconds(t);        
        LockedObject.SetActive(false);        
        if (!UnlockedObject.activeSelf)
        {
            Destroy(Instantiate(unlockPartical, transform.position, Quaternion.identity), 5);
            AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
            UnlockedObject.SetActive(true);
        }
               

        controlStaffDesk.isLocked = false;
    }
    void moneyManagement()
    {
        cashCounter.text = unlockingAmount.ToString("N0");
        if (controlStaffDesk.isLocked && unlockingAmount <= 0)
        {
            Destroy(Instantiate(unlockPartical, transform.position, Quaternion.identity), 5);
            LockedObject.SetActive(false);
            UnlockedObject.SetActive(true);
            AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
            controlStaffDesk.isLocked = false;
        }

        if (controlStaffDesk.isLocked && isPlayerNear)
            moneyReducer();
    }
    void moneyReducer()
    {
        if (unlockingAmount > 0 && GameManager.maxCash > 0)
        {
            unlockingAmount -= reduceAmountSpeed * Time.deltaTime;
            GameManager.maxCash -= reduceAmountSpeed * Time.deltaTime;
        }
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
    }

}
