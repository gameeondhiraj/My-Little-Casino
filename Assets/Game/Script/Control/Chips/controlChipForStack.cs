using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlChipForStack : MonoBehaviour
{
    private controlChipsObjects controlChips;
    public controlChipSpwanner seatChipController;
    public Transform cartPosition;
    // Start is called before the first frame update
    void Start()
    {
        controlChips = GetComponent<controlChipsObjects>();
        StartCoroutine(delayCall(0.5f));
        
    }

    IEnumerator delayCall(float t)
    {
        yield return new WaitForSeconds(t);
        onTouch();
    }

    // Update is called once per frame
    void Update()
    {
        if (seatChipController && !cartPosition) cartPosition = seatChipController.cartPosition;
    }


    void onTouch()
    {
        if (!seatChipController.Cart.Contains(this.gameObject)) seatChipController.Cart.Add(this.gameObject);
        
        transform.parent = seatChipController.cartPosition;
        controlChips.isMove = true;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Collider>().isTrigger = true;
        seatChipController.ArrangeObjectInCart();
        return;
    }
}
