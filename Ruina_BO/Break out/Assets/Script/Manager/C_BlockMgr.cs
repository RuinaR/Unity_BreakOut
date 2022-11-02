using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class C_BlockMgr : MonoBehaviour
{
	[SerializeField]
	private C_GameUI m_cGameUI;
	[SerializeField]
	private C_BlockPool m_cBlockPool;
	[SerializeField]
	private C_Block m_cOrigin;
	[SerializeField]
	private BoxCollider2D m_colDefault;
	[SerializeField]
	private int m_nCreateCount;
	[SerializeField]
	private Transform m_tfLeftCreatePos;
	[SerializeField]
	private Transform m_tfRightCreatePos;
	[SerializeField]
	private Text GameEndText;

	private Vector2 m_vec2CenterTopPos;
	private Vector2 m_vec2BlockSize;

	private List<C_Block> m_listBlock;
	private Vector2 m_Vec2MoveDistance;

	public void Init()
	{
		m_cBlockPool.Init(m_cOrigin);
		m_listBlock = m_cBlockPool.GetList();
		//m_Vec2MoveDistance = new Vector2(0.0f, -0.725f);

		m_vec2CenterTopPos = m_colDefault.transform.position;
		m_vec2BlockSize = m_colDefault.size;
		m_vec2BlockSize.x *= m_colDefault.transform.localScale.x;
		m_vec2BlockSize.y *= m_colDefault.transform.localScale.y;
		m_Vec2MoveDistance = new Vector2(0.0f, -1 * m_vec2BlockSize.y * 2.0f);
	}

	public void StartStage()
	{
		StartCoroutine(CoroutineStage());
	}
	
	public void EndStage()
	{
		StopAllCoroutines();
	}

	public void CreateBlockLine()
	{
		StartCoroutine(CoroutineCreateBlockLine());
	}

	private IEnumerator CoroutineStage()
	{

		int nLevel = C_GameMgr.Instance.GetLevel();
		int nStage = C_GameMgr.Instance.GetStage();

		float fUnitTime = 0.5f;

		WaitForSeconds UnitTime = new WaitForSeconds(fUnitTime);
		float fElapseTime = 0.0f;

		m_cGameUI.SetBlockCreateCount((C_BaseDataMgr.Instance.GetLevelData(nLevel).nBlockICreateCount.ToString()));

		
		while (true)
		{

			yield return UnitTime;
			fElapseTime += fUnitTime;
			m_cGameUI.SetStageTimer((C_BaseDataMgr.Instance.GetLevelData(nLevel).nStageTime - (int)fElapseTime).ToString());


			if (fElapseTime >= C_BaseDataMgr.Instance.GetLevelData(nLevel).nStageTime)
			{
				Debug.Log("게임 클리어");
				GameEndText.text = "게임 클리어~";
				GameEndText.gameObject.SetActive(true);
				C_GameMgr.Instance.GameEnd();
			}
		}
	}

	private IEnumerator CoroutineCreateBlockLine()
	{
		int nLevel = C_GameMgr.Instance.GetLevel();
		int nStage = C_GameMgr.Instance.GetStage();
		C_BlockData[] arBlockData = C_BaseDataMgr.Instance.GetBlockPool(nLevel, nStage);

		WaitForFixedUpdate MoveTime = new WaitForFixedUpdate();

		float fBlockMoveTime = 0.5f;
		float fElapseTime_MoveBlock = 0.0f;
		//블럭 라인 생성
		//	int nMin = -1 * (m_nCreateCount / 2);
		//	int nMax = m_nCreateCount + nMin;

		float fDistance = (m_tfRightCreatePos.position.x - m_tfLeftCreatePos.position.x) / (m_nCreateCount + 1);
		Vector2 vec2Pos;
		
		for (int i = 0; i < m_nCreateCount; i++)
		{
			float fRandomNoBlock = Random.Range(0.0f, 1.0f);
			if (!(fRandomNoBlock <= C_BaseDataMgr.Instance.GetLevelData(nLevel).fNoBlockProbability))
			{
				float fRandomRatio = Random.Range(0.0f, arBlockData[arBlockData.Length - 1].fRandomRatio);
				for (int j = 0; j < arBlockData.Length; j++)
				{

					if ((j == 0 && (fRandomRatio >= 0.0f && fRandomRatio <= arBlockData[j].fRandomRatio)) ||
						(j != 0 && (fRandomRatio >= arBlockData[j - 1].fRandomRatio && fRandomRatio <= arBlockData[j].fRandomRatio)))
					{
						vec2Pos = m_vec2CenterTopPos;
						vec2Pos.x = m_tfLeftCreatePos.position.x + (fDistance * (i + 1));

						ActiveBlock(
								arBlockData[j].isReinforce,
								arBlockData[j].eType,
								vec2Pos);
						break;
					}
				}
			}
		}
		//블럭 라인 이동
		m_cGameUI.SetBlockCreateCount("x_x");
			Vector2 StartPos = m_cBlockPool.transform.position;
			while (fElapseTime_MoveBlock <= fBlockMoveTime)
			{
				yield return MoveTime;
				fElapseTime_MoveBlock += Time.fixedDeltaTime;

				for (int i = 0; i < m_listBlock.Count; i++)
				{
					if (m_listBlock[i].gameObject.activeInHierarchy)
					{
						m_listBlock[i].GetRb().MovePosition
							(m_listBlock[i].GetRb().position + (m_Vec2MoveDistance * Time.fixedDeltaTime * 1.0f / fBlockMoveTime));
					}
				}
			}

	}

	public void ActiveBlock(/*bool sLeft, bool sRight, bool sUp, bool sDown, */bool isReinforce, E_Block_Type eType, Vector2 pos)
	{
		C_Block cBlock = m_cBlockPool.GetFromPool();

		cBlock.SetBlockAll(/*sLeft, sRight, sUp, sDown,*/ isReinforce, eType);
		cBlock.transform.position = pos;
		cBlock.Active();
	}

	public void AllBlockSetActiveFalse()
	{
		m_cBlockPool.AllSetActiveFalse();
	}
}
