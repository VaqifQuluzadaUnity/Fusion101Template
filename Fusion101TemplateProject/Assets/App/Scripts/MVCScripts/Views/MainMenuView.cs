using DynamicBox.UIControllers;
using UnityEngine;

namespace DynamicBox.UIViews
{
	public class MainMenuView : MonoBehaviour
	{
		[Header("Controller Reference")]
		[SerializeField] private MainMenuPanelController controller;

		[Header("View Properties")]
		[SerializeField] private GameObject mainMenuPanel;

		#region View-Controller Methods

		public void OnQuickGameButtonPressed()
		{
			mainMenuPanel.SetActive(false);
			controller.OnQuickGameButtonPressed();
		}

		public void OnCreateRoomButtonPressed()
		{
			mainMenuPanel.SetActive(false);
			controller.OnCreateRoomButtonPressed();
		}

		public void OnLobbyButtonPressed()
		{
			mainMenuPanel.SetActive(false);
			controller.OnLobbyButtonPressed();
		}

		#endregion

		#region View Methods

		public void OpenMainMenu()
		{
			mainMenuPanel.SetActive(true);
		}

		#endregion
	}
}

