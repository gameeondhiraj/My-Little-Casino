using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlChipSpwanner : MonoBehaviour
{

    public List<GameObject> Cart = new List<GameObject>();
    public Transform cartPosition;

    public void ArrangeObjectInCart()
    {
        if (Cart.Count >= 1)
        {
            Cart[Cart.Count - 1].GetComponent<controlChipsObjects>().EndPosition =
                new Vector3(Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.x,
                Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.y + 0.12f,
                Cart[Cart.Count - 2].GetComponent<controlChipsObjects>().EndPosition.z);
            return;
        }
    }
}
