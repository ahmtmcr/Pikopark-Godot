using Godot;
using System;

public class respawnPoint : Area2D
{
   
    KinematicBody2D player1;
    KinematicBody2D player2;
   
    public override void _Ready()
    {
        player1 = GetParent().GetNode("game/player1") as KinematicBody2D;
        player2 = GetParent().GetNode("game/player2") as KinematicBody2D;
    }


 public override void _Process(float delta)
  {
      if(OverlapsBody(player1))
      {
          player1.Position = new Vector2(20, 100);
          
      }
      if(OverlapsBody(player2))
      {
           player2.Position = new Vector2(20, 100);
           
      }
  
  }
}
