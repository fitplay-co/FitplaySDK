using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime;
using UnityEngine;

namespace TurnModel.Scripts.TestDemo
{
    public class DisplayHud : MonoBehaviour
    {
        [SerializeField] private TurnController turnController;
        [SerializeField] private StandTravelModelManager standTravelModelManager;

        private int startYOffset = 320;
        private bool dirty;
        
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

            if(GUI.Button(new Rect(20, startYOffset + 300, 200, 50), $"使用最新OS"))
            {
                WalkActionItem.useRealtimeData = true;
            }

            if(GUI.Button(new Rect(20, startYOffset + 360, 200, 50), $"使用之前OS"))
            {
                WalkActionItem.useRealtimeData = false;
            }

            GUI.Label(new Rect(20, startYOffset + 435, 300, 50), $"走跑切换阈值 " + standTravelModelManager.GetRunThrehold().ToString("0.000"));

            var runThrehold = GUI.HorizontalSlider(new Rect(20, startYOffset + 460, 200, 20), standTravelModelManager.GetRunThrehold(), 0, 4);
            if(runThrehold != standTravelModelManager.GetRunThrehold())
            {
                dirty = true;
                standTravelModelManager.SetRunThrehold(runThrehold);
            }

            GUI.Label(new Rect(20, startYOffset + 485, 300, 50), $"速度缩放倍数 " + standTravelModelManager.GetRunSpeedScale().ToString("0.000"));

            var speedScale = GUI.HorizontalSlider(new Rect(20, startYOffset + 510, 200, 20), standTravelModelManager.GetRunSpeedScale(), 0, 10);
            if(speedScale != standTravelModelManager.GetRunSpeedScale())
            {
                dirty = true;
                standTravelModelManager.SetRunSpeedScale(speedScale);
            }

            var useFrequencyCur = standTravelModelManager.GetUseFrequency();
            var useSpeedCur = !standTravelModelManager.GetUseFrequency();
            var useFrequency = GUI.Toggle(new Rect(250, startYOffset + 470, 80, 50), useFrequencyCur, "使用步频切换");
            var useSpeed = GUI.Toggle(new Rect(250, startYOffset + 490, 80, 50), useSpeedCur, "使用速度切换");

            if(useFrequency != useFrequencyCur || useSpeed != useSpeedCur)
            {
                dirty = true;
                standTravelModelManager.SetUseFrequency(useFrequency);
            }

            if(dirty && GUI.Button(new Rect(20, startYOffset + 530, 300, 50), $"保存"))
            {
                dirty = false;
                standTravelModelManager.SerializeParams();
            }
        }
    }
}