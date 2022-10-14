using MotionCaptureBasic.OSConnector;
using UnityEngine;

public class UnityWebSocketScript : MonoBehaviour
{
#if UNITY_ANDROID
    [SerializeField] private string webSocketUrl = "172.10.10.49"; //"ws://127.0.0.1:8181/";
    [SerializeField] private bool isUseJson;

    private int StartX = 50;

    private int StartY = 50;

    // Start is called before the first frame update
    void Start()
    {
        StartX = Screen.width - 200;
    }


    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle(GUIStyle.none);
        labelStyle.fontSize = 30;
        webSocketUrl = GUI.TextField(new Rect(StartX, StartY, 200, 40), webSocketUrl, labelStyle);
        if (GUI.Button(new Rect(StartX, StartY + 50, 200, 40), "连接WebSocket"))
        {
             HttpProtocolHandler.GetInstance().StartWebSocket(webSocketUrl, isUseJson);
             //HttpProtocolHandler.GetInstance().SetDebug(true);
        }
    }
#endif
}