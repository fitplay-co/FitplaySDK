using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace UnityWebSocket.Server
{
    public class WebSocketServerBehavior : WebSocketBehavior
    {
        private SynchronizationContext context;
 
        private OnOpenDelegate onOpen;
        private OnMessageDelegate onMessage;
        private OnMessageBytesDelegate onMessageBytes;
        private OnCloseDelegate onClose;
    
        public delegate void OnOpenDelegate();
        public delegate void OnMessageDelegate(string data);
        public delegate void OnMessageBytesDelegate(byte[] data);
        public delegate void OnCloseDelegate();
    
        public void SetContext(SynchronizationContext context, 
            OnOpenDelegate onOpenDelegate, 
            OnMessageDelegate onMessageDelegate, 
            OnMessageBytesDelegate onMessageBytesDelegate, 
            OnCloseDelegate onCloseDelegate)
        {
            this.context = context;
            this.onOpen = onOpenDelegate;
            this.onMessage = onMessageDelegate;
            this.onMessageBytes = onMessageBytesDelegate;
            this.onClose = onCloseDelegate;
        }
 
        protected override void OnOpen()
        {
            context?.Post(_ =>
            {
                onOpen?.Invoke();
            }, null);
        }
 
        protected override void OnMessage (WebSocketSharp.MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                context?.Post(_ =>
                {
                    onMessageBytes?.Invoke(e.RawData);
                }, null);
            }
            else if (e.IsText)
            {
                context?.Post(_ =>
                {
                    onMessage?.Invoke(e.Data);
                }, null);
            }
        }
 
        protected override void OnClose (WebSocketSharp.CloseEventArgs e)
        {
            context?.Post(_ =>
            {
                onClose?.Invoke();
            }, null);
        }
    }
}