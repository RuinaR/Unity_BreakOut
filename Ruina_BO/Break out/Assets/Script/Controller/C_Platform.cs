using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Platform : MonoBehaviour
{
	private ContactPoint2D[] m_arContactPoint2D;
	private Vector2 m_vec2Normal;

	private void Awake()
	{
		m_arContactPoint2D = new ContactPoint2D[5];
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ball")
		{

			collision.GetContacts(m_arContactPoint2D);

			if (m_arContactPoint2D[1].normal != Vector2.zero)
			{
				Vector2 vec2Sum = (m_arContactPoint2D[0].normal * (-1.0f)) + (m_arContactPoint2D[1].normal * (-1.0f));
				m_vec2Normal = vec2Sum.normalized;
				Debug.Log("이중충돌");

			}
			else
			{
				m_vec2Normal = m_arContactPoint2D[0].normal * (-1.0f);
			}
			C_Ball.Instance.OnColPlatform(m_vec2Normal);

		}
	}
}
