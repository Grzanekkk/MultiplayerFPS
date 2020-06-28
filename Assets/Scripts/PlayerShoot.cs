using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError(" PlayerShoot : No camera reference");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }


    [Client]
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            // We hit something 
            // Debug.Log($"We hit {hit.collider.name}");

            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
    
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int damage)
    {
        Debug.Log($"{_playerID} has been shot");

        Player player  = GameManager.GetPlayer(_playerID);
        player.RpcTakeDamage(damage);
    }
}
