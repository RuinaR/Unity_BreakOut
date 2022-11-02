using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_BlockPool : C_Pool<C_Block>
{
	public override void AllSetActiveFalse()
	{
		for (int i = 0; i < m_listPool.Count; i++)
		{
			if (m_listPool[i].gameObject.activeInHierarchy)
			{
				m_listPool[i].Deactivation();
			}
		}
	}
}
