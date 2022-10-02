using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  [SerializeField] private GameObject mainMenuPanel;

  [SerializeField] private GameObject createRoomPanel;

  [SerializeField] private GameObject lobbyPanel;

  public void LobbyToMainMenuButtonPressed()
	{
    //Reset lobby panel
    CloseAllPanels();
    mainMenuPanel.SetActive(true);
	}

  public void CreateRoomToMainMenuButtonPressed()
	{
    //Reset everything in create room panel
    CloseAllPanels();
    mainMenuPanel.SetActive(true);
	}

  public void OnCreateAndJoinButtonPressed()
	{
    CloseAllPanels();

    //Join functionality
	}

  public void OnCreateRoomButtonPressed()
	{
    CloseAllPanels();

    createRoomPanel.SetActive(true);
	}

  public void OnLobbyButtonPressed()
	{
    CloseAllPanels();

    lobbyPanel.SetActive(true);
	}

  private void CloseAllPanels()
	{
    mainMenuPanel.SetActive(false);

    createRoomPanel.SetActive(false);

    lobbyPanel.SetActive(false);
	}
}
