using Godot;
using System;

public partial class Main : Node
{
    private int _Score = 0;

    [Export]
    public PackedScene MobScene { get; set; }
    private Timer _mobTimer;
    private Timer _scoreTimer;
    private Timer _startTimer;

    public override void _Ready()
    {
        GD.Print("Game Started");
        this.NewGame();
    }

    public void GameOver()
    {
        _mobTimer.Stop();
        _scoreTimer.Stop();
    }

    public void NewGame()
    {
        this._Score = 0;

        var player = this.GetNode<Player>("Player");
        var startPosition = this.GetNode<Marker2D>("StartPosition");

        player.Position = startPosition.Position;

        this._mobTimer = this.GetNode<Timer>("MobTimer");
        this._mobTimer.Timeout += this.OnMobTimerTimeout;

        this._scoreTimer = this.GetNode<Timer>("ScoreTimer");
        this._scoreTimer.Timeout += this.OnScoreTimerTimeout;

        this._startTimer = this.GetNode<Timer>("StartTimer");
        this._startTimer.Timeout += this.OnStartTimerTimeout;

        this._startTimer.Start();

        GD.Print("New Game");
    }

    public void OnMobTimerTimeout()
    {
        Mob mob = this.MobScene.Instantiate<Mob>();

        GD.Print("Mob Spawned");
        GD.Print(mob.ToString());

        var mobSpawnPostion = this.GetNode<PathFollow2D>("MobPath/MobSpawnLocation");

        mobSpawnPostion.ProgressRatio = GD.Randf();
        mob.Position = mobSpawnPostion.Position;

        var direction = mobSpawnPostion.Rotation + MathF.PI / 2;

        direction += (float)GD.RandRange(-MathF.PI / 4, MathF.PI / 4);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(150, 250), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        this.AddChild(mob);
    }

    public void OnScoreTimerTimeout()
    {
        _Score++;
    }

    public void OnStartTimerTimeout()
    {
        GD.Print("Start Timer Timeout");
        this._mobTimer.Start();
        this._scoreTimer.Start();
    }

}
