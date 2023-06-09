using System;
using UnityEngine;

namespace EFT.Hideout;

[Serializable]
public class QuickTimeEvent
{
	public EQteType Type;

	public Vector2 Position;

	public float StartDelay;

	public float EndDelay;

	public float Speed;

	public Vector2 SuccessRange;

	public KeyCode Key;

	public QuickTimeEvent(EQteType type, Vector2 position, float speed, float startDelay, float endDelay, Vector2 successRange, KeyCode key)
	{
		Type = type;
		Position = position;
		Speed = speed;
		StartDelay = startDelay;
		EndDelay = endDelay;
		SuccessRange = successRange;
		Key = key;
	}
}
