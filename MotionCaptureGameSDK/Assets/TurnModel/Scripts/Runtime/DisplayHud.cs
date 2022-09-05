using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime;
using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using UnityEngine;

namespace TurnModel.Scripts.TestDemo
{
    public class DisplayHud : MonoBehaviour
    {
        [SerializeField] private TurnController turnController;
        [SerializeField] private StandTravelModelManager standTravelModelManager;

        private int startYOffset;
        private bool dirty;
        private RunConditioner runConditioner;

        private void Start()
        {
            standTravelModelManager = GameObject.FindObjectOfType<StandTravelModelManager>();
        }
        
        protected void OnEnable()
        {
            TurnEventHandler.onLocalPlayerSpawn += OnLocalSpawn;
        }
        
        private void OnLocalSpawn()
        {
            if (standTravelModelManager != null) return;
            standTravelModelManager = GameObject.Find("LocalPlayer").transform
                .GetComponentInChildren<StandTravelModelManager>();
        }

        public void OnGUI()
        {
            if (standTravelModelManager == null)
            {
                return;
            }

            startYOffset = 360;
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = Color.red;

            if (turnController != null)
            {
                string turn = (turnController.HowToTurn == TurnController.TurnMode.UseTorsoRotation) ? "躯干转身模式" : "左右倾斜模式";
                string turnData = (turnController.HowToTurn == TurnController.TurnMode.UseTorsoRotation) ? turnController.TurnData.ToString() : "";
                GUI.Label(new Rect(20, startYOffset, 400, 80),
                    $"转向偏转度:{turnController.angle.ToString("0.00")}", labelStyle);
                GUI.Label(new Rect(20, startYOffset + 40, 400, 80),
                    $"转向判定为:{turnController.turnLeftOrRight}", labelStyle);
                GUI.Label(new Rect(20,startYOffset + 80, 400, 80),
                    $"转向速度为:{turnController.turnValue.ToString("0.00")}", labelStyle);
                if (GUI.Button(new Rect(20, startYOffset + 140, 300, 50), "当前转身模式:" + turn))
                {
                    turnController.SwitchTurnMode();
                }

                if (turnController.HowToTurn == TurnController.TurnMode.UseTorsoRotation)
                {
                    if (GUI.Button(new Rect(20, startYOffset + 200, 300, 50), "当前数据源:" + turnData))
                    {
                        turnController.SwitchTurnDataMode();
                    }
                }
                    
            }

            if(runConditioner == null)
            {
                runConditioner = standTravelModelManager.GetRunConditioner();
            }

            if(runConditioner != null)
            {
                labelStyle.fontSize = 25;
                var actTime = runConditioner.GetActTimeLeft();
                GUI.Label(new Rect(640, 32, 400, 40), "左腿动作耗时 " + actTime.ToString("f2"), labelStyle);

                actTime = runConditioner.GetActTimeRight();
                GUI.Label(new Rect(1280, 32, 400, 40), "右腿动作耗时 " + actTime.ToString("f2"), labelStyle);
            }
        }
    }
}