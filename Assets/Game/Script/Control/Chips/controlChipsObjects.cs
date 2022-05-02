using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlChipsObjects : MonoBehaviour
{
    public coreSection section;
    public Vector3 EndPosition;
    public float moveSpeed = 5;
    public float force = 0.5f;

    public int chipCost;
   
    public int objectHeight;
    public bool isMove;
    public bool isDestroy;
    public bool isStacked;
    public bool isOccupied;

    public bool isPlayerCollected;
    public controlMoneyUI controlMoney;
    void Start()
    {


        if (!section.chips.Contains(this.gameObject))
            section.chips.Add(this.gameObject);

        controlMoney = FindObjectOfType<controlMoneyUI>();
        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(transform.forward * force * 0.75f, ForceMode.Impulse);
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
        if ( isMove && Vector3.Distance(transform.localPosition , EndPosition) < 0.3f)
        {
            transform.localPosition = EndPosition;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            GetComponent<Collider>().isTrigger = false;
            isMove = false;
        }
    }

    void getUI()
    {
        GameObject UI = controlMoney.CashUI[controlMoney.CashUI.Count - 1].gameObject;
        controlMoney.CashUI.Remove(controlMoney.CashUI[controlMoney.CashUI.Count - 1]);
        UI.SetActive(true);
        UI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        RectTransform RUI = UI.GetComponent<RectTransform>();
        LeanTween.move(RUI, new Vector3(0f, 0f, 0f), 0.5f).setOnComplete(() => {
            RUI.gameObject.SetActive(false);
            controlMoney.CashUI.Add(RUI);
            FindObjectOfType<GameManager>().maxCash += chipCost;
        });        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            /*GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;*/
            /*transform.rotation = Quaternion.Euler(0, 0, 0);*/
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vault"))
        {            
            Destroy(this.gameObject);
            if(isPlayerCollected)
                getUI();
            else
            {
                FindObjectOfType<GameManager>().maxCash += chipCost;
            }
        }
    }
}
