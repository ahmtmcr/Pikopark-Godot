using Godot;
using System;

public class buttonPushed : Area2D
{
    
    float PlatformSpeed;
    
    
    KinematicBody2D player1;
    KinematicBody2D player2;

    AnimatedSprite buttonAnim;
    
    
    RigidBody2D platform;
    Vector2 oldPos;
    
    
    
    public override void _Ready()
    {
        player1 = GetParent().GetNode("game/player1") as KinematicBody2D;
        player2 = GetParent().GetNode("game/player2") as KinematicBody2D;
        platform = GetParent().GetNode("platform") as RigidBody2D;
        buttonAnim = GetNode("AnimatedSprite") as AnimatedSprite;
        oldPos = platform.Position; 
    }

  public override void _Process(float delta)
  {
      
      PlatformSpeed += delta * 0.009f;
      
      buttonAnim.Play("idle");
      if(OverlapsBody(player1))
      {
         
          buttonAnim.Play("pushed");
          platform.Position = platform.Position.LinearInterpolate(new Vector2(368,336), PlatformSpeed);

      }
      
      if(OverlapsBody(player2))
      {
          buttonAnim.Play("pushed");
          platform.Position = platform.Position.LinearInterpolate(new Vector2(368,336), PlatformSpeed);
          
      }
      else 
      {
           platform.Position = platform.Position.LinearInterpolate(oldPos, PlatformSpeed);
      }
     
  
  }

 
}
