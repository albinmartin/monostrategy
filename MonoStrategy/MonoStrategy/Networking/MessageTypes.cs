using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Networking
{
    /*
     * A request is sent by the client.
     * A request is received by the server.
     *
     * A command is sent by the server.
     * A command is received by the client.
     * /

    /*Request format:
     * int32:int32:int32:String
     * CommandType:ClientID:Lockstep:DATA
     */

    /*Command format:
     * int32:int32:int32:String
     * CommandType:ClientID:Lockstep:DATA
     */


    public enum MessageTypes
    {
        Default,
        Maintenance,
        InGame
    }

    public enum MaintenanceCommandTypes
    {
        Chat,
        JoinGame,
        Lockstep,
        LeaveGame,
        StartGame,
        ChangeSeed
    }

    public enum GameCommandTypes
    {
        Chat,
        SpawnAgent,
        MoveAgent
    }
}
