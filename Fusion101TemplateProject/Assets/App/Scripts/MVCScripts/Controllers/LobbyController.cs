using DynamicBox.EventManagement;
using DynamicBox.UIEvents;
using DynamicBox.UIViews;
using System;
using UnityEngine;

namespace DynamicBox.UIControllers
{
  public class LobbyController : MonoBehaviour
  {
    [Header("View Reference")]
		[SerializeField] private LobbyView view;

		#region Unity Methods
		private void OnEnable()
		{
			EventManager.Instance.AddListener<OnLobbyButtonPressedEvent>(OnLobbyButtonPressedEventHandler);
			EventManager.Instance.AddListener<OnSessionListUpdatedEvent>(OnSessionListUpdatedEventHandler);
		}



		private void OnDisable()
		{
			EventManager.Instance.RemoveListener<OnLobbyButtonPressedEvent>(OnLobbyButtonPressedEventHandler);
			EventManager.Instance.RemoveListener<OnSessionListUpdatedEvent>(OnSessionListUpdatedEventHandler);
		}

		internal void OnJoinRoomButtonPressed(string roomName)
		{
			EventManager.Instance.Raise(new OnJoinRoomButtonPressedEvent(roomName));
		}
		#endregion

		#region Event Handlers

		private void OnLobbyButtonPressedEventHandler(OnLobbyButtonPressedEvent eventDetails)
		{
			view.OpenLobbyPanel();
		}

		private void OnSessionListUpdatedEventHandler(OnSessionListUpdatedEvent eventDetails)
		{
			view.SetupSessionList(eventDetails.SessionInfoList);
		}
		#endregion

		#region Controller Methods

		public void OnMainMenuButtonPressed()
		{
			EventManager.Instance.Raise(new OnMainMenuButtonPressedEvent());
		}

		#endregion
	}
}
