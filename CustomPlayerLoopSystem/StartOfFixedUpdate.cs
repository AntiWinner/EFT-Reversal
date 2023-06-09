using System;
using System.Runtime.InteropServices;
using UnityEngine.LowLevel;

namespace CustomPlayerLoopSystem;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct StartOfFixedUpdate
{
	public static event Action OnUpdate;

	public static PlayerLoopSystem GetNewSystem()
	{
		PlayerLoopSystem result = default(PlayerLoopSystem);
		result.type = typeof(StartOfFixedUpdate);
		result.updateDelegate = UpdateFunction;
		return result;
	}

	private static void UpdateFunction()
	{
		StartOfFixedUpdate.OnUpdate?.Invoke();
	}
}
