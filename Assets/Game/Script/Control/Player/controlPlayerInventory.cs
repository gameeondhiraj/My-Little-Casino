using System.Collections.Generic;
using UnityEngine;

namespace Casino.Control
{
    public class controlPlayerInventory : MonoBehaviour
    {
        public List<GameObject> Cart = new List<GameObject>();
        [SerializeField] private Vector3 StartPosition;
        public Transform cartTransform;
        public Collider cashCollector;

        public int MaxLimit;
        public int CurrentLimit;
        public float cartUpdateSpeed;


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
            if(Cart.Count>0)
                SellCartItem();
            stackLearping();

            if (Cart.Count >= MaxLimit)cashCollector.isTrigger = true; else { cashCollector.isTrigger = false; }
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
                    Cart[i].transform.position = Vector3.Lerp(Cart[i].transform.position, new Vector3(previousGameObjectPos.x, previousGameObjectPos.y + yOffsetIncrese, previousGameObjectPos.z), (stifness * Time.fixedDeltaTime) * x);
                    previousGameObjectPos = Cart[i].transform.position;
                }
            }
        }
        public void ArrangeObjectInCart()
        {
            if (Cart.Count == 1)
            {
                Cart[0].transform.GetComponent<controlChipsObjects>().EndPosition = StartPosition;
                return;
            }
           /* if (Cart.Count > 1)
            {
                Cart[Cart.Count - 1].GetComponent<controlChipsObjects>().EndPosition =
                    new Vector3(Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.x,
                    Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.y + 0.12f,
                    Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.z);
                return;
            }*/
        }
        public void onTouchChips(Collision col)
        {
            if (Cart.Count < 1)
            {
                col.transform.parent = cartTransform;
                col.transform.GetComponent<controlChipsObjects>().isMove = true;
            }
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

        }

        float x = 0.01f;
        public void SellCartItem()
        {
            if (SellItemInVault && Cart.Count > 1)
            {
                if (x > 0)
                    x -= Time.deltaTime;
                if (x <= 0)
                {
                    clear();
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
            if (other.gameObject.CompareTag("Chip") && !other.transform.GetComponent<controlChipsObjects>().isMove)
            {
                if (Cart.Count <= MaxLimit)
                {
                    onTouchChips(other);
                    //ArrangeObjectInCart();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
/*
            if (other.gameObject.CompareTag("Chip") && !other.GetComponent<controlChipsObjects>().isMove)
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
        }
    }
}