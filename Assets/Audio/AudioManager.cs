using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AudioChannel
{
	SFX,
	Music,
	UI
}

[System.Serializable]
public struct AudioBankEntry
{
	public string name;
	public AudioClip clip;
	public AudioChannel channel;
	public bool loop;
	public float defaultVolume;
}

public class AudioManager : MonoSingleton<AudioManager>
{
	[Header("Audio Source Pool")]
	[SerializeField] private AudioSource audioSourcePrefab;
	[SerializeField] private int sfxPoolSize = 16;
	[SerializeField] private int musicPoolSize = 2;
	[SerializeField] private int uiPoolSize = 8;


	[Header("Audio Bank")]
	[SerializeField] private AudioBankEntry[] audioBank;

	private Dictionary<string, AudioBankEntry> _bankMap;
	private Dictionary<AudioChannel, AudioSource[]> _pools;
	private Dictionary<AudioChannel, int> _poolIndices;

	private int _currentMusicIndex = 0;
	private Coroutine _musicFadeCoroutine;

	public override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);

		// Map clip names to entries
		_bankMap = audioBank.ToDictionary(e => e.name);
		_pools = new Dictionary<AudioChannel, AudioSource[]>();
		_poolIndices = new Dictionary<AudioChannel, int>();

		CreatePool(AudioChannel.SFX, sfxPoolSize);
		CreatePool(AudioChannel.Music, musicPoolSize);
		CreatePool(AudioChannel.UI, uiPoolSize);
	}

	private void CreatePool(AudioChannel channel, int size)
	{
		var sources = new AudioSource[size];
		for (int i = 0; i < size; ++i)
		{
			var src = Instantiate(audioSourcePrefab, transform);
			src.playOnAwake = false;
			src.spatialBlend = 0f;
			sources[i] = src;
		}

		_pools[channel] = sources;
		_poolIndices[channel] = 0;
	}

	public void Play(string soundName, float? volumeOverride = null, float pitch = 1f)
	{
		if (!_bankMap.TryGetValue(soundName, out var entry))
		{
			Debug.LogWarning($"Missing audio clip: {soundName}");
			return;
		}

		var pool = _pools[entry.channel];
		int index = _poolIndices[entry.channel];
		var source = pool[index];

		source.clip = entry.clip;
		source.loop = entry.loop;
		source.volume = volumeOverride ?? entry.defaultVolume;
		source.pitch = pitch;
		source.Play();

		_poolIndices[entry.channel] = (index + 1) % pool.Length;
	}

	public void PlayMusicCrossfade(string musicName, float fadeDuration = 1f)
	{
		var musicSources = _pools[AudioChannel.Music];
		var currentSource = musicSources[_currentMusicIndex];

		if (string.IsNullOrWhiteSpace(musicName))
		{
			if (_musicFadeCoroutine != null)
				StopCoroutine(_musicFadeCoroutine);

			if (currentSource.isPlaying && currentSource.clip != null)
				_musicFadeCoroutine = StartCoroutine(FadeMusic_Internal(currentSource, null, 0f, fadeDuration));

			return;
		}

		if (!_bankMap.TryGetValue(musicName, out var entry) || entry.channel != AudioChannel.Music)
		{
			Debug.LogWarning($"Missing or invalid music track: {musicName}");
			return;
		}

		if (currentSource.isPlaying && currentSource.clip == entry.clip)
			return;

		int newIndex = (_currentMusicIndex + 1) % musicSources.Length;
		var fromSource = musicSources[_currentMusicIndex];
		var toSource = musicSources[newIndex];

		toSource.clip = entry.clip;
		toSource.loop = entry.loop;
		toSource.volume = 0f;
		toSource.Play();

		bool skipFadeOut = !fromSource.isPlaying || fromSource.clip == null;
		if (_musicFadeCoroutine != null)
			StopCoroutine(_musicFadeCoroutine);

		_musicFadeCoroutine = StartCoroutine(FadeMusic_Internal(
			skipFadeOut ? null : fromSource,
			toSource,
			entry.defaultVolume,
			fadeDuration
		));

		_currentMusicIndex = newIndex;
	}

	private IEnumerator FadeMusic_Internal(AudioSource from, AudioSource to, float targetVolume, float duration)
	{
		float t = 0f;
		float fromStartVol = from != null ? from.volume : 0f;

		while (t < duration)
		{
			float lerp = t / duration;

			if (from != null)
				from.volume = Mathf.Lerp(fromStartVol, 0f, lerp);

			if (to != null)
				to.volume = Mathf.Lerp(0f, targetVolume, lerp);

			t += Time.unscaledDeltaTime;
			yield return null;
		}

		if (from != null)
		{
			from.Stop();
			from.volume = 0f;
		}

		if (to != null)
			to.volume = targetVolume;
	}

	public void StopAll(AudioChannel channel)
	{
		foreach (var source in _pools[channel])
		{
			source.Stop();
		}
	}

	public void FadeOutAllMusic(float duration = 1f)
	{
		var musicSources = _pools[AudioChannel.Music];

		foreach (var src in musicSources)
		{
			if (src.isPlaying && src.clip != null)
			{
				StartCoroutine(FadeOut_Internal(src, duration));
			}
		}
	}

	private IEnumerator FadeOut_Internal(AudioSource source, float duration)
	{
		float startVolume = source.volume;
		float t = 0f;

		while (t < duration)
		{
			source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
			t += Time.unscaledDeltaTime;
			yield return null;
		}

		source.Stop();
		source.volume = 1f;
	}

}