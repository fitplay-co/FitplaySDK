
using System.Net.Sockets;
using MotionCaptureBasic.OSConnector;
using UnityEngine;
using UnityEngine.UI;
using UnityWebSocket.Server;
using UnityWebSocket.SocketServer;

public class QRConnectionTest : SocketServerBase
{
    public QRCodeEncodeController e_qrController;
    public RawImage qrCodeImage;
    public GameObject qrDisplayGroup;

    private bool isUseJson = false;
    private string localIpAddress;
    private bool isServerStart;

    private void Awake()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        localIpAddress = IPManager.GetLocalIPAddressWin();
#else
        localIpAddress = IPManager.GetLocalIPAddress();
#endif
        Debug.Log("local address: " + localIpAddress);
    }

    private void OnDestroy()
    {
        if (isServerStart)
        {
            Close();
        }

        PlayerPrefs.SetString(HttpProtocolHandler.OsIpKeyName, "");
    }

    public void OnOpenButton()
    {
        //打开画布，生成二维码，启动Websocket服务器
        if (qrDisplayGroup == null || qrCodeImage == null)
        {
            Debug.LogWarning("没有QR code显示相关ui组件");
            return;
        }

        qrDisplayGroup.SetActive(true);

        if (e_qrController != null)
        {
            e_qrController.onQREncodeFinished += QrEncodeFinished;
            Encode();
        }

        StartServer(Path, Port);
        
        isServerStart = true;
    }

    protected override void OnAccept(Socket client, string ip)
    {
        if (client == null)
        {
            Debug.LogError($"Client is invalid while accepted");
            return;
        }
        
        Debug.Log($"Client Accepted : {ip}");
        HttpProtocolHandler.GetInstance().StartWebSocket(ip, isUseJson);
        PlayerPrefs.SetString(HttpProtocolHandler.OsIpKeyName, ip);
    }

    void QrEncodeFinished(Texture2D tex)
    {
        if (tex != null)
        {
            qrCodeImage.texture = tex;
        }
    }

    public void OnCloseButton()
    {
        //关闭画布，关闭服务器
        if (qrDisplayGroup != null)
        {
            qrDisplayGroup.SetActive(false);
        }

        Close();
        isServerStart = false;
    }

    public void OnConnectOs()
    {
        if (PlayerPrefs.GetString(HttpProtocolHandler.OsIpKeyName) == "")
        {
            Debug.Log("OS IP为空，无法连接OS");
            return;
        }

        //读取PlayerPrefs的IP连接OS
        HttpProtocolHandler.GetInstance().StartWebSocket(PlayerPrefs.GetString(HttpProtocolHandler.OsIpKeyName), isUseJson);
    }

    private void Encode()
    {
        if (e_qrController == null)
        {
            return;
        }

        int errorlog = e_qrController.Encode(localIpAddress);
        switch (errorlog)
        {
            case -128:
            {
                Debug.LogError("Contents length should be between 1 and 80 characters !");
                break;
            }
            case -39:
            {
                Debug.LogError("Only support digits");
                break;
            }
            case -13:
            {
                Debug.LogError("Must contain 12 digits,the 13th digit is automatically added !");
                break;
            }
            case -8:
            {
                Debug.LogError("Must contain 7 digits,the 8th digit is automatically added !");
                break;
            }
            case -1:
            {
                Debug.LogError("Please select one code type !");
                break;
            }
            case 0:
            {
                Debug.Log("Encode successfully !");
                break;
            }
        }
    }

    //protected override void OnOpen(string userIp)
    //{
    //    Debug.Log($"当前连接的用户ip: {userIp}");
    //    PlayerPrefs.SetString(HttpProtocolHandler.OsIpKeyName, userIp);
    //}
}