using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class C_GameUI : MonoBehaviour
{
	[SerializeField]
	private Text m_tStageTimer;
	[SerializeField]
	private Text m_tBlockCreateCount;
	[SerializeField]
	private C_PlatformController m_cPlatform;
	[SerializeField]
	private Slider m_sliderBallHp;

	public void Init()
	{
		m_cPlatform.Init();
		SetStageTimer("");
		SetBlockCreateCount("");

		m_sliderBallHp.minValue = 0;
		SetBallMaxHp(C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nDefaultBallMaxHp);
		SetBallHp(C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nDefaultBallMaxHp);
	}

	public void SetStageTimer(string str)
	{
		m_tStageTimer.text = str;
	}

	public void SetBlockCreateCount(string str)
	{
		m_tBlockCreateCount.text = str;
	}

	public void SetPlatformZero()
	{
		m_cPlatform.SetZero();
	}
	public void SetBallMaxHp(int nMaxHp)
	{
		m_sliderBallHp.maxValue = nMaxHp;
	}
	public void SetBallHp(int nHp)
	{
		m_sliderBallHp.value = nHp;
	}
}
