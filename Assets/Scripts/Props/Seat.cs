using UnityEngine;

public class Seat : MonoBehaviour
{
    private bool isOccupied = false;

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }
}
