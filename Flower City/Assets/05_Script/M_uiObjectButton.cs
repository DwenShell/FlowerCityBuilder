using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_uiObjectButton : MonoBehaviour
{
    public GameObject objectPrefab;
    public float offset;

    public void OnClick()
    {
        M_objectPlacer.Instance.StartPlacing(objectPrefab, offset);
    }
}
