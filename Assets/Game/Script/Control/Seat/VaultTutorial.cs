using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultTutorial : MonoBehaviour
{
    private Casino.Control.controlPlayerInventory playerInventory;
    private GameManager GameManager;

    public GameObject Arrow;
    public Animator cinemachineCamera;
    public string sCamera;
    private bool arrowShown;
    void Start()
    {
        playerInventory = GetComponent<Casino.Control.controlPlayerInventory>();
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowArrow();
    }
    void ShowArrow()
    {
        if (GameManager.Level <= 0)
        {
            if (!arrowShown && playerInventory.Cart.Count >= 1)
            {
                cinemachineCamera.Play(sCamera);
                Arrow.SetActive(true);
                arrowShown = true;
            }
            if (Arrow.activeSelf && playerInventory.Cart.Count <= 0)
            {
                Arrow.SetActive(false);
            }
        }
    }
    
}
