using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casino.Control
{
    public class controlPlayerAnimation : MonoBehaviour
    {
        public Animator Animation;

        private movePlayer movePlayer;
        private controlPlayerInventory playerInventory;

        void Start()
        {
            movePlayer = GetComponent<movePlayer>();
            playerInventory = GetComponent<controlPlayerInventory>();
        }


        void Update()
        {
            characterAnimation();
        }

        void characterAnimation()
        {
            Animation.SetFloat("speed", movePlayer.direction.magnitude);
            if (playerInventory.Cart.Count > 0)
            {
                Animation.SetBool("hold", true);
            }
            else
            {
                Animation.SetBool("hold", false);
            }
        }
    }
}
