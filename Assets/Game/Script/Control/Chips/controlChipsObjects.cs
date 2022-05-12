using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlChipsObjects : MonoBehaviour
{
    public coreSection section;
    public Vector3 EndPosition;
    public float moveSpeed = 5;
    public float force = 7.5f;

    public int chipCost;

    public int objectHeight;
    public bool isMove;
    public bool isDestroy;
    public bool isStacked;
    public bool isOccupied;

    public bool isPlayerCollected;
    public controlMoneyUI controlMoney;
    private AudioManager AudioManager;
    void Start()
    {

        AudioManager = FindObjectOfType<AudioManager>();
        if (!section.chips.Contains(this.gameObject))
            section.chips.Add(this.gameObject);

        controlMoney = FindObjectOfType<controlMoneyUI>();

        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f * force, 1f * force), Random.Range(-1f * force, 1f * force) * 3, Random.Range(-1f * force, 1f * force)), ForceMode.Impulse);
        LeanTween.delayedCall(1.5f, () => {
            GetComponent<Rigidbody>().isKinematic = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        moveObject();
        if (isDestroy)
            Destroy(this.gameObject, 0.1f);

        if (isStacked && section.chips.Contains(this.gameObject))
        {
            section.chips.Remove(this.gameObject);
            isStacked = false;
        }

        if (isOccupied) section.chips.Remove(this.gameObject);
    }
    public void moveObject()
    {
        if (isMove)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, EndPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 20 * Time.deltaTime);

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().isTrigger = true;

        }
        if (isMove && Vector3.Distance(transform.localPosition, EndPosition) < 0.3f)
        {
            transform.localPosition = EndPosition;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            GetComponent<Collider>().isTrigger = false;
            isMove = false;
        }
    }

    void getUI()
    {
        AudioManager.source.PlayOneShot(AudioManager.chipRoCash);
        GameObject UI = controlMoney.CashUI[controlMoney.CashUI.Count - 1].gameObject;
        controlMoney.CashUI.Remove(controlMoney.CashUI[controlMoney.CashUI.Count - 1]);
        UI.SetActive(true);
        UI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        RectTransform RUI = UI.GetComponent<RectTransform>();

        LeanTween.moveLocal(UI, controlMoney.cashPosition.localPosition, 0.5f).setOnComplete(() =>
        {
            RUI.gameObject.SetActive(false);
            controlMoney.CashUI.Add(RUI);
            FindObjectOfType<GameManager>().maxCash += chipCost;
        });
    }

/*    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            LeanTween.delayedCall(0.5f,() => {
                GetComponent<Rigidbody>().isKinematic = true;
            });
            
            //GetComponent<Collider>().isTrigger = true;
            *//*transform.rotation = Quaternion.Euler(0, 0, 0);*//*
        }
    }*/


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vault"))
        {
            Destroy(this.gameObject);

            if (isPlayerCollected)
                getUI();
            else
            {
                FindObjectOfType<GameManager>().maxCash += chipCost;
            }
        }
    }
}
