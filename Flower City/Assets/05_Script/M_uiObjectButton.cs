using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_uiObjectButton : MonoBehaviour
{
    public GameObject objectToPlace;

    [HideInInspector] public GameObject lastObjectToPlace;
    [HideInInspector] public Vector3 lastTempPosition;
    [HideInInspector] public Quaternion lastTempRotation;
    [HideInInspector] public Vector3 lastTempScale;

    public float offset;
    public Color colorTile;

    [Header ("Screenshot Parameter")]
    public Vector3 tempPosition;
    public Quaternion tempRotation;
    public Vector3 tempScale;
    public UnityEngine.UI.Image buttonImage;

    public void OnClick()
    {
        M_objectPlacer.Instance.StartPlacing(objectToPlace, offset, colorTile);
    }

    public Texture2D CapturePrefab(GameObject prefab)
    {
        var temp = GameObject.Instantiate(prefab);
        temp.transform.position = tempPosition; // placer au centre d'une caméra
        temp.transform.rotation = tempRotation;
        temp.transform.localScale = tempScale;

        var camera = new GameObject("TempCamera").AddComponent<Camera>();
        camera.backgroundColor = Color.clear;
        camera.clearFlags = CameraClearFlags.SolidColor;

        RenderTexture rt = new RenderTexture(512, 512, 24);
        camera.targetTexture = rt;

        camera.Render();

        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        screenshot.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        screenshot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.DestroyImmediate(rt);
        GameObject.DestroyImmediate(temp);
        GameObject.DestroyImmediate(camera.gameObject);

        return screenshot;
    }

}
