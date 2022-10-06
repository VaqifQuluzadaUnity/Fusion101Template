using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class NetworkUIHandler : NetworkBehaviour
{

	[SerializeField] private GameObject networkCanvas;
	

	

	public void OnQuitButtonPressed()
	{
		Runner.Shutdown();
	}
}
