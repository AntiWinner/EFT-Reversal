using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.LowLevel;

namespace CustomPlayerLoopSystem;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct FrameCounter
{
	private static int _currentFrame;

	public static int CurrentFrame => _currentFrame;

	public static PlayerLoopSystem GetNewSystem()
	{
		PlayerLoopSystem result = default(PlayerLoopSystem);
		result.type = typeof(FrameCounter);
		result.updateDelegate = UpdateFunction;
		return result;
	}

	private static void UpdateFunction()
	{
		Interlocked.Exchange(ref _currentFrame, Time.frameCount);
	}
}
