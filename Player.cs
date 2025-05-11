using Godot;
using System;

public partial class Player : Area2D
{
    [Export]
    public int Speed { get; set; } = 400;

    public Vector2 ScreenSize { get; set; }

    override public void _Ready()
    {
        this.ScreenSize = this.GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("MoveUp"))
        {
            velocity.Y -= 1;
        }
        else if (Input.IsActionPressed("MoveDown"))
        {
            velocity.Y += 1;
        }
        if (Input.IsActionPressed("MoveLeft"))
        {
            velocity.X -= 1;
        }
        else if (Input.IsActionPressed("MoveRight"))
        {
            velocity.X += 1;
        }

        var animateSprite2D = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * this.Speed;
            animateSprite2D.Play();
        }
        else
        {
            animateSprite2D.Stop();
        }

        this.Position += velocity * (float)delta;

        this.Position = new Vector2(
                    Math.Clamp(this.Position.X, 0, this.ScreenSize.X),
                    Math.Clamp(this.Position.Y, 0, this.ScreenSize.Y)
                );

                

        base._Process(delta);
    }





}
