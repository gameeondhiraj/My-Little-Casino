using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpwan : MonoBehaviour
{
    public List<GameObject> AvailableSeats = new List<GameObject>();
    public GameObject Customer;
    public Transform spwanPosition;
    public coreSection section;

    public int SpwanCount = 1;
    public float delayBetweenSpwan = 0.2f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpwanCustomer();
    }
    float x = 0.2f;
    public void SpwanCustomer()
    {
        if (AvailableSeats.Count > 0 && x > 0)
            x -= Time.deltaTime;

        if (AvailableSeats.Count > 0 && x <= 0)
        {
            GameObject c = Instantiate(Customer, spwanPosition);
            c.transform.position = spwanPosition.position;
            c.GetComponent<moveCustomer>().DestinationToSeat = AvailableSeats[AvailableSeats.Count - 1].transform;
            c.GetComponent<controlCustomer>().section = section;
            c.GetComponent<controlCustomer>().DestinationToExit = spwanPosition;
            c.GetComponent<controlCustomer>().spwanCount = SpwanCount;
            AvailableSeats[AvailableSeats.Count - 1].GetComponent<controlSeat>().isOccupied = true;
            x = delayBetweenSpwan;
        }
    }
}
