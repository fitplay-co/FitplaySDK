using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWebSocket.Server;

public class QRConnectionTest : WebSocketServerBase
{
    public QRCodeEncodeController e_qrController;
    public RawImage qrCodeImage;

    private string localIpAddress;

    private void Awake()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        localIpAddress = IPManager.GetLocalIPAddressWin();
#else
        localIpAddress = IPManager.GetLocalIPAddress();
#endif
        Debug.Log(localIpAddress);
    }

    private void Encode()
    {
        if (e_qrController != null)
        {
            int errorlog = e_qrController.Encode(localIpAddress);
            if (errorlog == -13)
            {
                Debug.LogError("Must contain 12 digits,the 13th digit is automatically added !");
            }
            else if (errorlog == -8)
            {
                Debug.LogError("Must contain 7 digits,the 8th digit is automatically added !");
            }
            else if (errorlog == -39)
            {
                Debug.LogError("Only support digits");
            }
            else if (errorlog == -128)
            {
                Debug.LogError("Contents length should be between 1 and 80 characters !");
            }
            else if (errorlog == -1)
            {
                Debug.LogError("Please select one code type !");
            }
            else if (errorlog == 0)
            {
                Debug.Log("Encode successfully !");
            }
        }
    }
}