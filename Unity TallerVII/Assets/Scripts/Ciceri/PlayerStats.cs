using Fusion;
using UnityEngine;
public class PlayerStats : NetworkBehaviour
{

    /*
     * + Name : NetworkString<_32>
+ Score : int
+ TotalDeaths : int
+ Skins : Dictionary<skin, bool>

+ Ready : NetworkBool
+ DoneLoading : NetworkBool

+ Avatar: gameObject
     * */
    [SerializeField] public AvatarStats avatar;
    private AvatarStats _avatar;
    [Networked] public bool start { get; set; } 
    public PlayerStats Local { set; get; }
    [Networked(OnChanged = nameof(ChangeName))]

    public NetworkString<_32> Name { get; set; }
    [Networked] public Color Color { get; set; }
    [Networked] public NetworkBool Ready { get; set; }
    [Networked] public NetworkBool DoneLoading { get; set; }
    [Networked] public int Token { get; set; }

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public override void FixedUpdateNetwork()
    {
        if (_avatar == null )
        {
            _avatar = Runner.Spawn(avatar, transform.position, transform.rotation, Object.InputAuthority, (runner, o) =>
             {
                 AvatarStats Avatar = o.GetComponent<AvatarStats>();
                 Debug.Log($"Created Character for Player {Name}");
                 Avatar.Player = this;
               
             });
        }

    }
    public void Despawn()
    {
        if (HasStateAuthority)
        {
            if (_avatar != null)
            {
                Runner.Despawn(_avatar.Object);
                _avatar = null;
            }
            Runner.Despawn(Object);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    static void ChangeName(Changed<PlayerStats> changed)
    {
        print("cambio de nombre " + changed.Behaviour.Name);
        changed.Behaviour.changeName();
    }
    private void changeName()
    {
        
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetName(string Name, RpcInfo info = default)
    {
        print("[RPC]cambio de nombre " + Name);
        this.Name = Name;

    }
    public void starEffec()
    {
        start = true;

    }

}
