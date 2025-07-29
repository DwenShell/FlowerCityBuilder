
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollBoxManager : MonoBehaviour
{
    public enum LayoutMode { List, Grid }

    [Header("References")]
    public GameObject itemPrefab;
    public GameObject separatorPrefab;
    public Transform content;
    public VerticalLayoutGroup verticalLayout;
    public GridLayoutGroup gridLayout;

    private List<GameObject> currentItems = new();

    public void SetLayoutMode(LayoutMode mode)
    {
        verticalLayout.enabled = (mode == LayoutMode.List);
        gridLayout.enabled = (mode == LayoutMode.Grid);
    }

    public void Clear()
    {
        foreach (var go in currentItems)
            Destroy(go);
        currentItems.Clear();
    }

    public void Populate(List<ScrollBoxItem> items)
    {
        Clear();

        foreach (var item in items)
        {
            GameObject go;

            if (item.isSeparator)
            {
                go = Instantiate(separatorPrefab, content);
            }
            else
            {
                go = Instantiate(itemPrefab, content);
                var text = go.GetComponentInChildren<Text>();
                if (text != null)
                    text.text = item.label;

                if (item.onClick != null)
                    go.GetComponent<Button>()?.onClick.AddListener(() => item.onClick());
            }

            currentItems.Add(go);
        }
    }
}

[System.Serializable]
public class ScrollBoxItem
{
    public string label;
    public bool isSeparator;
    public System.Action onClick;

    public ScrollBoxItem(string label, System.Action onClick = null)
    {
        this.label = label;
        this.onClick = onClick;
        this.isSeparator = false;
    }

    public static ScrollBoxItem Separator()
    {
        return new ScrollBoxItem("---") { isSeparator = true };
    }
}
