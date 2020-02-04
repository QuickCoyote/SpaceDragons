using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTargets : MonoBehaviour
{
    [SerializeField] GameObject Selected = null;
    [SerializeField] GameObject MapIcon = null;

    public bool IsSelected = false;

    private void Start()
    {
        Selected.SetActive(IsSelected);
    }

    public void SelectTarget(bool select)
    {
        IsSelected = select;
        Selected.SetActive(IsSelected);
    }
}
