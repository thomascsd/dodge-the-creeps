using Godot;
using System;

public partial class Hud : CanvasLayer
{

    [Signal]
    public delegate void GameStartEventHandler();
}
