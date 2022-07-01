using System;
using UnityEngine;

public class ActionReconLegDown : ActionReconLeg
{
    public ActionReconLegDown(bool isLeft, Action<ActionId> onAction) : base(
        isLeft,
        false,
        new Vector2(0, angleUpKneeMin),
        new Vector2(angleUpHipMax, 180),
        onAction
        )
    {
    }
}