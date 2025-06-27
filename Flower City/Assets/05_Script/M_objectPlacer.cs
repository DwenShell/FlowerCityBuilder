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

    private void Awake()
    {
        Instance = this;
    }

    public void StartPlacing(GameObject prefab, float offset)
    {
        if (currentGhost != null)
            Destroy(currentGhost);

        objectToPlace = prefab;
        heightOffset = offset;
        currentGhost = Instantiate(prefab);
        MakeTransparent(currentGhost);
        isPlacing = true;
    }

    void Update()
    {
        if (!isPlacing) return;

        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
            return; // On sort pour éviter de continuer la logique
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

                if (Input.GetMouseButtonDown(0) && isFree)
                {
                    PlaceObject(tile);
                }
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
        Instantiate(objectToPlace, new Vector3(tile.transform.position.x,
            tile.transform.position.y + heightOffset,
            tile.transform.position.z),
            Quaternion.identity);
        tile.SetOccupied(true);
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
