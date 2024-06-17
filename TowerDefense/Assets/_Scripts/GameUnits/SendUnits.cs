using Photon.Pun;
using UnityEngine;

public class SendUnits : MonoBehaviourPun
{
    private GridManager _gridManager;
    private PlayerManager _playerManager;


    public void SendUnitsToNextAlivePlayer()
    {
        _gridManager = FindObjectOfType<GridManager>();
        Unit unit = new Unit(50f, 20f, 2f);

        int localPlayerId = PhotonNetwork.LocalPlayer.ActorNumber;
        int nextPlayerId = _gridManager.GetNextAlivePlayerId(localPlayerId); //returns -1 if error

        if (nextPlayerId != -1)
        {
            _gridManager._photonView.RPC("SpawnUnitsOnMyMap", PhotonNetwork.CurrentRoom.Players[nextPlayerId], nextPlayerId, unit, 1);
            Debug.Log("Ran RPC to send units to player " + nextPlayerId + ".");
        }
        else if (nextPlayerId == -1)
        {
            Debug.LogError("No more players left to send units to.");
        }
    }
}
