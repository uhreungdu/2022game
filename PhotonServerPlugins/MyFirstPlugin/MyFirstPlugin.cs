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
        public override string Name => "MyFirstPlugin";

        public override void OnCreateGame(ICreateGameCallInfo info)
        {
            PluginHost.LogInfo($"OnCreateGame {info.Request.GameId} by user {info.UserId}");
            info.Continue(); // base.OnCreateGame(info) 와 같다.
        }
    }
}
