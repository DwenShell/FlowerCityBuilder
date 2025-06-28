using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(M_uiObjectButton))]
public class M_objectPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        M_uiObjectButton placer = (M_uiObjectButton)target;

        if (placer.objectToPlace != placer.lastObjectToPlace/* || placer.objectToPlace == placer.lastObjectToPlace*/)
        {
            updateScreenShot(placer);
        }
        if (placer.tempRotation != placer.lastTempRotation || placer.tempPosition != placer.lastTempPosition || placer.tempScale != placer.lastTempScale)
        {
            updateScreenShot(placer);
        }
    }

    private static void updateScreenShot(M_uiObjectButton placer)
    {
        placer.lastObjectToPlace = placer.objectToPlace;
        placer.lastTempPosition = placer.tempPosition;
        placer.lastTempRotation = placer.tempRotation;
        placer.lastTempScale = placer.tempScale;

        Debug.Log("Prefab change");

        Texture2D img = placer.CapturePrefab(placer.objectToPlace);
        placer.buttonImage.sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
    }
}
