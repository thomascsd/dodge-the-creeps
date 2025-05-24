using Godot;
using System;

public partial class Player : Area2D
{
    [Export]
    public int Speed { get; set; } = 400; // 玩家移動速度，預設為400，可在Godot編輯器中調整

    [Signal]
    public delegate void HitEventHandler();

    public Vector2 ScreenSize { get; set; } // 螢幕尺寸，用來限制玩家移動範圍

    override public void _Ready()
    {
        this.ScreenSize = this.GetViewportRect().Size; // 遊戲開始時取得螢幕尺寸
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero; // 初始化速度向量為零

        if (Input.IsActionPressed("MoveUp")) // 按下向上鍵時
        {
            velocity.Y -= 1; // 速度Y軸減1，向上移動
        }
        else if (Input.IsActionPressed("MoveDown")) // 按下向下鍵時
        {
            velocity.Y += 1; // 速度Y軸加1，向下移動
        }
        if (Input.IsActionPressed("MoveLeft")) // 按下向左鍵時
        {
            velocity.X -= 1; // 速度X軸減1，向左移動
        }
        else if (Input.IsActionPressed("MoveRight")) // 按下向右鍵時
        {
            velocity.X += 1; // 速度X軸加1，向右移動
        }

        var animateSprite2D = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D"); // 取得動畫精靈節點

        if (velocity.Length() > 0) // 如果有移動
        {
            if (velocity.X != 0)
            {
                animateSprite2D.Animation = "walk";
                animateSprite2D.FlipV = false; // 垂直翻轉動畫精靈
                animateSprite2D.FlipH = velocity.X < 0;
            }
            else if (velocity.Y != 0)
            {
                animateSprite2D.Animation = "walk";
                animateSprite2D.FlipV = velocity.Y > 0; // 垂直翻轉動畫精靈
                animateSprite2D.FlipH = false; // 水平不翻轉
            }

            velocity = velocity.Normalized() * this.Speed; // 正規化速度向量並乘上速度值
            animateSprite2D.Play(); // 播放移動動畫
        }
        else
        {
            animateSprite2D.Stop(); // 沒有移動時停止動畫
        }

        this.Position += velocity * (float)delta; // 根據速度與delta時間更新玩家位置

        // 限制玩家位置在螢幕範圍內
        this.Position = new Vector2(
                    Math.Clamp(this.Position.X, 0, this.ScreenSize.X), // 限制X座標在螢幕範圍內
                    Math.Clamp(this.Position.Y, 0, this.ScreenSize.Y)  // 限制Y座標在螢幕範圍內
                );

        base._Process(delta); // 呼叫父類別的_Process方法
    }

    public void Start(Vector2 position)
    {
        this.Position = position; // 設定玩家初始位置
        this.Show(); // 顯示玩家
        var coll = this.GetNode<CollisionShape2D>("CollisionShape2D"); // 取得碰撞形狀節點
        coll.Disabled = false;
    }


    public void OnBodyEntered(Node2D body)
    {
        var coll = this.GetNode<CollisionShape2D>("CollisionShape2D"); // 取得碰撞形狀節點

        this.Hide(); // 當玩家與其他物體碰撞時隱藏玩家
        this.EmitSignal(SignalName.Hit); // 發出Hit事件信號

        // 
        coll.SetDeferred(CollisionShape2D.PropertyName.Disabled, true); // 延遲禁用碰撞形狀，避免再次觸發碰撞事件

    }

}
