using OkapifanUtils.GenericSelectionUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyMenuScreen : SelectionUI<ButtonSlot>
{
    private void Start()
    {
        SetItems(GetComponentsInChildren<ButtonSlot>().ToList());
    }
}
