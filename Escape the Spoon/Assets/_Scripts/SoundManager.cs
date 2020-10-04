using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Sound
{
#region classes

public interface IPlayableSound
{
	void Play();
	void Play(Vector3 _position);
	void Play(Vector3 _position, float _override);
}


[System.Serializable]
public class PlayableSound : IPlayableSound
{
	public string name;
	public AudioSource source;
	[Tooltip("Needs to contain the initial clip, if exists. Initial clip not required")]
	public List<AudioClip>  alternativeClips;

	private float defaultPitch;
	private float defaultVolume;

	public void Initialize()
	{
		defaultPitch = source.pitch;
		defaultVolume = source.volume;
	}
	
	public void Play()
	{ 
		PlaySound();
	}

	public void Play(Vector3 _position)
	{
		source.transform.position = _position;
		PlaySound();
	}

	public void Play(Vector3 _position, float _override)
	{
		source.volume = defaultVolume * _override;
		source.pitch = defaultPitch * _override;

		source.transform.position = _position;

		PlaySound();
	}

	//Change are made to the AudioSource
	private void PlaySound()
	{

		if (alternativeClips.Count != 0)
		{
			int sad = Random.Range(0, alternativeClips.Count - 1);
			source.clip = alternativeClips[sad];
		}
	
		source.Play();
	}

	public void AdjustValues(float _volume, float _pitch)
	{
		source.volume = _volume;
		source.pitch = _pitch;
	}
}


#endregion


public class SoundManager : MonoBehaviour
{
	public static SoundManager Self { get; private set; }
	[SerializeField] private List<PlayableSound> Sounds;
	
	private List<PlayableSound> loopers = new List<PlayableSound>();
	private List<float> timers = new List<float>();
	[HideInInspector] public bool running = false;
	
	void Awake()
	{
		if(Self != null && Self != this)
		{
			Destroy(gameObject);
		}
		Self = this;
		
		
		foreach (PlayableSound _sound in Sounds)
		{
			_sound.Initialize();
		}
	}
	
	//void Start () {
	//	running = true;
	//	StartCoroutine(SlowUpdate());
	//}

	public void PlaySound(string _name, Vector3 _position)
	{
		GetSoundByName(_name).Play(_position);
	}

	public void PlaySound(string _name, Vector3 _position, float _speedInput)
	{
		GetSoundByName(_name).Play(_position, _speedInput);
	}

	public void PlaySound(string _name)
	{
		GetSoundByName(_name).Play();
	}

	public void PlayAsLoop(string _name, Vector3 _pos, float _override)
	{
		var s = GetSoundByName(_name).source;
		if (s.loop == true)
		{
			Debug.LogError("Source already playing as loop");
			return;
		}

		if (s.isPlaying)
		{
			s.Stop();
		}


		AdjustSource(s, _override);

		s.transform.position = _pos;
		s.loop = true;
		s.Play();
	}
	
	public void PlayAsLoop(string _name, Vector3 _pos)
	{
		var s = GetSoundByName(_name).source;
		if (s.loop)
		{
			return;
		}

		if (s.isPlaying)
		{
			s.Stop();
		}

		s.transform.position = _pos;
		s.loop = true;
		s.Play();
	}
	
	public void PlayAsLoop(string _name, float _override)
	{
		var s = GetSoundByName(_name).source;
		if (s.loop == true)
		{
			Debug.LogError("Source already playing as loop");
			return;
		}

		if (s.isPlaying)
		{
			s.Stop();
		}
		
		AdjustSource(s, _override);

		s.loop = true;
		s.Play();
	}
	
	public void PlayAsLoop(string _name)
	{
		var s = GetSoundByName(_name).source;
		if (s.loop == true)
		{
			Debug.LogError("Source already playing as loop");
			return;
		}

		if (s.isPlaying)
		{
			s.Stop();
		}
		
		s.loop = true;
		s.Play();
	}

	public void StopLoop(string _name)
	{
		var s = GetSoundByName(_name).source;
		s.Stop();
		s.loop = false;
	}
	
	public void AdjustSource(string _name, float _volume, float _pitch)
	{
		var s = GetSoundByName(_name).source;
		s.volume = _volume;
		s.pitch = _pitch;
	}
	
	public void AdjustSource(string _name, float _override)
	{
		AdjustSource(GetSoundByName(_name).source, _override);;
	}

	private void AdjustSource(AudioSource _source, float _override)
	{
		_source.volume = _override;
		_source.pitch = _override;
	}

	public void AdjustPitchVolume(string _name, float _pitch, float _volume)
	{
		var s = GetSoundByName(_name).source;
		s.pitch = _pitch;
		s.volume = _volume;
	}
	
	public void AdjustVolume(string _name, float _volume)
	{
		var s = GetSoundByName(_name).source;
		s.volume = _volume;
	}
	
	private PlayableSound GetSoundByName(string _name)
	{
		foreach (var t in Sounds)
		{
			if (t.name == _name)
			{
				return t;
			}
		}
		Debug.Log("No sound with this name was found: " + _name);
		return null;
	}
	
	
	//public void PlayLoop(string _name)
	//{
	//	for (int i = 0; i < Sounds.Count; i++)
	//	{
	//		if (Sounds[i].name == _name)
	//		{
	//			loopers.Add(Sounds[i]);
	//			timers.Add(0);
	//			Sounds[i].Play();
	//			return;
	//		}
	//	}
	//	
	//	Debug.Log("No sound with this name was found: " + _name);
	//}

	//public void CancelLoop(string _name)
	//{
	//	for (int i = 0; i < loopers.Count; i++)
	//	{
	//		if (loopers[i].name == _name)
	//		{
	//			loopers.RemoveAt(i);
	//			timers.RemoveAt(i);
	//			return;
	//		}
	//	}
//
	//	Debug.Log("No loop to cancel with this name was found: " + _name);
	//}
	

	//IEnumerator SlowUpdate()
	//{
	//	while (running)
	//	{
	//		for (int i = 0; i < loopers.Count; i++)
	//		{
	//			timers[i] += 0.2f;
	//			
	//			if (!(timers[i] > loopers[i].source.clip.length)) continue;
	//			timers[i] = 0;
	//			loopers[i].Play();
	//		}
	//		yield return new WaitForSeconds(0.2f);
	//	}
	//}
}

}
