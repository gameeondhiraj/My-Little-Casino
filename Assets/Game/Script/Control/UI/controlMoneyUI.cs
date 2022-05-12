using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlMoneyUI : MonoBehaviour
{
    public List<Transform> CashUI = new List<Transform>();
    public Transform cashPosition;
    public Transform SlotMachinePosition;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in CashUI)
        {
            if (t.gameObject.activeSelf)
                t.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetFalseEachObjectInList();
    }
    void SetFalseEachObjectInList()
    {
        foreach (Transform t in CashUI)
        {
            if (t.gameObject.activeSelf)
                t.gameObject.SetActive(false);
        }
    }
}
