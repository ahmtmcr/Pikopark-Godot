using Godot;
using System;
using Steamworks;
using FlatBuffers;


public class playerMovement : KinematicBody2D
{
  
   
    int speed = 150;
    float gravity = 500f;
    bool sendPacketReady;
    Vector2 velocity;
    KinematicBody2D player1;
    KinematicBody2D player2;

    AnimatedSprite player1Anim;
    AnimatedSprite player2Anim;
    
    Timer sendPacketTimer;
    AnimatedSprite playerSprite;

    string animationState;
    bool playerLookDirection;


    

    
    

    
    
    


    Global global;
    
    public override void _Ready()
    {
       
      
        
        
        global = GetNode("/root/Global") as Global;

        player1 = GetParent().GetNode("player1") as KinematicBody2D;
        player2 = GetParent().GetNode("player2") as KinematicBody2D;
        player1Anim = GetParent().GetNode("player1/Sprite") as AnimatedSprite;
        player2Anim = GetParent().GetNode("player2/Sprite") as AnimatedSprite;
        sendPacketTimer = GetParent().GetNode("sendPacketTimer") as Timer;
        playerSprite = GetNode("Sprite") as AnimatedSprite;
      
        
       
       

        sendPacketTimer.Connect("timeout", this, "_on_timeout");
        
    
    }

    
   
   //-------------------LOCAL MOVEMENT--------------------


     public override void _PhysicsProcess(float delta) 
    {
        
       
        
        animationState = playerSprite.Animation;
        playerLookDirection = playerSprite.FlipH;
       
       
        velocity.y += delta * gravity;

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x = -speed;
            playerSprite.FlipH = true;
            if(IsOnFloor())
            {
                playerSprite.Play("walk");
                
            }
           
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            velocity.x = speed;
            playerSprite.FlipH = false;
            
            if(IsOnFloor())
            {
                playerSprite.Play("walk");
            }
        }
        else
        {
            velocity.x = 0;
            if(IsOnFloor())
            {
                playerSprite.Play("idle");
            }
            
        }

        if(IsOnFloor() && Input.IsActionPressed("ui_up"))
        {
            velocity.y = -200;
            playerSprite.Play("jump");
        }
        if(!IsOnFloor())
        {
            speed = 100;
        }
        else
        {
            speed = 200;
        }

       
        MoveAndSlide(velocity, new Vector2(0, -1));
       
        
        
        
    }

    

 



    
    
   
    

    
    
    
     //-------------------NETWORKED MOVEMENT--------------------
  public override void _Process(float delta) 
  {
      transferPlayerMovement();
      
      
   
      
      
  }
     
     
  private void transferPlayerMovement()
    {
       
        if ( sendPacketReady )
        {
            // Create animationState new packet with movement data
            FlatBufferBuilder builder = new FlatBufferBuilder(8);
            var x = builder.CreateString(animationState);
            
            
            
            NetworkPacket.OtherPlayer.StartOtherPlayer(builder);

            
            

            if (global.playingAsHost)
            {
                NetworkPacket.OtherPlayer.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, player1.Transform.origin.x, player1.Transform.origin.y));
                NetworkPacket.OtherPlayer.AddAnim(builder,x);
                NetworkPacket.OtherPlayer.AddDirection(builder, playerLookDirection);
            }
                
                
                
            else
            {
                NetworkPacket.OtherPlayer.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, player2.Transform.origin.x, player2.Transform.origin.y));
                NetworkPacket.OtherPlayer.AddAnim(builder, x);
                NetworkPacket.OtherPlayer.AddDirection(builder, playerLookDirection);
            }
                

            NetworkPacket.OtherPlayer.AddAction(builder, 1);
            var stopBuilding = NetworkPacket.OtherPlayer.EndOtherPlayer(builder);
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

    private void _on_timeout() => sendPacketReady = true;
   
  
   
    
}
