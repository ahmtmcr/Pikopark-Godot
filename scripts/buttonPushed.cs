using Godot;
using System;
using Steamworks;
using FlatBuffers;

public class buttonPushed : Area2D
{
    
    float PlatformSpeed;
    
    
    KinematicBody2D player1;
    KinematicBody2D player2;

    AnimatedSprite buttonAnim;
    
    
    RigidBody2D platform;
    Vector2 oldPos;
    
    Global global;


    bool sendPacketReady;
    
    public override void _Ready()
    {
        player1 = GetParent().GetNode("game/player1") as KinematicBody2D;
        player2 = GetParent().GetNode("game/player2") as KinematicBody2D;
        platform = GetParent().GetNode("platform") as RigidBody2D;
        buttonAnim = GetNode("AnimatedSprite") as AnimatedSprite;
        oldPos = platform.Position; 
        global = GetNode("/root/Global") as Global;
    }

  public override void _Process(float delta)
  {
      
      PlatformSpeed += delta * 0.009f;
      
      buttonAnim.Play("idle");
      if(OverlapsBody(player1))
      {
          sendPacketReady = true;
          buttonAnim.Play("pushed");
          platform.Position = platform.Position.LinearInterpolate(new Vector2(368,336), PlatformSpeed);

      }
     
      
      else if(OverlapsBody(player2))
      {
          sendPacketReady = true;
          buttonAnim.Play("pushed");
          platform.Position = platform.Position.LinearInterpolate(new Vector2(368,336), PlatformSpeed);
          
          
      }
      else 
      {
           platform.Position = platform.Position.LinearInterpolate(oldPos, PlatformSpeed);
           sendPacketReady = false;
      }
        transferPlatformInfo();
  
  }

  public void transferPlatformInfo()
  {
       if ( sendPacketReady )
        {
            // Create animationState new packet with movement data
            FlatBufferBuilder builder = new FlatBufferBuilder(8);
            var x = builder.CreateString(buttonAnim.Animation);
            
            
            
            NetworkPacket.platform.Startplatform(builder);

            
            

            if (global.playingAsHost)
            {
                NetworkPacket.platform.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, platform.Transform.origin.x, platform.Transform.origin.y));
                NetworkPacket.platform.AddAnim(builder,x);
                
            }
                
                
                
            else
            {
                NetworkPacket.platform.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, platform.Transform.origin.x, platform.Transform.origin.y));
                NetworkPacket.platform.AddAnim(builder, x);
               
            }
                

            NetworkPacket.platform.AddAction(builder, 1);
            var stopBuilding = NetworkPacket.platform.Endplatform(builder);
            builder.Finish(stopBuilding.Value);

            // Convert the Flatbuffer into animationState byte array
            byte[] packet = builder.SizedByteArray();

            // Send packet through Steam, destination according to who you're playing as
            if (global.playingAsHost)
                SteamNetworking.SendP2PPacket(global.player2, packet, (uint)packet.Length, EP2PSend.k_EP2PSendUnreliable);
            else
                SteamNetworking.SendP2PPacket(global.player1, packet, (uint)packet.Length, EP2PSend.k_EP2PSendUnreliable);
        }
        sendPacketReady = false;
  }

 
}
