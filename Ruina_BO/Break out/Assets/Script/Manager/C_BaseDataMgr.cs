using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class C_BaseDataMgr : MonoBehaviour
{
	public static C_BaseDataMgr Instance;

	[SerializeField]
	private int m_nStageMax = 2;
	[SerializeField]
	private int m_nLevelMax = 3;

	private C_BlockData[][][] m_arBlockData;
	private C_LevelData[] m_arLevelData;

	private float m_fRayMaxDistance;
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
		m_fRayMaxDistance = 100.0f;

		m_arBlockData = new C_BlockData[m_nLevelMax][][];
		for (int i = 0; i < m_arBlockData.Length; i++)
		{
			m_arBlockData[i] = new C_BlockData[m_nStageMax][];
			for (int j = 0; j < m_arBlockData[i].Length; j++)
			{
				string strJson = (Resources.Load("BaseData/StageBlockPoolData" +
											     "/Level_" + (i + 1).ToString() +
											     "/BlockPool_Stage_" + (j + 1).ToString()) as TextAsset).text;
				m_arBlockData[i][j] = C_JsonHelper.FromJson<C_BlockData>(strJson);
			}
		}

		m_arLevelData = new C_LevelData[m_nLevelMax];
		for (int i = 0; i < m_arLevelData.Length; i++)
		{
			string strJson = (Resources.Load("BaseData/LevelData" +
											 "/LevelData_" + (i + 1).ToString()) as TextAsset).text;
			m_arLevelData[i] = JsonUtility.FromJson<C_LevelData>(strJson);
		}

	}

	public C_BlockData[] GetBlockPool(int nLevel, int nStage)
	{
		if(nLevel < 0 || nLevel > m_nLevelMax)
		{
			return null;
		}
		if (nStage < 0 || nStage > m_nStageMax)
		{
			return null;
		}

		return m_arBlockData[nLevel - 1][nStage - 1];
	}

	public C_LevelData GetLevelData(int nLevel)
	{
		if(nLevel < 1 || nLevel > m_nLevelMax)
		{
			return null;
		}
		return m_arLevelData[nLevel - 1];
	}
	public float GetRayMaxDistance()
	{
		return m_fRayMaxDistance;
	}

	public int GetLevelMax()
	{
		return m_nLevelMax;
	}

	public int GetStageMax()
	{
		return m_nStageMax;
	}
}
