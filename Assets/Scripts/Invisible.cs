using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    MeshRenderer[] renderers;
    int invisibleCount = 0;

    private void Start()
    {
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        SetVisible(false);
    }

    public void IncreaseVisibleCount(int cnt)
    {
        if (invisibleCount <= 0)
        {
            SetVisible(true);
        }
        invisibleCount = cnt;

    }

    private void SetVisible(bool onoff)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = onoff;
        }
    }

    private void Update()
    {
        if (invisibleCount > 0)
        {
            invisibleCount--;

            if (invisibleCount <= 0)
            {
                SetVisible(false);
            }
        }
    }
}
