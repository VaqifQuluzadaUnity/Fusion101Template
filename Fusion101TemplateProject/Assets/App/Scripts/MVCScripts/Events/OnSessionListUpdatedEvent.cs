using DynamicBox.EventManagement;
using Fusion;
using System.Collections.Generic;

namespace DynamicBox.UIEvents
{
  public class OnSessionListUpdatedEvent : GameEvent
  {
    public List<SessionInfo> SessionInfoList = new List<SessionInfo>();

		public OnSessionListUpdatedEvent(List<SessionInfo> sessionInfoList)
		{
			SessionInfoList = sessionInfoList;
		}
	}
}