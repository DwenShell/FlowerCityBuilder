using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_tileStatut : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    public Renderer tileRenderer;

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }
    public void SetTileColor(Color color)
    {
        if (tileRenderer != null)
        {
            tileRenderer.material.color = color;
        }
    }
}
