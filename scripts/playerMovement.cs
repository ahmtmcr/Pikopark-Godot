using Godot;
using System;
using Steamworks;
using FlatBuffers;


public class playerMovement : KinematicBody2D
{
  
    
    int speed = 200;
    float gravity = 500f;
    bool sendPacketReady;
    Vector2 velocity;
    KinematicBody2D player1;
    KinematicBody2D player2;
    Timer sendPacketTimer;
    Sprite playerSprite;


    Global global;
    
    public override void _Ready()
    {
        global = GetNode("/root/Global") as Global;

        player1 = GetNode("/root/game/player1") as KinematicBody2D;
        player2 = GetNode("/root/game/player2") as KinematicBody2D;
        sendPacketTimer = GetNode("/root/game/sendPacketTimer") as Timer;
        playerSprite = GetNode("Sprite") as Sprite;

        sendPacketTimer.Connect("timeout", this, "_on_timeout");
    }

   
   //-------------------LOCAL MOVEMENT--------------------
   /* public override void _Input(InputEvent @event)
    {
        velocity = new Vector2();
        
        GD.Print(IsOnFloor());
        GD.Print(gravity);
        
        if(Input.IsActionPressed("ui_right"))
            velocity.x +=1;
        if(Input.IsActionPressed("ui_left"))
            velocity.x -=1;
        if(Input.IsActionPressed("ui_up") && IsOnFloor())
            velocity.y = -10;

    
        
        velocity = velocity.Normalized() * speed + gravity;
    }*/

    public override void _PhysicsProcess(float delta) 
    {
        velocity.y += delta * gravity;

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x = -speed;
            playerSprite.FlipH = true;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            velocity.x = speed;
            playerSprite.FlipH = false;
        }
        else
        {
            velocity.x = 0;
        }

        if(IsOnFloor() && Input.IsActionPressed("ui_up"))
        {
            velocity.y = -200;
        }

       
        MoveAndSlide(velocity, new Vector2(0, -1));
    }
    
        
    

     //-------------------NETWORKED MOVEMENT--------------------

     
     public override void _Process(float delta) => transferPlayerMovement();
     
     
  private void transferPlayerMovement()
    {
        if ( sendPacketReady )
        {
            // Create a new packet with movement data
            FlatBufferBuilder builder = new FlatBufferBuilder(8);
            NetworkPacket.OtherPlayer.StartOtherPlayer(builder);

            if (global.playingAsHost)
                NetworkPacket.OtherPlayer.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, player1.Transform.origin.x, player1.Transform.origin.y));
            else
                NetworkPacket.OtherPlayer.AddPos(builder, NetworkPacket.Vec2.CreateVec2(builder, player2.Transform.origin.x, player2.Transform.origin.y));

            NetworkPacket.OtherPlayer.AddAction(builder, 1);
            var stopBuilding = NetworkPacket.OtherPlayer.EndOtherPlayer(builder);
            builder.Finish(stopBuilding.Value);

            // Convert the Flatbuffer into a byte array
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
