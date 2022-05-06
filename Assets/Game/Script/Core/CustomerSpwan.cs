using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpwan : MonoBehaviour
{
    public List<GameObject> AvailableSeats = new List<GameObject>();
    public GameObject[] Customer;
    public GameObject VIPCustomer;
    public Transform spwanPosition;
    public Transform DestinationForExit;
    public coreSection section;

    public int chipSpwanCount = 1;
    public int VIPChipSpwanCount = 1;
    public float delayBetweenChipSpwan = 0.2f;
    public float slotMachineChipSpwanWaitTimer = 1;
    public bool isTable;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTable)
        SpwanCustomer();

        if (isTable)
            SpwanCustomerForTable();
    }
    float x = 0.2f;
    float y = 2.5f;
    public void SpwanCustomer()
    {
        if (AvailableSeats.Count > 0 && x > 0)
            x -= Time.deltaTime;

        if (y>0 && AvailableSeats.Count > 0 && x <= 0)
        {
            GameObject c = Instantiate(Customer[Random.Range(0, Customer.Length)], spwanPosition);
            c.transform.position = spwanPosition.position;
            c.GetComponent<moveCustomer>().DestinationToSeat = AvailableSeats[AvailableSeats.Count - 1].transform;
            AvailableSeats[AvailableSeats.Count - 1].GetComponent<controlSeat>().isOccupied = true;
            if (AvailableSeats.Contains(c.GetComponent<moveCustomer>().DestinationToSeat.gameObject))
                AvailableSeats.Remove(c.GetComponent<moveCustomer>().DestinationToSeat.gameObject);
            c.GetComponent<controlCustomer>().section = section;
            c.GetComponent<controlCustomer>().DestinationToExit = DestinationForExit;
            c.GetComponent<controlCustomer>().chipSpwanCount = chipSpwanCount;
            c.GetComponent<controlCustomer>().fWaitTime = slotMachineChipSpwanWaitTimer;

            x = delayBetweenChipSpwan;
        }

        //////VIPCustomer Spwan////////
        if (AvailableSeats.Count > 0 && y > 0)
            y -= Time.deltaTime;
        if (AvailableSeats.Count > 0 && y <= 0)
        {
            GameObject c = Instantiate(VIPCustomer, spwanPosition);
            c.transform.position = spwanPosition.position;
            c.GetComponent<moveCustomer>().DestinationToSeat = AvailableSeats[AvailableSeats.Count - 1].transform;
            AvailableSeats[AvailableSeats.Count - 1].GetComponent<controlSeat>().isOccupied = true;
            if (AvailableSeats.Contains(c.GetComponent<moveCustomer>().DestinationToSeat.gameObject))
                AvailableSeats.Remove(c.GetComponent<moveCustomer>().DestinationToSeat.gameObject);
            c.GetComponent<controlCustomer>().section = section;
            c.GetComponent<controlCustomer>().DestinationToExit = DestinationForExit;
            c.GetComponent<controlCustomer>().chipSpwanCount = VIPChipSpwanCount;
            c.GetComponent<controlCustomer>().fWaitTime = slotMachineChipSpwanWaitTimer;
            y = Random.Range(2, 3.5f);
        }
    }

    public void SpwanCustomerForTable()
    {       
        if (AvailableSeats.Count > 0 && x > 0)
            x -= Time.deltaTime;

        if (AvailableSeats.Count > 0 && x <= 0)
        {
            GameObject c = Instantiate(Customer[Random.Range(0, Customer.Length)], spwanPosition);
            c.transform.position = spwanPosition.position;
            c.GetComponent<moveCustomer>().DestinationToSeat = AvailableSeats[AvailableSeats.Count - 1].transform;
            AvailableSeats[AvailableSeats.Count - 1].GetComponent<controlSeat>().isOccupied = true;
            if (AvailableSeats.Contains(c.GetComponent<moveCustomer>().DestinationToSeat.gameObject))
                AvailableSeats.Remove(c.GetComponent<moveCustomer>().DestinationToSeat.gameObject);
            c.GetComponent<controlCustomer>().section = section;
            c.GetComponent<controlCustomer>().DestinationToExit = DestinationForExit;
            c.GetComponent<controlCustomer>().chipSpwanCount = chipSpwanCount;
            c.GetComponent<controlCustomer>().isCustomerForTable = true;
            c.GetComponent<controlCustomer>().table = GetComponent<controlTable>();
            x = delayBetweenChipSpwan;
        }
    }
}
