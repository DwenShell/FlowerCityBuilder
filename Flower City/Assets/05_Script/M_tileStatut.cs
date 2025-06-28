using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_tileStatut : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    public Renderer tileRenderer;
    public GameObject attachedProps;

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
    [ContextMenu ("Reset Tile")]
    public void ResetTile()
    {
        Destroy(attachedProps);
        this.GetComponent<Renderer>().material.color = Color.white;
        SetOccupied(false);
    }
}
