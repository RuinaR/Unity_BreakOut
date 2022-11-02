using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class C_GameMgr : MonoBehaviour
{
	public static C_GameMgr Instance;

	[SerializeField]
	private Animator m_mainAnimator;

	[SerializeField]
	C_StartUI m_cStartUI;
	[SerializeField]
	C_GameUI m_cGameUI;
	[SerializeField]
	C_Ball m_cBall;

	[SerializeField]
	private C_BlockMgr m_cBlockMgr;

	[SerializeField]
	public bool IsGame;
	[SerializeField]
	private int m_nLevel;
	[SerializeField]
	private int m_nStage;
	[SerializeField]
	private Text gameEndText;
	[SerializeField]
	private C_Line line;



	private void Init()
	{
		C_Camera.Instance.Init();
		//Time.timeScale = 0.33f;
		////////////////////
		SetLevel(1);
		SetStage(2);

		C_BaseDataMgr.Instance.Init();
		C_SpriteMgr.Instance.Init();

		m_cStartUI.Init();
		m_cGameUI.Init();

		m_cBlockMgr.Init();
		m_cBall.Init();

		IsGame = false;
		
	}

	public void GameStart_Btn()
	{
		gameEndText.gameObject.SetActive(false);
		IsGame = true;
		m_mainAnimator.SetTrigger("StartToGame");

		m_cGameUI.SetStageTimer("");
		m_cGameUI.SetBlockCreateCount("");

		m_cGameUI.SetPlatformZero();

		m_cGameUI.SetBallMaxHp(C_BaseDataMgr.Instance.GetLevelData(m_nLevel).nDefaultBallMaxHp);
		m_cGameUI.SetBallHp(C_BaseDataMgr.Instance.GetLevelData(m_nLevel).nDefaultBallMaxHp);

		m_cBall.SetDefaultData();
		m_cBall.SetDefaultPos();
		


	}

	public void GameActive()
	{
		m_cBlockMgr.StartStage();
		m_cBall.StartGame();
		m_cBall.Set_IsActive(true);
	}

	public void GameEnd()
	{
		IsGame = false;
		m_mainAnimator.SetTrigger("GameToStart");

		m_cBlockMgr.AllBlockSetActiveFalse();
		m_cBlockMgr.EndStage();

		//line.ClearLine();
		m_cBall.EndGame();
	}

	public void SetLevel(int nLevel)
	{
		m_nLevel = nLevel;
	}
	public int GetLevel()
	{
		return m_nLevel;
	}
	public void SetStage(int nStage)
	{
		m_nStage = nStage;
	}
	public int GetStage()
	{
		return m_nStage;
	}


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

	void Start()
    {
		Init();

		//////////////////////////////////////////////////////////////////////////////////////////////////////
		//int tmpLevel = 3;
		//int tmpRound = 1;

		//for (int i = 0; i < C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound).Length; i++)
		//{
		//	Debug.Log("-----------------------------------" + (i + 1).ToString() + "-------------------------------------");
		//	Debug.Log(C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound)[i].eType.ToString() + " 타입");
		//	Debug.Log(C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound)[i].isReinforce.ToString() + " 강화유무");
		//	Debug.Log(C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound)[i].bLeftShield.ToString() + " 왼쪽실드");
		//	Debug.Log(C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound)[i].bRightShield.ToString() + " 오른쪽실드");
		//	Debug.Log(C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound)[i].bUpShield.ToString() + " 위쪽실드");
		//	Debug.Log(C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound)[i].bDownShield.ToString() + " 아래쪽실드");
		//	Debug.Log(C_BaseDataMgr.Instance.GetBlockPool(tmpLevel, tmpRound)[i].fRandomRatio.ToString() + " 랜덤비율");
		//}
	}


	/////////////////////////////////////////////////////////////////////////////////////////

	public void TestFunc()
	{
		GameEnd();
	}
}
