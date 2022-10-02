using DynamicBox.UIControllers;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicBox.UIViews
{
	public class SessionInfoView : MonoBehaviour
	{
		[Header("Controller Reference")]
		[SerializeField] private SessionInfoController controller;

		[Header("View properties")]
		[SerializeField] private TMP_Text sessionNameText;

		[SerializeField] private TMP_Text mapNameText;

		[SerializeField] private TMP_Text playerCountText;

		[SerializeField] private Image lockImage;

		[SerializeField] private Button joinButton;

		private SessionInfo currentSessionInfo;


		#region View-Controller Methods

		public void SetupRoom(SessionInfo sessionInfo)
		{

			currentSessionInfo = sessionInfo;

			sessionNameText.text = sessionInfo.Name;

			playerCountText.text = sessionInfo.PlayerCount + " / " + sessionInfo.MaxPlayers;

			;

			mapNameText.text = sessionInfo.Properties["map"].PropertyValue.ToString();

			string pass =sessionInfo.Properties["pass"].PropertyValue.ToString();

			if (string.IsNullOrEmpty(pass))
			{
				lockImage.gameObject.SetActive(false);
			}

			//add join button functionality
		}


		public void OnJoinButtonPressed()
		{
			//Check password

			controller.OnJoinButtonPressed(currentSessionInfo.Name);
		}
		#endregion
	}
}

