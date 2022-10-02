using DynamicBox.EventManagement;
using DynamicBox.UIEvents;
using DynamicBox.UIViews;
using UnityEngine;

namespace DynamicBox.UIControllers
{
	public class MainMenuPanelController : MonoBehaviour
	{
		[Header("View Reference")]
		[SerializeField] private MainMenuView view;

		#region Unity Methods
		private void OnEnable()
		{
			EventManager.Instance.AddListener<OnMainMenuButtonPressedEvent>(OnMainMenuButtonPressedEventHandler);
		}

		private void OnDisable()
		{
			EventManager.Instance.RemoveListener<OnMainMenuButtonPressedEvent>(OnMainMenuButtonPressedEventHandler);
		}
		#endregion

		#region Event Handlers

		private void OnMainMenuButtonPressedEventHandler(OnMainMenuButtonPressedEvent eventDetails)
		{
			view.OpenMainMenu();
		}

		#endregion

		#region Controller Methods

		public void OnCreateRoomButtonPressed()
		{
			EventManager.Instance.Raise(new OnCreateRoomPanelButtonEvent());
		}

		public void OnLobbyButtonPressed()
		{
			EventManager.Instance.Raise(new OnLobbyButtonPressedEvent());
		}

		public void OnQuickGameButtonPressed()
		{
			EventManager.Instance.Raise(new OnQuickGameButtonPressedEvent());
		}

		#endregion


	}
}

