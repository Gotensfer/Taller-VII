using Fusion;
using UnityEngine;

public enum AvatarButtons
{
    Jump,
    Crouch,
    Dash,
    Fire,
    Reload,
    Pickup,
}
public struct AvatarInput : INetworkInput
{
    private Vector2 directionalInput;
    public Vector2 DirectionalInput { get => directionalInput; set => directionalInput = value; }
    
    public NetworkButtons Buttons;
}