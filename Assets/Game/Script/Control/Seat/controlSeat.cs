using UnityEngine;

public class controlSeat : MonoBehaviour
{
    public bool isOccupied;
    public bool isLocked;
    public float rotationFace = 90;

    public CustomerSpwan CustomerSpwan;
    private GameManager GameManager;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();       
    }


    void Update()
    {
        if (!isLocked)
        {
            if (!GameManager.Seats.Contains(this.gameObject))GameManager.Seats.Add(this.gameObject);
            if (isOccupied && CustomerSpwan.AvailableSeats.Contains(this.gameObject)) CustomerSpwan.AvailableSeats.Remove(this.gameObject);
            if (!isOccupied && !CustomerSpwan.AvailableSeats.Contains(this.gameObject)) CustomerSpwan.AvailableSeats.Add(this.gameObject);
        }
    }
}
