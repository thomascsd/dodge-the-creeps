using Godot;
using System;

public partial class Main : Node
{
    private int _Score = 0;

    [Export]
    public PackedScene MobScene { get; set; }

    public void GameOver()
    {
        this.GetNode<Timer>("MobTimer").Stop();
        this.GetNode<Timer>("ScoreTimer").Stop();
    }

    public void GameStart()
    {
        this._Score = 0;

        var player = this.GetNode<Player>("Player");
        var startPosition = this.GetNode<Marker2D>("StartPosition");

        player.Position = startPosition.Position;

        this.GetNode<Timer>("StartTimer").Start();
    }

    public void OnMobTimerTimeout()
    {

    }

    public void OnScoreTimerTimeout()
    {
        _Score++;
    }

    public void OnStartTimerTimeout()
    {
        this.GetNode<Timer>("MobTimer").Start();
        this.GetNode<Timer>("ScoreTimer").Start();

    }

}
