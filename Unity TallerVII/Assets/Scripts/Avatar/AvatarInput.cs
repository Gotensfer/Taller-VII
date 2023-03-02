using Fusion;
using UnityEngine;

public enum AvatarButtons
{
    Jump = 0,
    Crouch = 1,
    Dash = 2,
    Fire = 3,
    Reload = 4,
    Pickup = 5
}
public struct AvatarInput : INetworkInput
{
    private Vector2 directionalInput;
    public Vector2 DirectionalInput { get => directionalInput; set => directionalInput = value; }

    private NetworkButtons buttons;
    public NetworkButtons Buttons { get => buttons; set => buttons = value; }
}