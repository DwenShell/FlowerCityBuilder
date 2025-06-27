using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_tileStatut : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }
}
