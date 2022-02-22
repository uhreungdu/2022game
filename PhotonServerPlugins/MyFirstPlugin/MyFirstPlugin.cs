using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Hive.Plugin;

namespace MyFirstPlugin
{
    class MyFirstPlugin : PluginBase
    {   
        enum EventCode : byte
        {
            Test = 0,
            RenewScore
        }

        private int variableTest = 0;
        private int[] score = new int[2];
        public override string Name => "MyFirstPlugin";

        public override void OnCreateGame(ICreateGameCallInfo info)
        {
            HttpRequest request = new HttpRequest()
            {
                Callback = OnHttpResponse,
                Url = "http://127.0.0.1/test.php",
                Async = true
            };
            PluginHost.HttpRequest(request, info);

            PluginHost.LogInfo($"OnCreateGame {info.Request.GameId} by user {info.UserId}");
            info.Continue(); // base.OnCreateGame(info) 와 같다.
        }
        public override void OnJoin(IJoinGameCallInfo info)
        {
            variableTest++;
            base.OnJoin(info);
        }

        public override void OnRaiseEvent(IRaiseEventCallInfo info)
        {
            PluginHost.LogInfo($"user {info.UserId} called event {info.Request.EvCode}");

            switch (info.Request.EvCode)
            {
                case (byte)EventCode.Test:
                    info.Request.Data = new object[] { "이 이벤트는", "몰?루가 지배했다", variableTest, "HOOK", "TEST" };
                    break;

                case (byte)EventCode.RenewScore:
                    object[] data = (object[])info.Request.Data;
                    int team = (int)data[0];
                    int point = (int)data[1];
                    score[team] = score[team] + point;
                    info.Request.Data = new object[] { score[0], score[1] };
                    break;

                default:
                    break;
            }

            info.Continue();
        }

        private void OnHttpResponse(IHttpResponse response, object userState)
        {
            ICallInfo info = response.CallInfo;

        }
    }

}
