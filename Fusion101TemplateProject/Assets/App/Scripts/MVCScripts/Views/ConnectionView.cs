using UnityEngine;
using DynamicBox.UIControllers;
using System;

public class ConnectionView : MonoBehaviour
{
  [Header("Controller Reference")]
  [SerializeField] private ConnectionController controller;

  [Header("View Properties")]
  [SerializeField] private GameObject connectionPanel;

	public void CloseConnectionPanel()
	{
    connectionPanel.SetActive(false);
	}
}
