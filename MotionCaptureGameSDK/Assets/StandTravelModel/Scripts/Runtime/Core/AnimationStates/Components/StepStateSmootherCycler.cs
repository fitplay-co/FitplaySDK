using UnityEngine;

public class StepStateSmootherCycler
{
    private int baseFrame;
    private int frameCount;

    public StepStateSmootherCycler(int frameCount)
    {
        this.frameCount = frameCount;
    }

    public void IncreaseBaseFrame()
    {
        baseFrame += frameCount;
    }
}