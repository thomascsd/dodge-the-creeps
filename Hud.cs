using Godot;
using System;

public partial class Hud : CanvasLayer
{

    [Signal]
    public delegate void GameStartEventHandler();

    private Timer _messageTimer;

    private Button _startButton;

    override public void _Ready()
    {
        _startButton = this.GetNode<Button>("StartButton");
        _startButton.Pressed += this.OnStartButtonPressed;

        _messageTimer = this.GetNode<Timer>("MessageTimer");
        _messageTimer.Timeout += this.OnMessageTimerTimeout;
    }

    public void ShowMessage(string message)
    {
        var lbl = this.GetNode<Label>("Message");

        lbl.Text = message;
        lbl.Show();
    }

    public void ShowMessageAndTimerStart(string message)
    {
        this.ShowMessage(message);
        _messageTimer.Start();
    }


    public async void ShowGameOver()
    {
        this.ShowMessageAndTimerStart("Game Over");

        await this.ToSignal(_messageTimer, Timer.SignalName.Timeout);

        this.ShowMessage("Dodge the Creeps!");


        await this.ToSignal(this.GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);

        _startButton.Show();
    }

    public void UpdateScore(int score)
    {
        this.GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    public void OnStartButtonPressed()
    {
        _startButton.Hide();
        this.EmitSignal(SignalName.GameStart);
    }

    public void OnMessageTimerTimeout()
    {
        this.GetNode<Label>("Message").Hide();
    }
}
