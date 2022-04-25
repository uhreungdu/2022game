﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Photon.Hive.Plugin;

namespace MyFirstPlugin
{
    enum EventCode : byte
    {
        Test = 0,
        SpawnPlayer,
        StartGame,
        SetTeamOnServer
    }

    class PlayerInfo
    {
        public string name { get; set; }
        public int team { get; set; }
    }


    class MyFirstPlugin : PluginBase
    {
        private List<PlayerInfo> playerInfo = new List<PlayerInfo>();
        private int[] score = new int[2];
        private float time = 300.0f;
        private string internalRoomName;
        private bool inGame = false;
        public override string Name => "MyFirstPlugin";

        public override void OnCreateGame(ICreateGameCallInfo info)
        {
            playerInfo.Add(new PlayerInfo() { name = info.Nickname, team = 0 });
            PluginHost.LogInfo($"OnCreateGame {info.Request.GameId} by user {info.UserId}");
            internalRoomName = info.Request.GameId;
            info.Continue(); // base.OnCreateGame(info) 와 같다.
        }
        public override void OnJoin(IJoinGameCallInfo info)
        {
            if (!inGame)
            {
                playerInfo.Add(new PlayerInfo() { name = info.Nickname, team = -1 });
            }
            PluginHost.LogInfo($"User {info.Nickname} join room {internalRoomName}");
            base.OnJoin(info);
        }

        public override void OnLeave(ILeaveGameCallInfo info)
        {
            if (!inGame)
            {
                playerInfo.Remove(playerInfo.Find(x => x.name == info.Nickname));
            }
            PluginHost.LogInfo($"User {info.Nickname} exit room {internalRoomName}");
            base.OnLeave(info);
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
                    info.Request.Data = new object[] { "이 이벤트는", "몰?루가 지배했다", 555, "HOOK", "TEST" };
                    break;

                // 게임 시작
                case (byte)EventCode.StartGame:
                    StartGame(info);
                    break;
               // 플레이어 정보 세팅
                case (byte)EventCode.SetTeamOnServer:
                    var index = playerInfo.FindIndex(x => x.name == (string)data[0]);
                    var tempPlayerinfo = playerInfo[index];
                    tempPlayerinfo.team=(int)data[1];
                    playerInfo[index] = tempPlayerinfo; 
                    break;

                default:
                    break;
            }

            info.Continue();
        }

        public override void OnCloseGame(ICloseGameCallInfo info)
        {
            string url = "http://127.0.0.1/room_delete.php?iname=" + "\"" + internalRoomName + "\"";
            HttpRequest request = new HttpRequest()
            {
                Callback = OnHttpResponse,
                Url = url,
                Async = true
            };
            PluginHost.HttpRequest(request, info);
        }

        private void StartGame(IRaiseEventCallInfo info)
        {
            inGame = true;
            string url = "http://127.0.0.1/game_start.php?iname=" + "\"" + internalRoomName + "\"";
            HttpRequest request = new HttpRequest()
            {
                Callback = OnHttpResponse,
                Url = url,
                Async = true
            };
            PluginHost.HttpRequest(request, info);
        }

        private void OnHttpResponse(IHttpResponse response, object userState)
        {
            ICallInfo info = response.CallInfo;

        }
    }

}
