﻿#region Using

using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Serializer;
using Sandbox.Common.ObjectBuilders.VRageData;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using SteamSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using VRage.Library.Utils;
using VRageMath;

#endregion

namespace Sandbox.Game.Multiplayer
{
    [PreloadRequired]
    static class MySyncThrower
    {
        [ProtoContract]
        [MessageId(11889, P2PMessageEnum.Reliable)]
        struct ThrowMsg
        {
            [ProtoMember(1)]
            public MyObjectBuilder_CubeGrid Grid;
            [ProtoMember(2)]
            public Vector3D Position;
            [ProtoMember(3)]
            public Vector3D LinearVelocity;
            [ProtoMember(4)]
            public float Mass;
            [ProtoMember(5)]
            public MyStringId ThrowSound;
        }


        static MySyncThrower()
        {
            MySyncLayer.RegisterMessage<ThrowMsg>(OnThrowMessageRequest, MyMessagePermissions.ToServer, MyTransportMessageEnum.Request);
            MySyncLayer.RegisterMessage<ThrowMsg>(OnThrowMessageSuccess, MyMessagePermissions.FromServer, MyTransportMessageEnum.Success);
        }

        public static void RequestThrow(MyObjectBuilder_CubeGrid grid, Vector3D position, Vector3D linearVelocity, float mass, MyStringId throwSound)
        {
            ThrowMsg msg = new ThrowMsg();
            msg.Grid = grid;
            msg.Position = position;
            msg.LinearVelocity = linearVelocity;
            msg.Mass = mass;
            msg.ThrowSound = throwSound;
            MySession.Static.SyncLayer.SendMessageToServer(ref msg);
        }

        static void OnThrowMessageRequest(ref ThrowMsg msg, MyNetworkClient sender)
        {
            MySession.Static.SyncLayer.SendMessageToAllAndSelf(ref msg, MyTransportMessageEnum.Success);
        }

        static void OnThrowMessageSuccess(ref ThrowMsg msg, MyNetworkClient sender)
        {
            MySessionComponentThrower.Static.Throw(msg.Grid, msg.Position, msg.LinearVelocity, msg.Mass, msg.ThrowSound);
        }
    }
}
