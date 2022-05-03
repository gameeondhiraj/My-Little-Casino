using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlCustomerAnimation : MonoBehaviour
{
    public Animator anime;

    private moveCustomer move;
    private controlCustomer control;
    void Start()
    {
        move = GetComponent<moveCustomer>();
        control = GetComponent<controlCustomer>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationManager();
    }

    void AnimationManager()
    {
        anime.SetFloat("speed", move.agent.velocity.magnitude);
        anime.SetBool("sit", move.startTrading);
        anime.SetBool("win", control.bettingComplete);
    }
}
