using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager singelton;

    public MatchSettings matchSettings;

    private void Awake()
    {
        if (singelton != null)
        {
            Debug.Log("More then one GameManager in scene");
        }
        else
        {
            singelton = this;
        }
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(playerID, _player);
        _player.transform.name = playerID;
    }

    public static void UnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    #endregion Registering player
}


