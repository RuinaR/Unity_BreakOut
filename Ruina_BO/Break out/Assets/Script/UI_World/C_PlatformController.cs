using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_PlatformController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D m_rb;
	[SerializeField]
	private Slider m_slider;


	private Quaternion m_quaternionZero;


	public void Init()
	{
		m_quaternionZero = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		SetZero();
	}

	public void SetZero()
	{
		m_slider.value = 0;
		m_rb.SetRotation(0.0f);
	}

	public void SliderFunc()
	{
		m_rb.MoveRotation(m_slider.value);
	}

}
