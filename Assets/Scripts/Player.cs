using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

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

    [SyncVar]
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

    void SetDefault()
    {
        isDead = false;
        currentHealth = maxHealth;

    }

    [ClientRpc]
    public void RpcTakeDamage(int _damage)
    {
        if (isDead)
            return;

        currentHealth -= _damage;

        Debug.Log($"Now {transform.name} has {currentHealth} health points");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        //Disable components

        Debug.Log($"{transform.name} is DEAD!!!");

        // Respawn();
    }

}
