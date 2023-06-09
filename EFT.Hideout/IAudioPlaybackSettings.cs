namespace EFT.Hideout;

public interface IAudioPlaybackSettings
{
	void Play(AudioArray audioArray, ELightStatus status, bool firstStart);

	void Pause(AudioArray audioArray);
}
