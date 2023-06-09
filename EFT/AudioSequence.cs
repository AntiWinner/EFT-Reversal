using UnityEngine;

namespace EFT;

public sealed class AudioSequence : ScriptableObject
{
	public struct _E000
	{
		public double StartTime;

		public double LoopLength;

		public double SkipTime;

		public double JumpTime;

		public double InitialLength;

		public double LoopStartTime;

		public float Delay;

		public _E000(double startTime = 0.0, double loopStartTime = 0.0, double initialLength = 0.0, double loopLength = 0.0, double skipTime = -1.0, double jumpTime = -1.0)
		{
			InitialLength = initialLength;
			StartTime = startTime;
			LoopLength = loopLength;
			SkipTime = skipTime;
			JumpTime = ((jumpTime >= double.Epsilon) ? jumpTime : StartTime);
			LoopStartTime = loopStartTime;
			Delay = 0f;
		}
	}

	public AudioClip SequenceClip;

	public double OnTime;

	public double WorkingTime;

	public double OffTime;

	public double DisabledTime;

	public double OverlapTime;

	[HideInInspector]
	public AudioClip OnSource;

	[HideInInspector]
	public AudioClip WorkingSource;

	[HideInInspector]
	public AudioClip OffSource;

	[HideInInspector]
	public AudioClip DisabledSource;

	public _E000 GetSequenceSettings(EAudioSequenceType sequenceType)
	{
		double initialLength = 0.0;
		double num = 0.0;
		double loopStartTime = 0.0;
		double loopLength = 0.0;
		double skipTime = -1.0;
		double num2 = -1.0;
		switch (sequenceType)
		{
		case EAudioSequenceType.OnWorking:
			num = 0.0;
			loopStartTime = OnTime;
			initialLength = OnTime + WorkingTime;
			loopLength = WorkingTime;
			break;
		case EAudioSequenceType.OffDisabled:
			num = OnTime + WorkingTime + OverlapTime;
			loopStartTime = num + OffTime;
			initialLength = OffTime + DisabledTime;
			loopLength = DisabledTime;
			break;
		case EAudioSequenceType.Working:
			num = OnTime;
			loopStartTime = num;
			initialLength = WorkingTime;
			loopLength = WorkingTime;
			break;
		case EAudioSequenceType.Disabled:
			num = OnTime + WorkingTime + OverlapTime + OffTime;
			loopStartTime = num;
			initialLength = DisabledTime;
			loopLength = DisabledTime;
			break;
		case EAudioSequenceType.OnOffDisabled:
			num = 0.0;
			initialLength = OffTime + DisabledTime;
			loopLength = DisabledTime;
			skipTime = OnTime;
			num2 = OnTime + WorkingTime + OverlapTime;
			loopStartTime = num2 + OffTime;
			break;
		}
		return new _E000(num, loopStartTime, initialLength, loopLength, skipTime, num2);
	}
}
