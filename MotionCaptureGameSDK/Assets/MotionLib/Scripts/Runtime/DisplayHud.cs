using MotionCaptureBasic.OSConnector;
using StandTravelModel.Scripts.Runtime;
using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using UnityEngine;

namespace MotionLib.Scripts
{
    public class DisplayHud : MonoBehaviour
    {
        [SerializeField] private MotionLibController motionController;
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
            MotionLibEventHandler.onLocalPlayerSpawn += OnLocalSpawn;
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

           /* startYOffset = 360;
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = Color.red;

            if (motionController != null)
            {
                string turn = (motionController.howToMotion == MotionLibController.MotionMode.Motion7) ? "躯干转身模式" : "左右倾斜模式";
                string turnData = (motionController.howToMotion == MotionLibController.MotionMode.Motion7) ? motionController.TurnData.ToString() : "";
                GUI.Label(new Rect(20, startYOffset, 400, 80),
                    $"转向偏转度:{motionController.angle.ToString("0.00")}", labelStyle);
                GUI.Label(new Rect(20, startYOffset + 40, 400, 80),
                    $"转向判定为:{motionController.turnLeftOrRight}", labelStyle);
                GUI.Label(new Rect(20,startYOffset + 80, 400, 80),
                    $"转向速度为:{motionController.turnValue.ToString("0.00")}", labelStyle);
                if (GUI.Button(new Rect(20, startYOffset + 140, 300, 50), "当前转身模式:" + turn))
                {
                    motionController.SwitchTurnMode();
                }

                if (motionController.howToMotion == MotionLibController.MotionMode.Motion7)
                {
                    if (GUI.Button(new Rect(20, startYOffset + 200, 300, 50), "当前数据源:" + turnData))
                    {
                        motionController.SwitchTurnDataMode();
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

            startYOffset = 660;
            if(GUI.Button(new Rect(20, startYOffset - 40, 200, 30), $"使用最新OS"))
            {
                WalkActionItem.useRealtimeData = true;
            }

            if(GUI.Button(new Rect(20, startYOffset, 200, 30), $"使用之前OS"))
            {
                WalkActionItem.useRealtimeData = false;
            }

            GUI.Label(new Rect(20, startYOffset + 160, 300, 50), $"速度缩放倍数 ");

            var speedScaleStr = GUI.TextField(new Rect(20, startYOffset + 180, 200, 20), standTravelModelManager.GetRunSpeedScale().ToString());
            var speedScale = 0f;
            if(float.TryParse(speedScaleStr, out speedScale) && speedScale != standTravelModelManager.GetRunSpeedScale())
            {
                dirty = true;
                standTravelModelManager.SetRunSpeedScale(speedScale);
            }

            GUI.Label(new Rect(20, startYOffset + 200, 310, 50), $"冲刺速度缩放倍数 ");

            var sprintSpeedScaleStr = GUI.TextField(new Rect(20, startYOffset + 220, 200, 20), standTravelModelManager.GetSprintSpeedScale().ToString());
            var sprintSpeedScale = 0f;
            if(float.TryParse(sprintSpeedScaleStr, out sprintSpeedScale) && sprintSpeedScale != standTravelModelManager.GetSprintSpeedScale())
            {
                dirty = true;
                standTravelModelManager.SetSprintSpeedScale(sprintSpeedScale);
            }

            var useFrequencyCur = standTravelModelManager.GetUseFrequency();
            var useSpeedCur = !standTravelModelManager.GetUseFrequency();
            var useFrequency = GUI.Toggle(new Rect(230, startYOffset + 80, 100, 50), useFrequencyCur, "使用步频切换");
            var useSpeed = GUI.Toggle(new Rect(230, startYOffset + 100, 100, 50), useSpeedCur, "使用速度切换");

            if(useFrequency != useFrequencyCur || useSpeed != useSpeedCur)
            {
                dirty = true;
                standTravelModelManager.SetUseFrequency(useFrequency);
            }

            if(!useSpeed && useFrequency)
            {
                GUI.Label(new Rect(20, startYOffset + 70, 310, 50), $"走跑切换阈值 ");
                var runThreholdStr = GUI.TextField(new Rect(20, startYOffset + 90, 90, 20), standTravelModelManager.GetRunThrehold().ToString());
                var runThrehold = 0f;
                if(float.TryParse(runThreholdStr, out runThrehold) && runThrehold != standTravelModelManager.GetRunThrehold())
                {
                    dirty = true;
                    standTravelModelManager.SetRunThrehold(runThrehold);
                }

                GUI.Label(new Rect(120, startYOffset + 70, 310, 50), $"走跑切换阈值下沿 ");
                runThreholdStr = GUI.TextField(new Rect(130, startYOffset + 90, 90, 20), standTravelModelManager.GetRunThreholdLow().ToString());
                runThrehold = 0f;
                if(float.TryParse(runThreholdStr, out runThrehold) && runThrehold != standTravelModelManager.GetRunThreholdLow())
                {
                    dirty = true;
                    standTravelModelManager.SetRunThreholdLow(runThrehold);
                }
            }
            else
            {
                GUI.Label(new Rect(20, startYOffset + 70, 310, 50), $"速度阈值缩放倍数 * ");

                var runthrdScaleStr = GUI.TextField(new Rect(20, startYOffset + 90, 90, 20), standTravelModelManager.GetRunThresholdScale().ToString());
                var runthrdScale = 0f;
                if(float.TryParse(runthrdScaleStr, out runthrdScale) && runthrdScale != standTravelModelManager.GetRunThresholdScale())
                {
                    dirty = true;
                    standTravelModelManager.SetRunThresholdScale(runthrdScale);
                }

                GUI.Label(new Rect(20, startYOffset + 110, 310, 50), $"速度阈值缩放倍数下沿 * ");

                runthrdScaleStr = GUI.TextField(new Rect(20, startYOffset + 130, 90, 20), standTravelModelManager.GetRunThresholdScaleLow().ToString());
                runthrdScale = 0f;
                if(float.TryParse(runthrdScaleStr, out runthrdScale) && runthrdScale != standTravelModelManager.GetRunThresholdScaleLow())
                {
                    dirty = true;
                    standTravelModelManager.SetRunThresholdScaleLow(runthrdScale);
                }
            }

            GUI.Label(new Rect(20, startYOffset + 30, 310, 50), $"冲刺阈值 ");
            var sprintThreholdStr = GUI.TextField(new Rect(20, startYOffset + 50, 200, 20), standTravelModelManager.GetSprintThrehold().ToString());
            var sprintThrehold = 0f;
            if(float.TryParse(sprintThreholdStr, out sprintThrehold) && sprintThrehold != standTravelModelManager.GetSprintThrehold())
            {
                dirty = true;
                standTravelModelManager.SetSprintThrehold(sprintThrehold);
            }

            if(dirty && GUI.Button(new Rect(20, startYOffset + 250, 300, 30), $"保存"))
            {
                dirty = false;
                standTravelModelManager.SerializeParams();
            }*/
        }
    }
}