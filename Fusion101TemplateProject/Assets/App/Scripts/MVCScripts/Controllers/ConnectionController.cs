using UnityEngine;
using DynamicBox.UIViews;
using DynamicBox.UIEvents;
using DynamicBox.EventManagement;

namespace DynamicBox.UIControllers
{
	public class ConnectionController : MonoBehaviour
	{
		[Header("View Reference")]
		[SerializeField] private ConnectionView view;

		#region Unity Methods

		private void OnEnable()
		{
			EventManager.Instance.AddListener<OnConnectedToServerEvent>(OnConnectedToServerEventHandler);
		}

		private void OnDisable()
		{
			EventManager.Instance.RemoveListener<OnConnectedToServerEvent>(OnConnectedToServerEventHandler);
		}

		#endregion

		#region Event Handlers

		private void OnConnectedToServerEventHandler(OnConnectedToServerEvent eventDetails)
		{
			view.CloseConnectionPanel();
		}

		#endregion
	}
}

