using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModePlugin : MonoBehaviour
{
    public bool isTest;
    public CustomerSpwan CustomerSpwan;

    [Header("Test")]
    public float Cash;
    public float ChipSpwanCount;
    public int Level;   



    private GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        if (isTest) setTestMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setTestMode()
    {
        GameManager.maxCash = Cash;
        CustomerSpwan.chipSpwanCount = (int)ChipSpwanCount;
        GameManager.Level = Level;
    }
}
