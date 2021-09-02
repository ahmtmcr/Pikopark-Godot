using Godot;
using System;

public class nextLevel : Area2D
{
    
    
    KinematicBody2D player;
    
    public override void _Ready()
    {
        player = GetParent().GetNode("player1") as KinematicBody2D;
    }


 public override void _Process(float delta)
 {
   if(OverlapsBody(player) && Input.IsActionPressed("ui_down"))
   {
        GetTree().ChangeScene("scenes/level1.tscn");
   }
 }
}
