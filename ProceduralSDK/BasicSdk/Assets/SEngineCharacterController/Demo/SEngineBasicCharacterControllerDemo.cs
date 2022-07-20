using SEngineBasic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityWebSocket;

namespace SEngineCharacterController
{
    public class SEngineBasicCharacterControllerDemo : MonoBehaviour
    {
        private WebsocketOSClient os;
        void Start()
        {
            os = new WebsocketOSClient();
            FitBar.Clear();
            os.ConnectAsync(state =>
            {
                if (state == WebSocketState.Open)
                {
                    os
                        .SubscribeApplicationClient()
                        .SubscribeGroundLocation(EOSActionType.Subscribe)
                        .SubscribeFitting(EOSActionType.Subscribe, EFittingType.Dual)
                        .SubscribeActionDetection(EOSActionType.Subscribe)
                        .SubscribeGazeTracking(EOSActionType.Subscribe)
                        .SetImuFPS();
                    //.HeartCommand(EHandleType.LeftHandle,EHeartCommandType.OpenHeartRate)
                    //.HeartCommand(EHandleType.RightHandle, EHeartCommandType.OpenBloodOxygen);
                }
            });
            

            var entity = Entity.Create<Entity>(1);
            inputComponent = entity.AddComponent<InputComponent>();

            entity.AddComponent<AttributeComponent>(CharacterType.Master,"huss");

            var modelGameObject = GameObject.Find("unitychan");
            entity.AddComponent<ModelComponent>(modelGameObject);
            entity.AddComponent<TransformComponent>();

            entity.AddComponent<FsmComponent>();
                
            entity.AddComponent<OutputComponent>();
            
            DontDestroyOnLoad(this);
        }

        private InputComponent inputComponent;
        private void Update()
        {
            Launcher.Instance.OnTick(Time.deltaTime);
            //   var message = os?.IMessage as IKBodyMessage;
            //   print($"message:{message == null} {JsonUtility.ToJson(message)}");
            if (inputComponent != null)
            {
                inputComponent.Message = os.IMessage;
            }
        }

        private void FixedUpdate()
        {
            Launcher.Instance.OnFixedTick(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            Launcher.Instance.OnLateTick(Time.deltaTime);
        }
        
        private void OnDisable()
        {
            os?.CloseAsync();
        }
    }
}

