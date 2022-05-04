using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveCustomer : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform DestinationToSeat;
    public Transform DestinationToExit;
    public float rotationFace = 90;
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
        if (startTrading && DestinationToSeat)
        {
            rotationFace = DestinationToSeat.GetComponent<controlSeat>().rotationFace;
            LeanTween.rotate(this.gameObject, new Vector3(0, rotationFace, 0), 0.3f);
            startTrading = false;
        }

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
                if (!controlCustomer.isCustomerForTable) StartCoroutine(NewCustomerCall(1f));
                if (controlCustomer.isCustomerForTable) StartCoroutine(NewCustomerCall(6f));
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
                try
                {
                    if (controlCustomer.table.Customer.Contains(this.gameObject))
                        controlCustomer.table.Customer.Remove(this.gameObject);
                    Destroy(gameObject, 0.1f);
                }
                catch
                {
                    Destroy(gameObject);
                }
                
            }
        }
    }

    IEnumerator NewCustomerCall(float t)
    {
        yield return new WaitForSeconds(t);
        callNewCustomer = true;
    }
}
