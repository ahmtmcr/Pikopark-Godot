using Godot;
using System;
using Steamworks;
using FlatBuffers;

public class packetReceiver : Node
{
  
    protected Callback<P2PSessionRequest_t> Callback_P2PSessionRequest;
    protected Callback<P2PSessionConnectFail_t>  Callback_P2PSessionConnectFailed;

    KinematicBody2D player1;
    KinematicBody2D player2;


    Global global;
  
  
    public override void _Ready()
    {
        
        global = GetNode("/root/Global") as Global;
        
        Callback_P2PSessionRequest = Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
        Callback_P2PSessionConnectFailed = Callback<P2PSessionConnectFail_t>.Create(OnP2PSessionConnectFailed);
        
        player1 = GetParent().GetParent().GetNode("player1") as KinematicBody2D;
        player2 = GetParent().GetParent().GetNode("player2") as KinematicBody2D;  

<<<<<<< Updated upstream
=======
        player1Anim = GetParent().GetParent().GetNode("player1/Sprite") as AnimatedSprite;
        player2Anim = GetParent().GetParent().GetNode("player2/Sprite") as AnimatedSprite;

>>>>>>> Stashed changes

    }

    // ---------------------------ACCEPT OR REJECT INCOMING CONNECTION-----------------------------
    public void OnP2PSessionRequest(P2PSessionRequest_t request)
    {
        if (request.m_steamIDRemote == global.player1 || request.m_steamIDRemote == global.player2)
        {
            SteamNetworking.AcceptP2PSessionWithUser(request.m_steamIDRemote);
            GD.Print("You have accepted incoming connection from " + SteamFriends.GetFriendPersonaName(request.m_steamIDRemote));
        }
        else
        {
            GD.Print("A connection was just rejected from " + request.m_steamIDRemote + ".");
        }
    }

     public void OnP2PSessionConnectFailed(P2PSessionConnectFail_t failure) => GD.Print("P2P session failed. Error Code: " + failure.m_eP2PSessionError);

     //-------------------------RECEIVE PACKETS-----------------------------------------------------
     public override void _Process(float delta)
     {
         while(SteamNetworking.IsP2PPacketAvailable(out uint packetSize))
         {
             byte[] incomingPacket = new byte[packetSize];

            if(SteamNetworking.ReadP2PPacket(incomingPacket, packetSize, out uint bytesRead, out CSteamID remoteID ))
            {
                ByteBuffer buff = new ByteBuffer(incomingPacket);
                var OtherPlayer = NetworkPacket.OtherPlayer.GetRootAsOtherPlayer(buff);

                switch(OtherPlayer.Action)
                {
                    case 1:
                        moveOtherPlayer(OtherPlayer);
                        break;
                    case 2:
                        break;
                }
            }
         }
     }

     private void moveOtherPlayer(NetworkPacket.OtherPlayer otherPlayer)
     {
         NetworkPacket.Vec2 pos = otherPlayer.Pos.Value;
<<<<<<< Updated upstream
=======
         var animationState = otherPlayer.Anim;
         bool playerLookDirection = otherPlayer.Direction;
         
         
         
         
>>>>>>> Stashed changes

         if(global.playingAsHost)
         {
             player2.Transform = new Transform2D(0, new Vector2(pos.X, pos.Y));
<<<<<<< Updated upstream
=======
             player2Anim.Animation = animationState;
             player2Anim.FlipH = playerLookDirection;
>>>>>>> Stashed changes
         }
         else
         {
             player1.Transform = new Transform2D(0, new Vector2(pos.X, pos.Y));
<<<<<<< Updated upstream
=======
             player1Anim.Animation = animationState;
             player1Anim.FlipH = playerLookDirection;
>>>>>>> Stashed changes
         }
     }
    
}
