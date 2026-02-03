#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System;
using System.Collections;
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebSocketOperation : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get
            {
                if (item == null)
                {
                    return false;
                }
                
                if (item.isDone == false)
                {
                    return true;
                }

                return false;
            }
        }

        private WebSocket.RequestQueueItem item;
            
        public WebSocketOperation(WebSocket.RequestQueueItem item)
        {
            this.item = item;
        }
    }
}
#endif