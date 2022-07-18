using System;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class StateFader
    {
        private bool paused;
        private bool inverse;
        private float progress;
        private float fadeSpeed;
        private Action onComplete;

        public StateFader(float fadeSpeed)
        {
            this.fadeSpeed = fadeSpeed;
        }

        public void SetCompleteEvent(Action onComplete)
        {
            this.onComplete = onComplete;
        }

        public virtual void Reset(bool inverse = false)
        {
            this.inverse = inverse;
            this.progress = 0;
        }

        public void OnUdpate()
        {
            if(progress < 1)
            {
                progress += Time.deltaTime * fadeSpeed;
                OnProgress(inverse ? (1 - progress) : progress);

                if(progress >= 1)
                {
                    if(onComplete != null)
                    {
                        onComplete();
                    }
                }
            }
        }

        public bool IsComplete()
        {
            return progress >= 1;
        }

        public bool IsPaused()
        {
            return paused;
        }

        public void SetPause(bool paused)
        {
            this.paused = paused;
        }

        protected virtual void OnProgress(float percent)
        {

        }
    }
}