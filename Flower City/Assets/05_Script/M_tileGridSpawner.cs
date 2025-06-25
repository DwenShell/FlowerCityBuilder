using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_tileGridSpawner : MonoBehaviour
{
    [Header("Tuile")]
    public GameObject tilePrefab;
    public Vector3 tileSize = new Vector3(1, 0, 1);

    [Header("Grille")]
    public int tilesX = 10;
    public int tilesZ = 10;
    public float spawnDelay = 0.05f;

    [Header("Animation")]
    public float dropHeight = 2f;
    public float dropDuration = 0.3f;
    public AnimationCurve dropCurve;

    private List<GameObject> spawnedTiles = new();

    private void Start()
    {
        StartCoroutine(SpawnTiles());
    }

    IEnumerator SpawnTiles()
    {
        Vector3 origin = transform.position;

        for (int z = 0; z < tilesZ; z++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                Vector3 targetPos = origin + new Vector3(x * tileSize.x, 0, z * tileSize.z);
                GameObject tile = Instantiate(tilePrefab, targetPos + Vector3.up * dropHeight, Quaternion.identity, transform);

                spawnedTiles.Add(tile);
                StartCoroutine(AnimateDrop(tile.transform, targetPos));
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    IEnumerator AnimateDrop(Transform tile, Vector3 targetPos)
    {
        Vector3 startPos = tile.position;
        float t = 0f;

        while (t < dropDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / dropDuration);
            float eased = dropCurve != null ? dropCurve.Evaluate(normalized) : normalized;
            tile.position = Vector3.Lerp(startPos, targetPos, eased);
            yield return null;
        }

        tile.position = targetPos;
    }

    [ContextMenu("Clear Tiles")]
    public void ClearTiles()
    {
        StopAllCoroutines();

        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            DestroyImmediate(child.gameObject);
        }

        spawnedTiles.Clear();
    }

    [ContextMenu("Recreate Tiles")]
    public void SpawnTilesFromEditor()
    {
        ClearTiles();
        StartCoroutine(SpawnTiles());
    }
}
