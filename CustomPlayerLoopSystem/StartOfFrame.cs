using System;
using System.Runtime.InteropServices;
using UnityEngine.LowLevel;

namespace CustomPlayerLoopSystem;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct StartOfFrame
{
	public static event Action OnUpdate;

	public static PlayerLoopSystem GetNewSystem()
	{
		PlayerLoopSystem result = default(PlayerLoopSystem);
		result.type = typeof(StartOfFrame);
		result.updateDelegate = UpdateFunction;
		return result;
	}

	private static void UpdateFunction()
	{
		StartOfFrame.OnUpdate?.Invoke();
	}
}
