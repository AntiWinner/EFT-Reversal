using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CustomPlayerLoopSystem;

public static class CustomPlayerLoopSystemsInjector
{
	[RuntimeInitializeOnLoadMethod]
	private static void Injection()
	{
		Application.quitting += OnQuit;
		_E2E1.InsertAsSubSystem(typeof(EarlyUpdate), StartOfFrame.GetNewSystem(), _E2E1.EOrderType.First);
		_E2E1.InsertAsSubSystem(typeof(PostLateUpdate), EndOfFrame.GetNewSystem(), _E2E1.EOrderType.Last);
		_E2E1.InsertAsSubSystem(typeof(EarlyUpdate), FrameCounter.GetNewSystem(), _E2E1.EOrderType.First);
		MoveUNetUpdateSystemOnPreUpdate();
		InjectGlobalEventsOnPreUpdate();
		_E2E1.InsertAsSubSystem(typeof(FixedUpdate), StartOfFixedUpdate.GetNewSystem(), _E2E1.EOrderType.First);
		_E2E1.InsertAsSubSystem(typeof(FixedUpdate), EndOfFixedUpdate.GetNewSystem(), _E2E1.EOrderType.Last);
		_E2E1.InsertAsSubSystem(typeof(Update), StartOfUpdate.GetNewSystem(), _E2E1.EOrderType.First);
		_E2E1.InsertAsSubSystem(typeof(Update), EndOfUpdate.GetNewSystem(), _E2E1.EOrderType.Last);
		_E2E1.InsertAsSubSystem(typeof(PostLateUpdate), StartOfPostLateUpdate.GetNewSystem(), _E2E1.EOrderType.First);
		_E2E1.PrintAllPlayerLoopSystems();
	}

	private static void MoveUNetUpdateSystemOnPreUpdate()
	{
		_E2E1.InsertAsSubSystem(typeof(PreUpdate), UNetUpdate.GetNewSystem(), _E2E1.EOrderType.Last);
	}

	private static void InjectGlobalEventsOnPreUpdate()
	{
		_E2E1.InsertAsSubSystem(typeof(PreUpdate), GlobalEventsClear.GetNewSystem(), _E2E1.EOrderType.Last);
		_E2E1.InsertAsSubSystem(typeof(PreUpdate), GlobalEventsApply.GetNewSystem(), _E2E1.EOrderType.Last);
	}

	private static void InjectDataProviderSyncUpdate()
	{
		_E2E1.InsertAsSubSystem(typeof(PreUpdate), DataProviderSyncUpdate.GetNewSystem(), _E2E1.EOrderType.Last);
	}

	private static void MoveUNetUpdateSystemOnFixedUpdate()
	{
		_E2E1.InsertAsSubSystem(typeof(FixedUpdate), UNetUpdate.GetNewSystem(), _E2E1.EOrderType.First);
	}

	private static void InjectPostUNetUpdateSystem()
	{
		_E2E1.InsertAsSiblingSystem(typeof(UNetUpdate), PostUNetUpdate.GetNewSystem(), _E2E1.EOrderType.Last);
	}

	private static void OnQuit()
	{
		_E2E1.ResetGameLoop();
	}
}
