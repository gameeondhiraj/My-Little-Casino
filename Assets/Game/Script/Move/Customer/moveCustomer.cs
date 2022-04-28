using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveCustomer : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform DestinationToSeat;
    public Transform DestinationToExit;

    public bool isMoving;
    public bool startTrading;
    public bool callNewCustomer;
    private controlCustomer controlCustomer;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controlCustomer = GetComponent<controlCustomer>();
    }


    void Update()
    {
        moveAgent();
    }

    public void moveAgent()
    {
        if (DestinationToSeat && !DestinationToExit)
        {
            agent.SetDestination(DestinationToSeat.position);
            if (Vector3.Distance(transform.position, DestinationToSeat.position) <= 1f && !isMoving)
            {
                agent.isStopped = true;
                startTrading = true;
            }
            if (agent.velocity.magnitude > 0.1) isMoving = true; else { isMoving = false; }
        }

        if (DestinationToExit)
        {
            if(!callNewCustomer)
            {
                LeanTween.delayedCall(1f, () =>
                {                                     
                    callNewCustomer = true;
                });
            }
            if(callNewCustomer && DestinationToSeat != null)
            {
                DestinationToSeat.GetComponent<controlSeat>().isOccupied = false;
                DestinationToSeat = null;
            }
                

            agent.isStopped = false;
            agent.SetDestination(DestinationToExit.position);
            if (Vector3.Distance(transform.position, DestinationToExit.position) <= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
