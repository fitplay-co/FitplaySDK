using System.Collections.Generic;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition
{
    public class ActionAngleCache
    {
        private int capacity;
        private List<float> angles;

        public ActionAngleCache(int capacity)
        {
            this.capacity = capacity;
            this.angles = new List<float>();
        }

        public void Append(float angle)
        {
            angles.Add(angle);
            if(angles.Count > capacity)
            {
                angles.RemoveAt(0);
            }
        }

        public bool IsIncreasing()
        {
            if(angles.Count > capacity * 0.5f)
            {
                var min = float.MaxValue;
                var max = float.MinValue;

                for(int i = 0; i < angles.Count; i++)
                {
                    if(angles[i] < min)
                    {
                        min = angles[i];
                    }

                    if(angles[i] > max)
                    {
                        max = angles[i];
                    }
                }
                return max - min > 10;
            }
            return false;
        }
    }
}