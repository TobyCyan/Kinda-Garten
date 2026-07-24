using UnityEngine;

public class Seat : MonoBehaviour
{
    private Transform seatTransform;
    public Transform SeatTransform => seatTransform;
    private bool isOccupied = false;

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    private void Awake()
    {
        seatTransform = GetComponent<Transform>();
    }
}
