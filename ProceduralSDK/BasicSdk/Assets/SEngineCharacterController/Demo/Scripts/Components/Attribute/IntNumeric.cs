using System.Collections.Generic;

namespace SEngineCharacterController
{
    /// <summary>
    /// 整形修饰器
    /// </summary>
    public class IntModifier
    {
        public int Value;
    }
    /// <summary>
    /// 整形修饰器集合
    /// </summary>
    public class IntModifierCollection
    {
        public int TotalValue { get; private set; }
        private List<IntModifier> Modifiers { get; } = new List<IntModifier>();

        public int AddModifier(IntModifier modifier)
        {
            Modifiers.Add(modifier);
            Update();
            return TotalValue;
        }

        public int RemoveModifier(IntModifier modifier)
        {
            Modifiers.Remove(modifier);
            Update();
            return TotalValue;
        }

        public void Update()
        {
            TotalValue = 0;
            foreach (var item in Modifiers)
            {
                TotalValue += item.Value;
            }
        }
    }
    /// <summary>
    /// 整形数值
    /// </summary>
    public class IntNumeric
    {
        public int Value { get; private set; }
        public int baseValue { get; private set; }
        public int add { get; private set; }
        public int pctAdd { get; private set; }
        public int finalAdd { get; private set; }
        public int finalPctAdd { get; private set; }
        private IntModifierCollection AddCollection { get; } = new IntModifierCollection();
        private IntModifierCollection PctAddCollection { get; } = new IntModifierCollection();
        private IntModifierCollection FinalAddCollection { get; } = new IntModifierCollection();
        private IntModifierCollection FinalPctAddCollection { get; } = new IntModifierCollection();

        public void Initialize()
        {
            baseValue = add = pctAdd = finalAdd = finalPctAdd = 0;
        }
        
        public int SetBase(int value)
        {
            baseValue = value;
            Update();
            return baseValue;
        }
        
        public int AddBase(int value)
        {
            baseValue += value;
            Update();
            return baseValue;
        }

        public int MinusBase(int value)
        {
            baseValue -= value;
            if (baseValue < 0) baseValue = 0;
            Update();
            return baseValue;
        }

        public void AddAddModifier(IntModifier modifier)
        {
            add = AddCollection.AddModifier(modifier);
            Update();
        }
        
        public void AddPctAddModifier(IntModifier modifier)
        {
            pctAdd = PctAddCollection.AddModifier(modifier);
            Update();
        }
        
        public void AddFinalAddModifier(IntModifier modifier)
        {
            finalAdd = FinalAddCollection.AddModifier(modifier);
            Update();
        }
        
        public void AddFinalPctAddModifier(IntModifier modifier)
        {
            finalPctAdd = FinalPctAddCollection.AddModifier(modifier);
            Update();
        }
        
        public void RemoveAddModifier(IntModifier modifier)
        {
            add = AddCollection.RemoveModifier(modifier);
            Update();
        }
        
        public void RemovePctAddModifier(IntModifier modifier)
        {
            pctAdd = PctAddCollection.RemoveModifier(modifier);
            Update();
        }
        
        public void RemoveFinalAddModifier(IntModifier modifier)
        {
            finalAdd = FinalAddCollection.RemoveModifier(modifier);
            Update();
        }
        
        public void RemoveFinalPctAddModifier(IntModifier modifier)
        {
            finalPctAdd = FinalPctAddCollection.RemoveModifier(modifier);
            Update();
        }

        public void Update()
        {
            var value1 = baseValue;
            var value2 = (value1 + add) * (100 + pctAdd) / 100f;
            var value3 = (value2 + finalAdd) * (100 + finalPctAdd) / 100f;
            Value = (int)value3;
        }
    }
}