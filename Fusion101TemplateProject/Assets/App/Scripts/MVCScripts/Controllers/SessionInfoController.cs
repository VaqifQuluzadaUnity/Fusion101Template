using UnityEngine;
using DynamicBox.UIViews;
using Fusion;
using DynamicBox.EventManagement;
using DynamicBox.UIEvents;

namespace DynamicBox.UIControllers
{
	public class SessionInfoController : MonoBehaviour
	{
		[Header("View Reference")]
		[SerializeField] private SessionInfoView view;

		[Header("Controller properties")]
		[SerializeField] private SessionInfo sessionInfoData;

		public void SetSessionRoomData(SessionInfo sessionInfo)
		{

			view.SetupRoom(sessionInfo);

		}

		public void OnJoinButtonPressed(string sessionInfoName)
		{
			EventManager.Instance.Raise(new OnJoinRoomButtonPressedEvent(sessionInfoName));
		}
	}
}