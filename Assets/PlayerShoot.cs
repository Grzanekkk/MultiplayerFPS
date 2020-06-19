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
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name);
            }
            else
            {
                Debug.Log($"We hit {hit.collider.name}");
            }
    
        }
    }

    [Command]
    void CmdPlayerShot(string _ID)
    {
        Debug.Log($"{_ID} has been shot");

        // Destroy(GameObject.Find(_ID)); BARDZO WOLNE I NIE EFEKTYWNE
    }
}
