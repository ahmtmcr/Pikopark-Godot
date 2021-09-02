using Godot;
using System;
using Steamworks;
using FlatBuffers;

public class levelPacketReceiver : Node
{
   
    protected Callback<P2PSessionRequest_t> Callback_P2PSessionRequest;
    protected Callback<P2PSessionConnectFail_t>  Callback_P2PSessionConnectFailed;
   
   
   
    RigidBody2D platformLocal;
    AnimatedSprite buttonAnim;


     Global global;
    public override void _Ready()
    {
        platformLocal = GetParent().GetNode("platform") as RigidBody2D;
        buttonAnim =  GetParent().GetNode("button/AnimatedSprite") as AnimatedSprite;
        global = GetNode("/root/Global") as Global;

        Callback_P2PSessionRequest = Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
        Callback_P2PSessionConnectFailed = Callback<P2PSessionConnectFail_t>.Create(OnP2PSessionConnectFailed);
        
    }

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
  
  public override void _Process(float delta)
  {
      while(SteamNetworking.IsP2PPacketAvailable(out uint packetSize))
         {
             byte[] incomingPacket = new byte[packetSize];

            if(SteamNetworking.ReadP2PPacket(incomingPacket, packetSize, out uint bytesRead, out CSteamID remoteID ))
            {
                ByteBuffer buff = new ByteBuffer(incomingPacket);
                var platform = NetworkPacket.platform.GetRootAsplatform(buff);

                switch(platform.Action)
                {
                    case 1:
                        transformPlatform(platform);
                        break;
                    case 2:
                        break;
                }
            }
         }
  
  }

  private void transformPlatform(NetworkPacket.platform platform)
  {
      NetworkPacket.Vec2 pos = platform.Pos.Value;
      var anim = platform.Anim;

      platformLocal.Transform = new Transform2D(0, new Vector2(pos.X, pos.Y));
      buttonAnim.Animation = anim;
  }
}
