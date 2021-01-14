using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	/* object refs */
	public GameObject AudioListener;

	/* prefabs */
	public AudioClip BGM;
	public AudioClip Tap1;
	public AudioClip Tap2;
	public AudioClip TapOnItem;
	public AudioClip Builder;
	public AudioClip Button;
	public AudioClip Collect;
	public AudioClip Yeah;

	public AudioClip Sword;
	public AudioClip Bow;

	public void Awake()
	{
		instance = this;
	}
    

	public void PlaySound(AudioClip clip, bool loop)
	{
		if (clip == null)
			return;

		AudioSource source = this.AudioListener.AddComponent<AudioSource>();
		source.clip = clip;
		source.Play();
		source.loop = loop;

		if (!loop)
			this.StartCoroutine(this._DestroyAfterPlay(source));
	}

	private IEnumerator _DestroyAfterPlay(AudioSource source)
	{
		yield return new WaitForSeconds(source.clip.length);
		Destroy(source);
	}

	public void StopAllSounds()
	{
		foreach (AudioSource source in this.AudioListener.GetComponents<AudioSource>())
		{
			Destroy(source);
		}
	}

}
