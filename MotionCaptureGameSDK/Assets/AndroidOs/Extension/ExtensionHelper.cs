using UnityEngine;
namespace AndroidOs
{
    /// <summary>
    /// 对于Android扩展的调用
    /// </summary>
    public class ExtensionHelper
    {
#if UNITY_ANDROID  && !UNITY_EDITOR
        private static AndroidJavaClass _unityClass;

        private static AndroidJavaObject _unityActivity;
        private static AndroidJavaObject _pluginInstance;
        private static string            _pluginName = "com.fitplay.unitylibrary.PluginInstance";//TODO:need update
#endif
        /// <summary>
        /// 初始化插件
        /// </summary>
        public static void Initialize()
        {
#if UNITY_ANDROID  && !UNITY_EDITOR
            _unityClass     = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _unityActivity  = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _pluginInstance = new AndroidJavaObject(_pluginName);
            if(_pluginInstance == null)
            {
                Debug.Log("Plugin instance Error");
                return;
            }
            Debug.Log("Start Initialize");
            _pluginInstance.Call("ReceiveUnityActivity", _unityActivity);
#endif
        }

        /// <summary>
        /// 初始化Os通讯组件
        /// </summary>
        /// <param name="msgReciever">接收Usb返回消息的Gameobject,这个组件必须在根节点</param>
        public static void InitOsDataHandler(string msgReciever)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(_pluginInstance != null)
            {
                _pluginInstance.Call("StartOsHandler", msgReciever);
                Debug.Log($"Start OsHandler in Android Side:{msgReciever}");
            }
#endif
        }
        /// <summary>
        /// 发送数据给OS
        /// </summary>
        /// <param name="msg"></param>
        public static void SendDataToOS(string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(_pluginInstance != null)
            {
                _pluginInstance.Call("SendDataToOS", msg);
                Debug.Log("Close UsbHandler in Android Side");
            }
#endif
        }
        
        /// <summary>
        /// 订阅Action Detection数据
        /// </summary>
        public static void SubscribeActionDetection()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(_pluginInstance != null)
            {
                _pluginInstance.Call("SubscribeActionDetection");
                //Debug.Log($"Send Data SubscribeActionDetection!");
            }
#endif
        }
        
        /// <summary>
        /// 订阅Ground Location数据
        /// </summary>
        public static void SubscribeGroundLocation()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(_pluginInstance != null)
            {
                _pluginInstance.Call("SubscribeGroundLocation");
                //Debug.Log($"Send Data SubscribeGroundLocation!");
            }
#endif
        }
        
        
        /// <summary>
        /// 订阅Fitting数据
        /// </summary>
        public static void SubscribeFitting()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(_pluginInstance != null)
            {
                _pluginInstance.Call("SubscribeFitting");
                //Debug.Log($"Send Data SubscribeFitting!");
            }
#endif
        }

        /// <summary>
        /// Android层面的Toast
        /// </summary>
        /// <param name="message"></param>
        public static void ShowToast(string message)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(_pluginInstance != null)
            {
                _pluginInstance.Call("Toast", message);
            }
#endif
        }
    }
}