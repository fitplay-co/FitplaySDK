namespace StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents
{
    public class ActionReconCompAngleGradient : ActionReconCompAngle
    {
        private bool needExpanding;

        public ActionReconCompAngleGradient(float angleMin, float angleMax, bool needExpanding, ReconCompAngleGetter angleGetter) : base(angleMin, angleMax, angleGetter)
        {
            this.needExpanding = needExpanding;
        }

        protected override void OnStateFlip(bool isInAngle)
        {
            if(isInAngle)
            {
                if(needExpanding == IsExpanding())
                {
                    base.OnStateFlip(true);
                    return;
                }
            }
            base.OnStateFlip(false);
        }
    }
}