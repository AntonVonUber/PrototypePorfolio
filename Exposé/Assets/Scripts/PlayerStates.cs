using UnityEngine;
using System.Collections;

public class PlayerStates : MonoBehaviour {

    public enum PlayerStatesEnum {Default, ReadyTakingPhotos, Browsing, Waiting};
    private PlayerStatesEnum playerCurrentState = PlayerStatesEnum.Default;

    public void setPlayerState(PlayerStatesEnum state)
    {
        playerCurrentState = state;
    }

    public PlayerStatesEnum getPlayerState()
    {
        return playerCurrentState;
    }
}
