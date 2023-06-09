namespace EFT.Animations;

public interface IEffector
{
	void Initialize(PlayerSpring playerSpring);

	void Process(float deltaTime);

	string DebugOutput();
}
