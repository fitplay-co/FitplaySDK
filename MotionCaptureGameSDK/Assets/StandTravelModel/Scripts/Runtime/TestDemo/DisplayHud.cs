using UnityEngine;

namespace StandTravelModel.TestDemo
{
    public class DisplayHud : MonoBehaviour
    {
        public StandTravelModelManager standTravelModelManager;

        public void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = Color.red;
            GUI.Label(new Rect(20, 40, 300, 40), (standTravelModelManager.Enabled ? "动捕模式": "非动捕模式"), labelStyle);
            GUI.Label(new Rect(20, 160, 300, 40), "步频:" + $"{standTravelModelManager.currentFrequency:0.00}", labelStyle);
            
            if (standTravelModelManager.currentLeg < -0.5)
            {
                GUI.Label(new Rect(20, 80, 300, 40), "右脚", labelStyle);
            }
            else if (standTravelModelManager.currentLeg > 0.5)
            {
                GUI.Label(new Rect(20, 80, 300, 40), "左脚", labelStyle);
            }
            else
            {
                GUI.Label(new Rect(20, 80, 300, 40), "没抬脚", labelStyle);
            }

            switch (standTravelModelManager.currentMode)
            {
                case MotionMode.Stand:
                    GUI.Label(new Rect(20, 120, 300, 40), "Stand模式", labelStyle);
                    break;
                case MotionMode.Travel:
                    GUI.Label(new Rect(20, 120, 300, 40), "Travel模式", labelStyle);
                    break;
            }
        }
    }
}