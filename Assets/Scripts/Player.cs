using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SyncVar] [SerializeField]
    private int currentHealth;


    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefault();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
            RpcTakeDamage(99999);
    }

    void SetDefault()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;
    }

    [ClientRpc]
    public void RpcTakeDamage(int _damage)
    {
        if (isDead)
            return;

        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log($"Now {transform.name} has {currentHealth} health points");
        }
             
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        GetComponent<PlayerMotor>().Move(Vector3.zero);


        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        Debug.Log($"{transform.name} is DEAD!!!");

        // Respawn(); Tak nie zadziała XD

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        Debug.Log("Respawning in 3.. 2.. 1..");
        yield return new WaitForSecondsRealtime(GameManager.singelton.matchSettings.respawnTime);

        SetDefault();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(transform.name + " Respawned!!");
    }
}
