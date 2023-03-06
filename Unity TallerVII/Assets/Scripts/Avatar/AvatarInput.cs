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
    public Vector2 DirectionalInput;
    public NetworkButtons Buttons;
}

