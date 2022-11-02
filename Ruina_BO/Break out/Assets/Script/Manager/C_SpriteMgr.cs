using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class C_SpriteMgr : MonoBehaviour
{
	public static C_SpriteMgr Instance;

	[SerializeField]
	private Sprite m_None;

	private Sprite[][] m_arBlock;
	//private Sprite[][] m_arBlock_Weapon;

	private Sprite[] m_arBall;
	private Sprite[] m_arBall_Attack;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void Init()
	{
		m_arBlock = new Sprite[C_BaseDataMgr.Instance.GetStageMax()][];
		for (int i = 0; i < m_arBlock.Length; i++)
		{
			m_arBlock[i] = Resources.LoadAll<Sprite>("IMG/Block/Stage_" + (i + 1).ToString());
		}

		//m_arBlock_Weapon = new Sprite[(int)E_Block_Type.Max][];
		//for (int i = 0; i < m_arBlock_Weapon.Length; i++)
		//{
		//	m_arBlock_Weapon[i] = Resources.LoadAll<Sprite>("IMG/Block_Weapon/Weapon_" + (i + 1).ToString());
		//}

		m_arBall = Resources.LoadAll<Sprite>("IMG/Ball");
		m_arBall_Attack = Resources.LoadAll<Sprite>("IMG/Ball_Attack");
	}

	public Sprite GetBlock(int nStage, E_Block_IMG eImg)
	{
		if(nStage < 1 || nStage > C_BaseDataMgr.Instance.GetStageMax())
		{
			Debug.Log(nStage.ToString());
			return null;
		}
		return m_arBlock[nStage - 1][(int)eImg];
	}

	//public Sprite GetBlockWeapon(E_Block_Type eType, E_Block_Weapon_State eState)
	//{
	//	return m_arBlock_Weapon[(int)eType][(int)eState];
	//}

	public Sprite GetBall(E_Sprite_Ball eBall)
	{
		return m_arBall[(int)eBall];
	}

	public Sprite[] GetBall_Attack()
	{
		return m_arBall_Attack;
	}

	public Sprite GetNone()
	{
		return m_None;
	}
}
