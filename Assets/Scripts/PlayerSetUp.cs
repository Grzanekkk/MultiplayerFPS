using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

// [RequireComponent(typeof(PlayerManager))]
public class PlayerSetUp : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componetsToDisable;

    Camera startUpCamera;

    [SerializeField]
    string remotaLayerName = "RemotePlayer";

    
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
            
            
        }
        else
        {
            startUpCamera = Camera.main;
            if (startUpCamera != null)
            {
                Camera.main.gameObject.SetActive(false);
            }
        }
    }

    public override void OnStartClient() // aktywuje się gdy obiekt dołącza do gry
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netID, player);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componetsToDisable.Length; i++)
        {
            componetsToDisable[i].enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remotaLayerName);
    }

    void OnDisable()
    {
        if (startUpCamera != null)
        {
            Camera.main.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }

}
