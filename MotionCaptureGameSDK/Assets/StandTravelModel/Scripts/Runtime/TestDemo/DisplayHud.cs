using UnityEngine;

namespace StandTravelModel.TestDemo
{
    public class DisplayHud : MonoBehaviour
    {
        public StandTravelModelManager standTravelModelManager;

        public void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 60;
            labelStyle.normal.textColor = Color.red;
            
            GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.2f, 300, 80), "步频:" + $"{standTravelModelManager.currentFrequency:0.00}", labelStyle);
            
            if (standTravelModelManager.currentLeg < -0.5)
            {
                GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, 300, 80), "右脚", labelStyle);
            }
            else if (standTravelModelManager.currentLeg > 0.5)
            {
                GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, 300, 80), "左脚", labelStyle);
            }
            else
            {
                GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, 300, 80), "没抬脚", labelStyle);
            }

            switch (standTravelModelManager.currentMode)
            {
                case MotionMode.Stand:
                    GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.3f, 300, 80), "Stand模式", labelStyle);
                    break;
                case MotionMode.Travel:
                    GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.3f, 300, 80), "Travel模式", labelStyle);
                    break;
            }
        }
    }
}