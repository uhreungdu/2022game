using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Hive.Plugin;

namespace MyFirstPlugin
{
    enum EventCode : byte
    {
        Test = 0,
        SpawnPlayer,
        CreateItem,
        GetItem,
        RenewScore
    }


    class MyFirstPlugin : PluginBase
    {      
        private int variableTest = 0;
        private int[] score = new int[2];
        private float time = 300.0f;
        private string internalRoomName;
        public override string Name => "MyFirstPlugin";

        public override void OnCreateGame(ICreateGameCallInfo info)
        {
            PluginHost.LogInfo($"OnCreateGame {info.Request.GameId} by user {info.UserId}");
            internalRoomName = info.Request.GameId;
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
            if (info.Request.EvCode > System.Enum.GetValues(typeof(EventCode)).Length)
            {
                info.Continue();
                return;
            }

            object[] data = (object[])info.Request.Data;
            switch (info.Request.EvCode)
            {
                // 테스트용
                case (byte)EventCode.Test:
                    info.Request.Data = new object[] { "이 이벤트는", "몰?루가 지배했다", variableTest, "HOOK", "TEST" };
                    break;

                // 점수갱신
                case (byte)EventCode.RenewScore:                   
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

        public override void OnLeave(ILeaveGameCallInfo info)
        {
            base.OnLeave(info);
        }

        public override void OnCloseGame(ICloseGameCallInfo info)
        {
            HttpRequest request = new HttpRequest()
            {
                Callback = OnHttpResponse,
                Url = "http://127.0.0.1/room_delete.php",
                Async = true,
                CustomHeaders = new Dictionary<string, string>
                {
                    {"\"iname\"", "\""+internalRoomName+"\""}
                }
            };
            PluginHost.HttpRequest(request, info);
        }

        private void OnHttpResponse(IHttpResponse response, object userState)
        {
            ICallInfo info = response.CallInfo;

        }
    }

}
