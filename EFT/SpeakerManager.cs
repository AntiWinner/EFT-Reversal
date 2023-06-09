using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT;

public class SpeakerManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public int id;

		internal bool _E000(_E76C s)
		{
			return s.Id == id;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Player player;

		internal bool _E000(_E76D g)
		{
			return g.Members.Contains(player);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string groupId;

		internal bool _E000(_E76D x)
		{
			return x.GroupId == groupId;
		}
	}

	private List<_E76D> m__E000 = new List<_E76D>();

	[CompilerGenerated]
	private List<_E76C> m__E001;

	private Dictionary<int, _E76D> m__E002 = new Dictionary<int, _E76D>();

	public _E76C ClientSpeaker;

	public List<_E76C> Speakers
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	private SpeakerManager()
	{
		Speakers = new List<_E76C>();
	}

	public void AddSpeaker(_E76C speaker)
	{
		Speakers.Add(speaker);
	}

	public void RemoveSpeaker(_E76C speaker)
	{
		if (Speakers.Contains(speaker))
		{
			Speakers.Remove(speaker);
		}
	}

	public void ManualUpdate(float dt)
	{
		for (int i = 0; i < Speakers.Count; i++)
		{
			Speakers[i].ManualUpdate(dt);
		}
	}

	public _E76C GetSpeaker(int id)
	{
		return Speakers.FirstOrDefault((_E76C s) => s.Id == id);
	}

	public ETagStatus GetGroupStatus(_E76C speaker)
	{
		if (!this.m__E002.TryGetValue(speaker.Id, out var value))
		{
			return ETagStatus.Solo;
		}
		return value.GetGroupStatus(speaker);
	}

	public bool FreeToSpeak(EPhraseTrigger trigger, int id)
	{
		if (!this.m__E002.TryGetValue(id, out var value))
		{
			return true;
		}
		for (int num = value.Blockers.Count - 1; num > -1; num--)
		{
			if (value.Blockers[num].Time < Time.time)
			{
				value.Blockers.RemoveAt(num);
			}
			else if (value.Blockers[num].Event == trigger)
			{
				return false;
			}
		}
		return true;
	}

	public void RemoveFromGroup(Player player)
	{
		_E76D obj = this.m__E000.FirstOrDefault((_E76D g) => g.Members.Contains(player));
		if (obj != null)
		{
			player.Speaker.OnPhraseTold -= _E000;
			obj.Members.Remove(player);
			if (this.m__E002.ContainsKey(player.PlayerId))
			{
				this.m__E002.Remove(player.PlayerId);
			}
			if (obj.Members.Count == 0)
			{
				this.m__E000.Remove(obj);
			}
		}
	}

	public _E76D AddNewGroup(string groupId)
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = new List<_E76D>();
		}
		_E76D obj = new _E76D
		{
			GroupId = groupId
		};
		this.m__E000.Add(obj);
		return obj;
	}

	public void AssignToGroup(EPlayerSide side, Player player)
	{
		string groupId = ((side == EPlayerSide.Savage) ? _ED3E._E000(133465) : player.Profile.Info.GroupId);
		_E76D obj = this.m__E000.FirstOrDefault((_E76D x) => x.GroupId == groupId) ?? AddNewGroup(groupId);
		player.Speaker.OnPhraseTold += _E000;
		obj.Members.Add(player);
		if (!string.IsNullOrEmpty(groupId))
		{
			if (this.m__E002.ContainsKey(player.PlayerId))
			{
				Debug.LogErrorFormat(_ED3E._E000(133462), player.PlayerId);
			}
			this.m__E002[player.PlayerId] = obj;
		}
	}

	private void _E000(EPhraseTrigger trigger, TaggedClip clip, TagBank bank, _E76C speaker)
	{
	}

	private bool _E001(int id1, int id2)
	{
		if (this.m__E002.ContainsKey(id1) && this.m__E002.ContainsKey(id2))
		{
			return this.m__E002[id1] == this.m__E002[id2];
		}
		return false;
	}

	public void GroupEvent(int speakerId, EPhraseTrigger @event, Vector3 position, ETagStatus tags, int probability = 50)
	{
	}
}
