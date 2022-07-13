using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool AlwaysVisible = false;

    MeshRenderer[] renderers;
    int invisibleCount = 0;



    private void Start()
    {
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        if (AlwaysVisible)
            SetVisible(true);
        else
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

    private void FixedUpdate()
    {
        if (AlwaysVisible) return;

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
