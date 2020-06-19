using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetUp : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componetsToDisable;

    [SerializeField]
    Camera startUpCamera;
    
    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componetsToDisable.Length; i++)
            {
                componetsToDisable[i].enabled = false;
            }
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

    void OnDisable()
    {
        if (startUpCamera != null)
        {
            Camera.main.gameObject.SetActive(true);
        }
    }

}
