using System;
using System.Collections.Generic;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents.Containers;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recongizers
{
    public abstract class ActionReconBase : IActionRecon
    {
        private bool isLeft;
        private bool isDebug;
        private Action<ActionId> onAction;
        private IActionReconCompContainer compContainer;

        public ActionReconBase(bool isLeft, Action<ActionId> onAction)
        {
            this.isLeft = isLeft;
            this.onAction = onAction;
            this.compContainer = CreateReconCompContainer(OnAction);
        }

        public void OnUpdate(List<Vector3> keyPoints)
        {
            UpdateReconComps(keyPoints);
        }

        protected void SendAction(ActionId actionId)
        {
            if(onAction != null)
            {
                onAction(actionId);
            }
        }

        protected abstract void OnAction(bool active);

        protected virtual IActionReconCompContainer CreateReconCompContainer(Action<bool> onAction)
        {
            return new ActionReconCompContainerAnd(onAction);
        }

        protected void AddReconComp(IActionReconComp comp)
        {
            compContainer.AddReconComp(comp);
        }

        protected bool IsLeft()
        {
            return isLeft;
        }

        protected bool IsDebug()
        {
            return isDebug;
        }

        private void UpdateReconComps(List<Vector3> keyPoints)
        {
            compContainer.UpdateReconComps(keyPoints);
        }

        public virtual void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
            compContainer.SetDebug(isDebug);
        }
    }
}
