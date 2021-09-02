using Godot;
using System;
using Steamworks;
using FlatBuffers;


public class playerMovement : KinematicBody2D
{
  
    
    int speed = 200;
    bool sendPacketReady;
    Vector2 velocity;
    KinematicBody2D player1;
    KinematicBody2D player2;
    Timer sendPacketTimer;
<<<<<<< Updated upstream
=======
    AnimatedSprite playerSprite;

    string animationState;
    bool playerLookDirection;


    

    
    

    
    
    

>>>>>>> Stashed changes

    Global global;
    
    public override void _Ready()
    {
        global = GetNode("/root/Global") as Global;

<<<<<<< Updated upstream
        player1 = GetNode("/root/game/player1") as KinematicBody2D;
        player2 = GetNode("/root/game/player2") as KinematicBody2D;
        sendPacketTimer = GetNode("/root/game/sendPacketTimer") as Timer;

        sendPacketTimer.Connect("timeout", this, "_on_timeout");
=======
        player1 = GetParent().GetNode("player1") as KinematicBody2D;
        player2 = GetParent().GetNode("player2") as KinematicBody2D;
        player1Anim = GetParent().GetNode("player1/Sprite") as AnimatedSprite;
        player2Anim = GetParent().GetNode("player2/Sprite") as AnimatedSprite;
        sendPacketTimer = GetParent().GetNode("sendPacketTimer") as Timer;
        playerSprite = GetNode("Sprite") as AnimatedSprite;
      
        
       
       

        sendPacketTimer.Connect("timeout", this, "_on_timeout");
        
    
>>>>>>> Stashed changes
    }

   
   //-------------------LOCAL MOVEMENT--------------------
    public override void _Input(InputEvent @event)
    {
        velocity = new Vector2();
        if(Input.IsActionPressed("ui_right"))
            velocity.x +=1;
        if(Input.IsActionPressed("ui_left"))
            velocity.x -=1;
        if(Input.IsActionPressed("ui_down"))
            velocity.y +=1;
        if(Input.IsActionPressed("ui_up"))
            velocity.y -=1;
        
<<<<<<< Updated upstream
        velocity = velocity.Normalized() * speed;
=======
       
        
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
       
        
        
        
>>>>>>> Stashed changes
    }

    public override void _PhysicsProcess(float delta) => velocity = MoveAndSlide(velocity);
    
        
    

     //-------------------NETWORKED MOVEMENT--------------------
<<<<<<< Updated upstream

     
     public override void _Process(float delta) => transferPlayerMovement();
=======
  public override void _Process(float delta) 
  {
      transferPlayerMovement();
      
      
   
      
      
  }
>>>>>>> Stashed changes
     
     
  private void transferPlayerMovement()
    {
        if ( sendPacketReady )
        {
            // Create animationState new packet with movement data
            FlatBufferBuilder builder = new FlatBufferBuilder(8);
<<<<<<< Updated upstream
=======
            var x = builder.CreateString(animationState);
            
            
            
>>>>>>> Stashed changes
            NetworkPacket.OtherPlayer.StartOtherPlayer(builder);

            if (global.playingAsHost)
                NetworkPacket.OtherPlayer.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, player1.Transform.origin.x, player1.Transform.origin.y));
<<<<<<< Updated upstream
=======
                NetworkPacket.OtherPlayer.AddAnim(builder,x);
                NetworkPacket.OtherPlayer.AddDirection(builder, playerLookDirection);
            }
                
                
                
>>>>>>> Stashed changes
            else
                NetworkPacket.OtherPlayer.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, player2.Transform.origin.x, player2.Transform.origin.y));
<<<<<<< Updated upstream
=======
                NetworkPacket.OtherPlayer.AddAnim(builder, x);
                NetworkPacket.OtherPlayer.AddDirection(builder, playerLookDirection);
            }
                
>>>>>>> Stashed changes

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
   
<<<<<<< Updated upstream
   

=======
  
   
>>>>>>> Stashed changes
    
}
