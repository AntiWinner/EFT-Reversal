using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT;

public class PhraseSounds : ScriptableObject
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string voice;

		internal bool _E000(Voice x)
		{
			return x.Name == voice;
		}
	}

	public Voice[] Voices;

	public bool TryGetVoice(string voice, out TagBank[] result)
	{
		result = _E000(voice);
		return result != null;
	}

	public TagBank[] GetVoice(string voice, EPlayerSide side)
	{
		TagBank[] array = _E000(voice);
		if (array == null)
		{
			Debug.LogError(_ED3E._E000(133428) + voice);
			return _E000(side switch
			{
				EPlayerSide.Usec => _ED3E._E000(113032), 
				EPlayerSide.Bear => _ED3E._E000(112752), 
				_ => _ED3E._E000(113328), 
			});
		}
		return array;
	}

	[CanBeNull]
	private TagBank[] _E000(string voice)
	{
		Voice voice2 = Voices.FirstOrDefault((Voice x) => x.Name == voice);
		if (!(voice2 == null))
		{
			return voice2.Banks;
		}
		return null;
	}
}
