using DynamicBox.UIControllers;
using Fusion;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicBox.UIViews
{
	public class LobbyView : MonoBehaviour
	{
		[Header("Controller Reference")]
		[SerializeField] private LobbyController controller;

		[Header("View Properties")]
		[SerializeField] private GameObject lobbyPanel;

		[SerializeField] private GameObject sessionListElementPrefab;

		[SerializeField] private Transform sessionListParent;

		[SerializeField] private TMP_InputField roomNameInputField;

		[SerializeField] private Button joinRoomButton;


		#region View-Controller Methods

		public void OnMainMenuButtonPressed()
		{
			controller.OnMainMenuButtonPressed();
			lobbyPanel.SetActive(false);
		}

		public void OnJoinRoomButtonPressed()
		{
			controller.OnJoinRoomButtonPressed(roomNameInputField.text);
		}

		public void OnRoomNameInputFieldChanged(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				joinRoomButton.interactable = false;
				return;
			}

			joinRoomButton.interactable = true;
		}
		#endregion

		public void OpenLobbyPanel()
		{
			lobbyPanel.SetActive(true);
		}

		public void SetupSessionList(List<SessionInfo> sessionInfoList)
		{
			for(int i = 0; i < sessionListParent.childCount; i++)
			{
				Destroy(sessionListParent.GetChild(0).gameObject);
			}

			for(int i = 0; i < sessionInfoList.Count; i++)
			{
				GameObject sessionInfoInstance = Instantiate(sessionListElementPrefab, sessionListParent);

				sessionInfoInstance.GetComponent<SessionInfoController>().SetSessionRoomData(sessionInfoList[i]);
			}
		}

	}
}