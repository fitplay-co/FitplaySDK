using UnityEngine;

public class StepStateConvertToEvent
{
    private class StepStater
    {
        private int leg;
        private int frame;

        public bool UpdateLeg(int leg)
        {
            if(this.leg != leg)
            {
                this.leg = leg;
                this.frame = Time.frameCount;
                return true;
            }
            return false;
        }

        public int GetLeg()
        {
            return leg;
        }

        public int GetFrame()
        {
            return frame;
        }
    }

    private int updateFrame;
    private StepState state;
    private StepStater left = new StepStater();
    private StepStater right = new StepStater();

    public StepState GetState()
    {
        return state;
    }

    public void OnUpdate(int legLeft, int legRight)
    {
        left.UpdateLeg(legLeft);
        right.UpdateLeg(legRight);

        if(state == StepState.Idle)
        {
            if(TryChangeState(left, 1, StepState.LeftUp))
            {
                return;
            }

            if(TryChangeState(right, 1, StepState.RightUp))
            {
                return;
            }
            return;
        }

        if(state == StepState.LeftUp)
        {
            if(TryChangeState(left, -1, StepState.LeftDown))
            {
                return;
            }

            if(TryChangeState(left, 0, StepState.Idle))
            {
                return;
            }
            return;
        }

        if(state == StepState.LeftDown)
        {
            if(TryChangeState(left, 1, StepState.LeftUp))
            {
                return;
            }

            if(TryChangeState(left, 0, StepState.Idle))
            {
                return;
            }

            if(TryChangeState(right, 1, StepState.RightUp))
            {
                return;
            }
            return;
        }

        if(state == StepState.RightUp)
        {
            if(TryChangeState(right, -1, StepState.RightDown))
            {
                return;
            }

            if(TryChangeState(right, 0, StepState.Idle))
            {
                return;
            }
            return;
        }

        if(state == StepState.RightDown)
        {
            if(TryChangeState(right, 1, StepState.RightUp))
            {
                return;
            }

            if(TryChangeState(right, 0, StepState.Idle))
            {
                return;
            }

            if(TryChangeState(left, 1, StepState.LeftUp))
            {
                return;
            }
            return;
        }
    }

    private bool TryChangeState(StepStater stepStater, int leg, StepState nextState)
    {
        if(LegEquals(stepStater, leg))
        {
            ChangeState(nextState);
            return true;
        }
        return false;
    }

    private void ChangeState(StepState state)
    {
        //Debug.Log("ChangeState ----------------------------------------------> " + state + "|" + Time.frameCount);
        this.state = state;
        this.updateFrame = Time.frameCount;
    }

    private bool LegEquals(StepStater stater, int leg)
    {
        var preLeg = 0;
        if(GetLeg(stater, out preLeg))
        {
            return preLeg == leg;
        }
        return false;
    }

    private bool GetLeg(StepStater stater, out int leg)
    {
        leg = stater.GetLeg();
        var result = stater.GetFrame() > updateFrame;
        //Debug.Log(" leg -> " + leg + "| frame -> " + stater.GetFrame() + "| updateFrame -> " + updateFrame + "| curFrame -> " + Time.frameCount);
        return result;
    }
}