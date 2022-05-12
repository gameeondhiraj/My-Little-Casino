using System.Collections.Generic;
using UnityEngine;

namespace Casino.Control
{
    public class controlPlayerInventory : MonoBehaviour
    {
        public List<GameObject> Cart = new List<GameObject>();
        [SerializeField] private Vector3 StartPosition;

        public controlChipSpwanner seatChipController;

        public Transform cartTransform;
        public Collider cashCollector;
        public int MaxLimit;
        public int CurrentLimit;
        public float cartUpdateSpeed;



        public bool isSeatNearBy;
        private bool SellItemInVault;
        private Transform VaultPodition;
        private movePlayer movePlayer;
        private AudioManager AudioManager;

        void Start()
        {
            CurrentLimit = MaxLimit;
            movePlayer = GetComponent<movePlayer>();
            AudioManager = FindObjectOfType<AudioManager>();
        }
        private void Update()
        {
            //stackLearping();
            if (Cart.Count>0)
                SellCartItem();
            clear();
           // if (Cart.Count >= MaxLimit)cashCollector.isTrigger = true; else { cashCollector.isTrigger = false; }
        }

        void clear()
        {
            for (int i = 0; i <= Cart.Count - 1; i++)
            {
                if (Cart[i] == null)
                {
                    Cart.Remove(Cart[i]);
                }
            }
        }

        public float yOffsetIncrese = 0.12f;
        public float stifness = 30;

        
        void stackLearping()
        {
            float x = Mathf.Clamp(movePlayer.direction.magnitude, 0.5f, 1f);            
            if (Cart.Count > 1)
            {
                Vector3 previousGameObjectPos = Cart[0].transform.position;

                for (int i = 1; i < Cart.Count; i++)
                {
                    Cart[i].transform.position = Vector3.Lerp(Cart[i].transform.position, new Vector3(previousGameObjectPos.x, previousGameObjectPos.y + yOffsetIncrese, previousGameObjectPos.z), stifness * Time.fixedDeltaTime);
                    //Cart[i].transform.position = Vector3.MoveTowards(Cart[i].transform.position, new Vector3(previousGameObjectPos.x, previousGameObjectPos.y + yOffsetIncrese, previousGameObjectPos.z), stifness * Time.fixedDeltaTime);

                    previousGameObjectPos = Cart[i].transform.position;
                }
            }
        }
        public void ArrangeObjectInCart()
        {
            if (Cart.Count == 1)
            {
                Cart[0].transform.GetComponent<controlChipsObjects>().EndPosition = StartPosition;
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
        }

        float xT = 0.2f;
        public void onTouchChips(Collider col)
        {
            if (MaxLimit > 0)
            {
                if (xT > 0)
                    xT -= Time.deltaTime;
                if (xT <= 0)
                {
                    print(xT);
                    try
                    {
                        if (seatChipController && seatChipController.Cart.Count>0)
                        {
                            controlChipSpwanner c = seatChipController;

                            if(!Cart.Contains(c.Cart[c.Cart.Count - 1]))
                            {
                                Cart.Add(c.Cart[c.Cart.Count - 1]);
                                AudioManager.source.PlayOneShot(AudioManager.collectChip);
                            }                            
                            Cart[Cart.Count - 1].transform.parent = cartTransform;
                            
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







          /*  if (!col.GetComponent<controlChipsObjects>().isStacked)
            {
                col.transform.parent = cartTransform;
                col.transform.GetComponent<controlChipsObjects>().isMove = true;
                col.transform.GetComponent<controlChipsObjects>().isStacked = true;
                col.transform.GetComponent<controlChipsObjects>().isPlayerCollected = true;
                col.transform.GetComponent<Rigidbody>().isKinematic = true;
                col.transform.GetComponent<Collider>().isTrigger = true;
                col.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (!Cart.Contains(col.gameObject))
                {
                    Cart.Add(col.gameObject);
                    AudioManager.source.PlayOneShot(AudioManager.collectChip);
                }
                return;
            }*/
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
                    //clear();
                    Cart[Cart.Count - 1].transform.parent = null;
                    Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().EndPosition = VaultPodition.position;
                    //Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().moveSpeed += 35;
                    Cart[Cart.Count - 1].transform.GetComponent<controlChipsObjects>().isMove = true;
                    Cart.Remove(Cart[Cart.Count - 1]);

                    x = 0.01f;
                }
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
           /* if (other.gameObject.CompareTag("Chip") && !other.transform.GetComponent<controlChipsObjects>().isMove)
            {
                if (Cart.Count <= MaxLimit)
                {
                    onTouchChips(other);
                    //ArrangeObjectInCart();
                }
            }*/
        }
        private void OnTriggerEnter(Collider other)
        {

           /* if (other.gameObject.CompareTag("Chip") && !other.GetComponent<controlChipsObjects>().isMove)
            {
                if (Cart.Count <= MaxLimit)
                {
                    onTouchChips(other);
                    ArrangeObjectInCart();
                }
            }*/
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Vault") && movePlayer.direction.magnitude <= 0)
            {
                if (!SellItemInVault)
                {
                    SellItemInVault = true;
                    VaultPodition = other.transform;
                }
            }
            if (other.gameObject.CompareTag("Seat") && movePlayer.direction.magnitude <= 0)
            {
                if (!isSeatNearBy)
                {
                    isSeatNearBy = true;
                    seatChipController = other.GetComponentInParent<controlChipSpwanner>();                    
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
                    VaultPodition = null;
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
}