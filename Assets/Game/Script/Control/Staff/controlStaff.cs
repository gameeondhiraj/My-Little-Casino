using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class controlStaff : MonoBehaviour
{
    public coreSection section;
    public List<GameObject> Cart = new List<GameObject>();
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Collider Collector;
    public Transform Inventory;
    public controlChipSpwanner seatChipController;

    public float cartUpdateSpeed = 0.4f;
    public bool isSeatNearBy;
    public bool SellItemInVault;
    public int maxLimit;
    private moveStaff moveStaff;
    void Start()
    {
        moveStaff = GetComponent<moveStaff>();
    }



    // Update is called once per frame
    void Update()
    {
        //stackLearping();
        //if (Cart.Count >= maxLimit) Collector.isTrigger = true; else { Collector.isTrigger = false; }
        if (Cart.Count > 0)
        {
            clear();
            SellCartItem();
        }
    }

    public float yOffsetIncrese = 0.12f;
    public float stifness = 30;
    void stackLearping()
    {
        if (Cart.Count > 1)
        {
            Vector3 previousGameObjectPos = Cart[0].transform.position;

            for (int i = 1; i < Cart.Count; i++)
            {
                Cart[i].transform.position = Vector3.Lerp(Cart[i].transform.position, new Vector3(previousGameObjectPos.x, previousGameObjectPos.y + yOffsetIncrese, previousGameObjectPos.z), stifness * Time.fixedDeltaTime);                
                previousGameObjectPos = Cart[i].transform.position;
            }
        }
    }
    void clear()
    {
        //Cart = Cart.Where(item => item != null).ToList();
        for (int i = 0; i <= Cart.Count - 1; i++)
        {
            if (Cart[i] == null)
            {
                Cart.Remove(Cart[i]);
            }
            if (Cart[i] != null)
            {
                Cart[i].GetComponent<controlChipsObjects>().isStacked = true;
            }
        }
    }

    float x = 0.01f;
    public void SellCartItem()
    {
        if (SellItemInVault && Cart.Count > 0)
        {
            if (x > 0)
                x -= Time.deltaTime;
            if (x <= 0)
            {
                clear();
                Cart[Cart.Count - 1].transform.parent = null;
                Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().EndPosition = moveStaff.TargetToVault.position;
                //Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().moveSpeed += 35;
                Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isMove = true;
                Cart.Remove(Cart[Cart.Count - 1]);
                x = 0.01f;
            }
        }
        if (SellItemInVault && Cart.Count <= 0)
            SellItemInVault = false;
    }
    public void ArrangeObjectInCart()
    {
        if (Cart.Count == 1)
        {
            Cart[0].transform.GetComponent<controlChipsObjects>().EndPosition = startPosition;
            Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isPlayerCollected = true;
            Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isStacked = true;
            Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isMove = true;
            return;
        }
        if (Cart.Count > 1)
        {
            Cart[Cart.Count - 1].GetComponent<controlChipsObjects>().EndPosition =
                new Vector3(Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.x,
                Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.y + 0.12f,
                Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.z);
            Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isPlayerCollected = true;
            Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isStacked = true;
            Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isMove = true;

            return;
        }

        /*  if (Cart.Count == 1)
          {
              Cart[0].transform.GetComponent<controlChipsObjects>().EndPosition = startPosition;
              return;
          }*//*
           if (Cart.Count > 0)
           {
               Cart[Cart.Count - 1].GetComponent<controlChipsObjects>().EndPosition =
                   new Vector3(Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.x,
                   Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.y + 0.12f,
                   Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.z);
               return;
           }*/
    }

    float xT = 0.2f;
    public void onTouchChips(Collider col)
    {
        if (maxLimit > 0)
        {
            if (xT > 0)
                xT -= Time.deltaTime;
            if (xT <= 0)
            {
                print(xT);
                try
                {
                    if (seatChipController && seatChipController.Cart.Count > 0)
                    {
                        controlChipSpwanner c = seatChipController;

                        if (!Cart.Contains(c.Cart[c.Cart.Count - 1]))
                        {
                            Cart.Add(c.Cart[c.Cart.Count - 1]);                            
                        }
                        Cart[Cart.Count - 1].transform.parent = Inventory;

                        c.Cart.Remove(c.Cart[c.Cart.Count - 1]);

                        Cart[Cart.Count - 1].transform.rotation = Quaternion.Euler(0, 0, 0);
                        x = cartUpdateSpeed;
                        return;
                    }
                    {
                        print("Seat Controller Missing !");
                    }
                }
                catch
                {
                    print("ERROR !");
                }
            }
        }

        /*
                if (!col.transform.GetComponent<controlChipsObjects>().isStacked)
                {
                    col.transform.GetComponent<controlChipsObjects>().isStacked = true;
                    col.transform.parent = Inventory;
                    col.transform.GetComponent<controlChipsObjects>().isMove = true;
                    col.transform.GetComponent<Rigidbody>().isKinematic = true;
                    col.transform.GetComponent<Collider>().isTrigger = true;
                    col.transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (!Cart.Contains(col.gameObject))
                    {
                        Cart.Add(col.gameObject);
                    }
                    return;
                }*/
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chip") && !other.transform.GetComponent<controlChipsObjects>().isMove)
        {
            if (Cart.Count <= maxLimit)
            {
                onTouchChips(other);
                ArrangeObjectInCart();
            }
        }
    }*/


    
    private void OnCollisionEnter(Collision other)
    {

      

        /* if (other.gameObject.CompareTag("Chip") && !other.transform.GetComponent<controlChipsObjects>().isMove)
         {
             if (Cart.Count <= maxLimit)
             {
                 onTouchChips(other);
                 ArrangeObjectInCart();
             }
         }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Vault") && moveStaff.agent.velocity.magnitude <= 0 && Cart.Count > 0)
        {
            if (!SellItemInVault)
            {
                SellItemInVault = true;
            }
        }
        if (other.gameObject.CompareTag("Seat") && moveStaff.agent.velocity.magnitude <= 0)
        {
            if (!isSeatNearBy)
            {
                isSeatNearBy = true;
                seatChipController = other.transform.GetComponentInParent<controlChipSpwanner>();
            }
            onTouchChips(other);
            ArrangeObjectInCart();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Vault"))
        {
            if (SellItemInVault)
            {
                SellItemInVault = false;
            }
        }
        if (other.gameObject.CompareTag("Seat"))
        {
            if (isSeatNearBy)
            {
                isSeatNearBy = false;
                seatChipController = null;
            }
        }
    }
}
