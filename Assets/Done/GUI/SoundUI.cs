using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
	public Image sound_bar_fill;
	public Text sound_ratio;


	// Start is called before the first frame update
	public void Start()
	{
	}
	private void Update()

	{
		SoundBar();
	}



	public void SoundBar()

	{

		//float AudioVolume_now = AudioListener.volume; //볼륨값을 받아옴
		float AudioVolume_now = Mathf.Round(AudioListener.volume * 10) * 0.1f; //AudioVolume_now는 현재볼륨 값의 소숫점둘째자리에서 반올림한 값이다.

		sound_bar_fill.fillAmount = AudioVolume_now / 2;

		sound_ratio.text = string.Format("현재 볼륨 {0}/" + "2", AudioVolume_now);

		if (AudioVolume_now < 0)
		{
			AudioListener.volume = 0f;
		}
		if (AudioListener.volume > 2)
		{
			AudioListener.volume = 2f;
		}
	}

	public void sound_up()
	{
		AudioListener.volume = AudioListener.volume += 0.25f;
	}

	public void sound_down()
	{
		AudioListener.volume = AudioListener.volume -= 0.25f;
	}


}

