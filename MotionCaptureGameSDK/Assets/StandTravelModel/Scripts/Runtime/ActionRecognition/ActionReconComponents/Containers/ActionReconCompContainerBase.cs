using System;
using System.Collections.Generic;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents.Containers
{
    public abstract class ActionReconCompContainerBase : IActionReconCompContainer
    {
        private bool isDebug;
        private Action<bool> onAction;
        private List<IActionReconComp> reconComps;

        public ActionReconCompContainerBase(Action<bool> onAction)
        {
            this.onAction = onAction;
            this.reconComps = new List<IActionReconComp>();
        }

        public virtual void AddReconComp(IActionReconComp comp)
        {
            reconComps.Add(comp);
            comp.SetAction(
                active => OnActionDetect(comp, active)
            );
        }

        public void UpdateReconComps(List<Vector3> keyPoints)
        {
            for(int i = 0; i < reconComps.Count; i++)
            {
                reconComps[i].OnUpdate(keyPoints);
            }
        }

        protected void SendEvent(bool active)
        {
            if(onAction != null)
            {
                onAction(active);
            }
        }

        protected bool IsDebug()
        {
            return isDebug;
        }

        protected abstract void OnActionDetect(IActionReconComp comp, bool active);

        public void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
            foreach(var recon in reconComps)
            {
                recon.SetDebug(isDebug);
            }
        }
    }
}