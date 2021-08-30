using Godot;
using System;

public class initializePlayer : Node
{
  
    public override void _Ready()
    {
        Global global = GetNode("/root/Global") as Global;
        KinematicBody2D player1 = GetNode("/root/game/player1") as KinematicBody2D;
        KinematicBody2D player2 = GetNode("/root/game/player2") as KinematicBody2D;

        if (global.playingAsHost)
        {
            player1.SetScript(ResourceLoader.Load("scripts/playerMovement.cs"));
        }
        else
        {
            player2.SetScript(ResourceLoader.Load("scripts/playerMovement.cs"));
        }
        
    }

}
