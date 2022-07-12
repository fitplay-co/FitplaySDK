namespace SEngineCharacterController
{
    public class HealthPoint
    {
        public IntNumeric HealthPointNumeric = new IntNumeric();
        public IntNumeric HealthPointMaxNumeric = new IntNumeric();
        
        public int Value => HealthPointNumeric.Value;
        public int MaxValue => HealthPointMaxNumeric.Value;


        public void SetBase(int value)
        {
            HealthPointNumeric.SetBase(value);
        }

        public void SetMaxValue(int value)
        {
            HealthPointMaxNumeric.SetBase(value);
        }
        
        public void Minus(int value)
        {
            HealthPointNumeric.MinusBase(value);
        }

        public void Add(int value)
        {
            HealthPointNumeric.AddBase(value);
        }

        /// <summary>
        /// 血量百分比
        /// </summary>
        /// <returns></returns>
        public float Percent()
        {
            return (float)Value / MaxValue;
        }
        
        /// <summary>
        /// 是否满血
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        { 
            return Value == MaxValue;
        }

        /// <summary>
        /// 是否死亡
        /// </summary>
        /// <returns></returns>
        public bool isDie()
        {
            return Value <= 0;
        }
    }
}