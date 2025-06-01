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

    private Hud _hud;

    public override void _Ready()
    {
        //GD.Print("Game Started");
        //this.NewGame();
    }

    public void GameOver()
    {
        _hud.ShowGameOver();
        _mobTimer.Stop();
        _scoreTimer.Stop();
    }

    public void NewGame()
    {
        this._Score = 0;

        var player = this.GetNode<Player>("Player");
        var startPosition = this.GetNode<Marker2D>("StartPosition");

        this.GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

        player.Position = startPosition.Position;

        this._mobTimer = this.GetNode<Timer>("MobTimer");
        this._mobTimer.Timeout += this.OnMobTimerTimeout;

        this._scoreTimer = this.GetNode<Timer>("ScoreTimer");
        this._scoreTimer.Timeout += this.OnScoreTimerTimeout;

        this._startTimer = this.GetNode<Timer>("StartTimer");
        this._startTimer.Timeout += this.OnStartTimerTimeout;

        this._startTimer.Start();

        _hud = this.GetNode<Hud>("Hud");
        _hud.UpdateScore(this._Score);
        _hud.ShowMessageAndTimerStart("Dodge the Creeps!");

        GD.Print("New Game");
    }

    public void OnMobTimerTimeout()
    {
        Mob mob = this.MobScene.Instantiate<Mob>();

        GD.Print("Mob Spawned");
        GD.Print(mob.ToString());

        // Choose a random location on Path2D.
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        // Set the mob's direction perpendicular to the path direction.
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        // mob.Position = mobSpawnLocation.Position;
        mob.Position = new Vector2(Math.Abs(mobSpawnLocation.Position.X), Math.Abs(mobSpawnLocation.Position.Y));

        GD.Print("Mob Position: " + mob.Position.ToString());

        // Add some randomness to the direction.
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        // Choose the velocity.
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);
    }

    public void OnScoreTimerTimeout()
    {
        _Score++;
        _hud.UpdateScore(this._Score);

    }

    public void OnStartTimerTimeout()
    {
        GD.Print("Start Timer Timeout");
        this._mobTimer.Start();
        this._scoreTimer.Start();
    }



}
