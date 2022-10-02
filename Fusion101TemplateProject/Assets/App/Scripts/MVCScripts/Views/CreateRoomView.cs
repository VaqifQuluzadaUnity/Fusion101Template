using DynamicBox.UIControllers;
using Fusion;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DynamicBox.UIViews
{
	public class CreateRoomView : MonoBehaviour
	{
		[Header("Controller Reference")]
		[SerializeField] private CreateRoomPanelController controller;

		[Header("Properties")]
		[SerializeField] private int maxUserCount;

		[Header("View Properties")]

		[SerializeField] private TMP_InputField roomNameIF;

		[SerializeField] private TMP_InputField roomUserCountIF;

		[SerializeField] private TMP_InputField roomPassIF;

		[SerializeField] private TMP_InputField lobbyNameIF;

		[SerializeField] private Toggle isHiddenToggle;

		[SerializeField] private Button createRoomButton;

		[SerializeField] private TMP_Text maxUserCountText;

		[SerializeField] private TMP_Text notificationText;

		[SerializeField] private TMP_Dropdown mapSelectDropDown;

		[SerializeField] private TMP_Dropdown gameModeSelectDropDown;

		[SerializeField] private GameObject createRoomPanel;

		[SerializeField] private int roomCount = 0;

		#region Unity Methods

		private void OnEnable()
		{
			maxUserCountText.text = maxUserCount.ToString();

			roomNameIF.text = "Room " + (roomCount + 1).ToString();

			roomUserCountIF.text = "4";

			lobbyNameIF.text = "Default";
		}

		#endregion

		#region View-Controller Methods

		public void OnMainMenuButtonPressed()
		{
			controller.OnMainMenuButtonPressed();
			createRoomPanel.SetActive(false);
		}

		public void UpdateRoomCount(int count)
		{
			roomCount = count;
		}

		#endregion

		#region View methods

		public void OnRoomNameInputFieldChanged(string input)
		{
			if(string.IsNullOrEmpty(input)|| string.IsNullOrEmpty(roomUserCountIF.text)||string.IsNullOrEmpty(lobbyNameIF.text))
			{
				createRoomButton.interactable = false;
				return;
			}
			createRoomButton.interactable = true;
		}

		public void OnRoomUserCountInputFieldChanged(string input)
		{
			if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(roomNameIF.text)|| string.IsNullOrEmpty(lobbyNameIF.text))
			{
				createRoomButton.interactable = false;
				return;
			}

			int userCount = Convert.ToInt32(input);

			if (userCount < 2 || userCount > maxUserCount)
			{
				ShowNotification("Invalid user count");
				return;
			}

			createRoomButton.interactable = true;
		}

		public void OnLobbyNameInputFieldChanged(string input)
		{
			if (string.IsNullOrEmpty(roomNameIF.text) || string.IsNullOrEmpty(roomUserCountIF.text))
			{
				createRoomButton.interactable = false;
				return;
			}

			if (string.IsNullOrEmpty(input))
			{
				lobbyNameIF.text = "Default";
			}

			createRoomButton.interactable = true;
		}
		public void OnCreateRoomButtonPressed()
		{
			int modeIndex = gameModeSelectDropDown.value + 1;
			int mapIndex = mapSelectDropDown.value + 1;
			RoomData roomData = new RoomData(
				roomNameIF.text,
				Convert.ToInt32(roomUserCountIF.text),
				roomPassIF.text,
				mapIndex,
				isHiddenToggle.isOn,
				(GameMode)modeIndex,
				lobbyNameIF.text
				);

			controller.OnCreateRoomButtonPressed(roomData);
		}

		#endregion


		public void OpenCreateRoomPanel()
		{
			createRoomPanel.SetActive(true);
		}

		public async void ShowNotification(string notifText)
		{
			notificationText.text = notifText;

			notificationText.gameObject.SetActive(true);

			await Task.Delay(1000);

			notificationText.gameObject.SetActive(false);
		}
	}
}