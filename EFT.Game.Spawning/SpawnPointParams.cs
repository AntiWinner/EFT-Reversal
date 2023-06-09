namespace EFT.Game.Spawning;

public struct SpawnPointParams
{
	public string Id;

	public ClassVector3 Position;

	public float Rotation;

	public EPlayerSideMask Sides;

	public ESpawnCategoryMask Categories;

	public string Infiltration;

	public string BotZoneName;

	public float DelayToCanSpawnSec;

	public ISpawnColliderParams ColliderParams;
}
