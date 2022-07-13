using System;
using System.Collections.Generic;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents.Containers
{
    public class ActionReconCompContainerAnd : ActionReconCompContainerBase
    {
        private Dictionary<IActionReconComp, bool> compsActiveCache;

        public ActionReconCompContainerAnd(Action<bool> onAction) : base(onAction)
        {
            compsActiveCache = new Dictionary<IActionReconComp, bool>();
        }

        public override void AddReconComp(IActionReconComp comp)
        {
            base.AddReconComp(comp);
            compsActiveCache.Add(comp, false);
        }

        protected override void OnActionDetect(IActionReconComp comp, bool active)
        {
            RefreshCache(comp, active);

            foreach(var item in compsActiveCache)
            {
                if(!item.Value)
                {
                    SendEvent(false);
                    return;
                }
            }

            SendEvent(true);
        }

        private void RefreshCache(IActionReconComp comp, bool active)
        {
            try {
                compsActiveCache[comp] = active;
            } catch(Exception e) {
                UnityEngine.Debug.LogError("Can not find IActionReconComp !");
            }
        }
    }
}