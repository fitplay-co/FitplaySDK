using System;
using UnityEngine;

public class ActionReconLegUp : ActionReconLeg
{
    public ActionReconLegUp(bool isLeft, Action<ActionId> onAction) : base(
        isLeft,
        true,
        new Vector2(angleUpKneeMin, angleUpKneeMax),
        new Vector2(angleUpHipMin, angleUpHipMax),
        onAction
        )
    {
    }
}