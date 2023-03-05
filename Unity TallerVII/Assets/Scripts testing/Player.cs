using Fusion;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SerializeField] private Ball _prefabBall;
    [SerializeField] private PhysxBall _prefabPhysxBall;
    private Transform _camera;
    [SerializeField] public PostScream scream;

    [Networked] private TickTimer delay { get; set; }

    private NetworkCharacterControllerPrototype _cc;
    private AvatarStats _cs;
    private Vector3 _forward;
    private Text _messages;
    public float timecount;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
        _cs = GetComponent<AvatarStats>();
        _forward = transform.forward;
        timecount = 5;
    }
    private void LateUpdate()
    {
      /*  if (_camera == null)_camera = Camera.main.transform;
        Vector3 p = transform.position;
        _camera.position = p - 10 * _forward + 5 * Vector3.up;
        _camera.LookAt(p + 2 * Vector3.up);
      */
    }
    private void Update()
    {
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        if (_messages == null)
            _messages = FindObjectOfType<Text>();
        if (info.IsInvokeLocal)
            message = $"You said: {message}\n";
        else
            message = $"Some other player said: {message}\n";
        _messages.text += message;
    }
    [Networked(OnChanged = nameof(OnBallSpawned))]
    public NetworkBool spawned { get; set; }

    private Material _material;
    Material material
    {
        get
        {
            if (_material == null)
                _material = GetComponentInChildren<MeshRenderer>().material;
            return _material;
        }
    }
    public static void OnBallSpawned(Changed<Player> changed)
    {
        changed.Behaviour.material.color = Color.red;

    }
    public override void Render()
    {
        material.color = Color.Lerp(material.color, Color.blue, Time.deltaTime);
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
            {
             _forward = data.direction;
            }
            timecount -= Runner.DeltaTime;
            if (!_cs.IsDead &&timecount<=0)
            {
                _cs.GetHeal(20);
                timecount = 5;
               
            }
            if (_cs.IsDead)
            {
                dead();
                
            }
            if (delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 1f);
                    Runner.Spawn(_prefabBall,
                      transform.position + _forward,
                      Quaternion.LookRotation(_forward),
                      Object.InputAuthority,
                      (runner, o) =>
                      {
              // Initialize the Ball before synchronizing it
              o.GetComponent<Ball>().Init();
                        
                      });
                    if (Object.HasStateAuthority)
                    {
                        scream.PostEvent();
                        _cs.AddScore();
                    }
                }
                else if ((data.buttons & NetworkInputData.MOUSEBUTTON2) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                   
                    Runner.Spawn(_prefabPhysxBall,transform.position + _forward,Quaternion.LookRotation(_forward),Object.InputAuthority,
                      (runner, o) =>
                      {
                          o.GetComponent<PhysxBall>().Init(20 * _forward);
                        

                      });
                    spawned = !spawned;
                    if (Object.HasStateAuthority)
                    {
                        scream.PostEvent();
                        _cs.GetHit(20);
                    }
                    

                }
            }
        }
    }
    public void dead()
    {
        transform.position=new Vector3(0,10,0);
        _cs.Respawn();
    }
}