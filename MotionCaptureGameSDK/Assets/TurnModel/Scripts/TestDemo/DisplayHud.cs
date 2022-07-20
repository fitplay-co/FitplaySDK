using UnityEngine;

namespace TurnModel.Scripts.TestDemo
{
    public class DisplayHud : MonoBehaviour
    {
        [SerializeField] private TurnController turnController;
        private int startYOffset = 280;

        public void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = Color.red;

            if (turnController != null)
            {
                GUI.Label(new Rect(20, startYOffset, 400, 80),
                    $"转向偏转度:{turnController.angle.ToString("0.00")}", labelStyle);
                GUI.Label(new Rect(20, startYOffset + 40, 400, 80),
                    $"转向判定为:{turnController.turnLeftOrRight}", labelStyle);
                GUI.Label(new Rect(20,startYOffset + 80, 400, 80),
                    $"转向速度为:{turnController.turnValue.ToString("0.00")}", labelStyle);
            }
        }
    }
}