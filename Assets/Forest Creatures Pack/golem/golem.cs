using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golem : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		Animator anim = gameObject.GetComponent<Animator>();
		anim.Play("golem", -1, float.NegativeInfinity);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			Animator anim = gameObject.GetComponent<Animator>();
			// Reverse animation play
			anim.SetFloat("reverse", -1);
			anim.Play("golem", -1, float.NegativeInfinity);
		}
	}
}