using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_objectPlacer : MonoBehaviour
{
    public static M_objectPlacer Instance;

    [Header("Layer Mask")]
    public LayerMask tileLayerMask;

    private GameObject currentGhost;
    private GameObject objectToPlace;
    private bool isPlacing = false;
    private M_tileStatut hoveredTile;

    private float heightOffset;
    private float spawnScale;
    private Color tileColor;
    private bool delObj;

    private void Awake()
    {
        Instance = this;
    }

    public void StartPlacing(GameObject prefab, float offset, float scale, Color color, bool del)
    {
        if (currentGhost != null)
            Destroy(currentGhost);

        tileColor = color;
        objectToPlace = prefab;
        heightOffset = offset;
        spawnScale = scale;
        currentGhost = Instantiate(prefab);
        currentGhost.transform.position = new Vector3(500, 0, 0);
        currentGhost.transform.localScale = new Vector3(scale, scale, scale);
        MakeTransparent(currentGhost);
        isPlacing = true;
        delObj = del;
    }

    void Update()
    {
        if (!isPlacing) return;

        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
            return; 
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayerMask))
        {
            M_tileStatut tile = hit.collider.GetComponent<M_tileStatut>();
            if (tile != null)
            {
                hoveredTile = tile;
                currentGhost.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + heightOffset + 0.1f, tile.transform.position.z);

                bool isFree = !tile.IsOccupied;
                SetGhostColor(isFree ? Color.green : Color.red);

                if (Input.GetMouseButtonDown(0) && isFree && !delObj)
                {
                    PlaceObject(tile);
                }
                if(Input.GetMouseButtonDown(0) && !isFree && delObj)
                    tile.ResetTile();
            }
            else
            {
                hoveredTile = null;
                currentGhost.transform.position = hit.point;
                SetGhostColor(Color.red);
            }
        }
    }

    private void PlaceObject(M_tileStatut tile)
    {
        GameObject placedProps = Instantiate(objectToPlace, new Vector3(tile.transform.position.x,
            tile.transform.position.y + heightOffset,
            tile.transform.position.z),
            Quaternion.identity);
        placedProps.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
        tile.SetOccupied(true);
        tile.SetTileColor(tileColor);
        tile.attachedProps = placedProps;
    }

    private void MakeTransparent(GameObject obj)
    {
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                mat.shader = Shader.Find("Universal Render Pipeline/Unlit");
                Color c = mat.color;
                c.a = 0.5f;
                mat.color = c;
            }
        }
    }

    private void SetGhostColor(Color color)
    {
        foreach (var r in currentGhost.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                mat.color = color;
            }
        }
    }

    private void CancelPlacement()
    {
        isPlacing = false;

        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }
    }
}
