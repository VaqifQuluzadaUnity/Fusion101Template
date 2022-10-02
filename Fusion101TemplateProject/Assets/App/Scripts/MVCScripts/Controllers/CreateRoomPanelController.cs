using DynamicBox.EventManagement;
using DynamicBox.UIEvents;
using DynamicBox.UIViews;
using System;
using UnityEngine;

namespace DynamicBox.UIControllers
{
	public class CreateRoomPanelController : MonoBehaviour
	{
		[Header("View Reference")]
		[SerializeField] private CreateRoomView view;

		#region Unity Methods
		private void OnEnable()
		{
			EventManager.Instance.AddListener<OnCreateRoomPanelButtonEvent>(OnCreateRoomButtonPressedEventHandler);

			EventManager.Instance.AddListener<OnSessionListUpdatedEvent>(OnSessionListUpdatedEventHandler);
		}



		private void OnDisable()
		{
			EventManager.Instance.RemoveListener<OnCreateRoomPanelButtonEvent>(OnCreateRoomButtonPressedEventHandler);

			EventManager.Instance.RemoveListener<OnSessionListUpdatedEvent>(OnSessionListUpdatedEventHandler);
		}

		
		#endregion

		#region Event Handlers

		private void OnCreateRoomButtonPressedEventHandler(OnCreateRoomPanelButtonEvent eventDetails)
		{
			view.OpenCreateRoomPanel();
		}

		private void OnSessionListUpdatedEventHandler(OnSessionListUpdatedEvent eventDetails)
		{
			view.UpdateRoomCount(eventDetails.SessionInfoList.Count);
		}

		#endregion

		#region Controller Methods

		public void OnMainMenuButtonPressed()
		{
			EventManager.Instance.Raise(new OnMainMenuButtonPressedEvent());
		}

		public void OnCreateRoomButtonPressed(RoomData roomData)
		{
			EventManager.Instance.Raise(new OnCreateRoomButtonPressedEvent(roomData));
		}

		#endregion


	}
}