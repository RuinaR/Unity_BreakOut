using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Frame : MonoBehaviour
{
	[SerializeField]
	private E_Dir m_eColDir;

	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ball")
		{

			C_Ball.Instance.OnColFrame(m_eColDir);
		}
	}
}

