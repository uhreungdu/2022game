using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Photon.Hive.Plugin;

namespace MyFirstPlugin
{
    enum EventType : byte
    {
        Test = 0,
        SpawnPlayer,
        StartGame,
        SetTeamOnServer,
        RespawnForReconnect,
        CreateBuildingFromClient,
        DestroyBuildingFromClient,
        CreateBuildingFromServer,
        DestroyBuildingFromServer,
        HideBuildingFragments
    }

    class PlayerInfo
    {
        public string Name { get; set; }
        public int Team { get; set; }

        public bool isConnected = true;

        public float[] Position;

        public int coin { get; set; }
    }

    class Building
    {
        public float[] Position;
        public float[] Rotate;
        public float RespawnTime { get; set; }
        public float Timer { get; set; }
        public string Type { get; set; }

        public bool Dead = false;
    }


    class MyFirstPlugin : PluginBase
    {
        public static MyFirstPlugin Instance = new MyFirstPlugin();

        private List<PlayerInfo> playerInfo = new List<PlayerInfo>();
        private Dictionary<int, Building> buildings = new Dictionary<int, Building>();
        private int[] score = new int[2];
        private string internalRoomName;
        private bool inGame = false;
        public override string Name => "MyFirstPlugin";

        public override void OnCreateGame(ICreateGameCallInfo info)
        {
            playerInfo.Add(new PlayerInfo() { Name = info.Nickname, Team = 0, Position = new float[3] });
            PluginHost.LogInfo($"OnCreateGame {info.Request.GameId} by user {info.UserId}");
            internalRoomName = info.Request.GameId;
            info.Continue(); // base.OnCreateGame(info) 와 같다.
        }
        public override void OnJoin(IJoinGameCallInfo info)
        {
            if (inGame)
            {
                ReConnectPlayer(info);
            }
            else
            {
                playerInfo.Add(new PlayerInfo() { Name = info.Nickname, Team = -1, Position = new float[3] });
            }
            PluginHost.LogInfo($"User {info.Nickname} join room {internalRoomName}");
            base.OnJoin(info);
        }

        private void ReConnectPlayer(IJoinGameCallInfo info)
        {
            byte evCode = (byte)EventType.RespawnForReconnect;
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            byte index = 0;
            data.Add(index, info.Nickname);
            index++;
            data.Add(index, playerInfo.Find(x => x.Name == info.Nickname).Team);
            index++;
            foreach (KeyValuePair<int, Building> kvp in buildings)
            {
                if (!kvp.Value.Dead) continue;
                data.Add(index, kvp.Key);
                index++;
            }
            BroadcastEvent(evCode, data);
        }

        public override void OnLeave(ILeaveGameCallInfo info)
        {
            SerializableGameState d = PluginHost.GetSerializableGameState();
            if (inGame)
            {
                
            }
            else
            {
                playerInfo.Remove(playerInfo.Find(x => x.Name == info.Nickname));
            }
            PluginHost.LogInfo($"User {info.Nickname} exit room {internalRoomName}");
            base.OnLeave(info);
        }

        public override void OnRaiseEvent(IRaiseEventCallInfo info)
        {
            PluginHost.LogInfo($"user {info.UserId} called event {info.Request.EvCode}");
            if (info.Request.EvCode > System.Enum.GetValues(typeof(EventType)).Length)
            {
                info.Continue();
                return;
            }

            object[] data = (object[])info.Request.Data;
            switch (info.Request.EvCode)
            {
                // 테스트용
                case (byte)EventType.Test:
                    info.Request.Data = new object[] { "이 이벤트는", "몰?루가 지배했다", 555, "HOOK", "TEST" };
                    break;

                // 게임 시작
                case (byte)EventType.StartGame:
                    {
                        StartGame(info);
                        break;
                    }
                case (byte)EventType.SetTeamOnServer:
                    {
                        var index = playerInfo.FindIndex(x => x.Name == (string)data[0]);
                        var tempPlayerinfo = playerInfo[index];
                        tempPlayerinfo.Team = (int)data[1];
                        playerInfo[index] = tempPlayerinfo;
                        break;
                    }
                case (byte)EventType.CreateBuildingFromClient:
                    {
                        RcvBuildingInfo(info);
                        break;
                    }
                case (byte)EventType.DestroyBuildingFromClient:
                    {
                        SetDestroyBuildingTimer(info);
                        break;
                    }             
                default:
                    break;
            }

            info.Continue();
        }

        public override void OnCloseGame(ICloseGameCallInfo info)
        {
            GC.Collect();
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

        private void RcvBuildingInfo(IRaiseEventCallInfo info)
        {
            object[] data = (object[])info.Request.Data;
            var obj = new Building { };
            int index = 1;

            obj.Type = (string)data[index];
            index += 1;
            obj.Position = new float[3] { (float)data[index], (float)data[index + 1], (float)data[index + 2] };
            index += 3;
            obj.Rotate = new float[4] { (float)data[index], (float)data[index + 1], (float)data[index + 2], (float)data[index + 3] };
            index += 4;
            obj.RespawnTime = (float)data[index];

            buildings.Add((int)data[0], obj);
        }

        private void SetDestroyBuildingTimer(IRaiseEventCallInfo info)
        {
            object[] data = (object[])info.Request.Data;
            int key = (int)data[0];
            // 오브젝트 죽은상태로 변경
            buildings[key].Dead = true;

            // 오브젝트 쿨타임 찾아서 그 시간 지나면 없애고 새거 나오라고 신호보냄
            var target = buildings[key];
            var timer = new Timer(DestroyBuilding, key, (int)(target.RespawnTime * 1000), System.Threading.Timeout.Infinite);

            // 약 5초뒤 파편 숨기기 이벤트 전송
            var timer2 = new Timer(HideBuildingFragments, key, 5000, System.Threading.Timeout.Infinite);
        }

        private void DestroyBuilding(Object sender)
        {
            CreateBuildingFromServer((int)sender);
            DestroyBuildingFromServer((int)sender);
            
        }
        private void DestroyBuildingFromServer(int viewID)
        {
            buildings.Remove(viewID);
            byte evCode = (byte)EventType.DestroyBuildingFromServer;
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(0, viewID);
            BroadcastEvent(evCode, data);
        }
        private void CreateBuildingFromServer(int key)
        {
            var target = buildings[key];
            byte evCode = (byte)EventType.CreateBuildingFromServer;
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(0, target.Type);
            data.Add(1, target.Position[0]);
            data.Add(2, target.Position[1]);
            data.Add(3, target.Position[2]);
            data.Add(4, target.Rotate[0]);
            data.Add(5, target.Rotate[1]);
            data.Add(6, target.Rotate[2]);
            data.Add(7, target.Rotate[3]);
            BroadcastEvent(evCode, data);
        }

        private void HideBuildingFragments(Object sender)
        {
            byte evCode = (byte)EventType.HideBuildingFragments;
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(0, (int)sender);
            BroadcastEvent(evCode, data);
        }

        private void OnHttpResponse(IHttpResponse response, object userState)
        {
            ICallInfo info = response.CallInfo;

        }
    }

}
