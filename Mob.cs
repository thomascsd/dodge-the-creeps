using Godot;
using System;

public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        var asd = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D"); // 取得動畫精靈節點
        string[] types = asd.SpriteFrames.GetAnimationNames(); // 取得所有動畫名稱

        //  GD.Randi() % n selects a random integer between 0 and n-1.
        asd.Play(types[GD.Randi() % types.Length]); // 隨機播放一個動畫

        base._Ready();
    }

    public void OnScreenExited()
    {
        this.QueueFree(); // 當物件離開螢幕時，將其從場景中移除
    }
}
