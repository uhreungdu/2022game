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
        RenewScore,
        CreateItem
    }

    class ItemBox
    {
        private bool Activate;
        private int Type;

        public ItemBox()
        {
            Activate = false;
            Type = 0;
        }

        public bool GetActivate() { return Activate; }
        public void SetActivate(bool Value) { Activate = Value; }

        public int GetItemType() { return Type; }
        public void SetItemType(int Value) { Type = Value; }
    }

    class MyFirstPlugin : PluginBase
    {      
        private int variableTest = 0;
        private int[] score = new int[2];
        private List<ItemBox> items = new List<ItemBox>();
        private float time = 300.0f;
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

            // 아이템 하나 생성
            items.Insert(0, new ItemBox());

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
                // 테스트용
                case (byte)EventCode.Test:
                    info.Request.Data = new object[] { "이 이벤트는", "몰?루가 지배했다", variableTest, "HOOK", "TEST" };
                    break;

                // 점수갱신
                case (byte)EventCode.RenewScore:
                    object[] RSdata = (object[])info.Request.Data;
                    int team = (int)RSdata[0];
                    int point = (int)RSdata[1];
                    score[team] = score[team] + point;
                    info.Request.Data = new object[] { score[0], score[1] };
                    break;

                // 아이템 생성
                case (byte)EventCode.CreateItem:
                    object[] CIdata = (object[])info.Request.Data;
                    int type = (int)CIdata[0];
                    bool result = (bool)CIdata[1];
                    if (!items[0].GetActivate())
                    {
                        items[0].SetActivate(true);
                        items[0].SetItemType(type);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    info.Request.Data = new object[] { type, result };
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
