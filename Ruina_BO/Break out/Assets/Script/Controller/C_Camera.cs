using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Camera : MonoBehaviour
{
	public static C_Camera Instance;
	private Vector3 m_vec3ORiginPosition;

	private IEnumerator Shake(float fShakeTime, float fShakeAmount)
	{
		float fElapsedTime = 0.0f;
		while (fElapsedTime < fShakeTime)
		{
			Vector3 vec3RandomPoint = m_vec3ORiginPosition + Random.insideUnitSphere * fShakeAmount;
			transform.localPosition = Vector3.Lerp(transform.localPosition, vec3RandomPoint, fElapsedTime / fShakeTime);
			yield return null;
			fElapsedTime += Time.deltaTime;
		}

		transform.localPosition = m_vec3ORiginPosition;
	}

	public void Init()
	{
		m_vec3ORiginPosition = gameObject.transform.position;
	}

	public void ShakeCamera(float fShakeTime, float fShakeAmount)
	{
		StartCoroutine(Shake(fShakeTime, fShakeAmount));
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}
}
