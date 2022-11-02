using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Block : MonoBehaviour
{
	private delegate void Attack();

	[SerializeField]
	private Rigidbody2D m_rb;
	[SerializeField]
	private SpriteRenderer m_sr;
	//[SerializeField]
	//private SpriteRenderer m_Weapon_sr;
	//[SerializeField]
	//private GameObject m_Shield_Left;
	//[SerializeField]
	//private GameObject m_Shield_Right;
	//[SerializeField]
	//private GameObject m_Shield_Up;
	//[SerializeField]
	//private GameObject m_Shield_Down;
	[SerializeField]
	private SpriteRenderer m_HpBar;

	[SerializeField]
	private bool m_isReinforce;
	[SerializeField]
	E_Block_Type m_eType;
	[SerializeField]
	private int m_nMaxHp;
	[SerializeField]
	private int m_nHp;

	//private Attack[] m_delAttack;
	private Coroutine m_attackCoroutine;
	private Vector2 m_vec2HpBarSizeOrigin;
	private Vector2 m_vec2HpBarSize;

	[SerializeField]
	private BoxCollider2D m_col;
	[SerializeField]
	private Sprite[] m_arBlockSprite;
	private float fHalfSizeX;
	private float fHalfSizeY;

	private float m_fShortestDist;
	private float m_fTempDist;
	private Quaternion m_qDefault;
	private Quaternion m_qRotate;

	private ContactPoint2D[] m_arContactPoint2D;


	private void Awake()
	{
		m_arContactPoint2D = new ContactPoint2D[5];
		m_qDefault = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		m_qRotate = new Quaternion(0.0f, 0.0f, 45.0f, 0.0f);
		fHalfSizeX = transform.localScale.x * m_col.size.x * 0.5f;
		fHalfSizeY = transform.localScale.y * m_col.size.y * 0.5f;


		m_vec2HpBarSizeOrigin = m_HpBar.size;

		//m_delAttack = new Attack[(int)E_Block_Type.Max];
		//m_delAttack[(int)E_Block_Type.Cannon_0] = Del_Attack_Cannon_1;
		//m_delAttack[(int)E_Block_Type.Cannon_1] = Del_Attack_Cannon_2;
		//m_delAttack[(int)E_Block_Type.Cannon_2] = Del_Attack_Cannon_3;

		//m_delAttack[(int)E_Block_Type.Laser_0] = Del_Attack_Laser_1;
		//m_delAttack[(int)E_Block_Type.Laser_1] = Del_Attack_Laser_2;
		//m_delAttack[(int)E_Block_Type.Laser_2] = Del_Attack_Laser_3;
	}

	public void Active()
	{
		gameObject.SetActive(true);
		if (m_eType != E_Block_Type.None)
		{
			//m_attackCoroutine = StartCoroutine(coroutineAttack());
		}
		m_HpBar.size = m_vec2HpBarSizeOrigin;
		m_vec2HpBarSize = m_vec2HpBarSizeOrigin;
	}

	public void Deactivation()
	{
		if (m_eType != E_Block_Type.None)
		{
			//StopCoroutine(m_attackCoroutine);
		}
		gameObject.SetActive(false);
	}

	public void GetDMG(int nDmg)
	{
		C_Text_DMGPool.Instance.GetFromPool().SetTextAndActive(gameObject.transform.position, nDmg.ToString());
		if (m_nHp <= nDmg)
		{
			m_nHp = 0;
			m_vec2HpBarSize.x = 0.0f;
			m_HpBar.size = m_vec2HpBarSize;
			Deactivation();
		}
		else
		{
			m_nHp -= nDmg;
			m_vec2HpBarSize.x = ((float)m_nHp / (float)m_nMaxHp) * m_vec2HpBarSizeOrigin.x;
			m_HpBar.size = m_vec2HpBarSize;
		}
	}

	public Rigidbody2D GetRb()
	{
		return m_rb;
	}

	public void SetBlockAll(/*bool sLeft, bool sRight, bool sUp, bool sDown, */bool isReinforce, E_Block_Type eType)
	{
		//SetShield_Left(sLeft);
		//SetShield_Right(sRight);
		//SetShield_Up(sUp);
		//SetShield_Down(sDown);

		SetReinforce(isReinforce);
		m_eType = eType;
		switch(m_eType)
		{
			case E_Block_Type.None:
				{
					transform.rotation = m_qDefault;
					m_sr.sprite = m_arBlockSprite[(int)E_Block_Type.None];
					gameObject.GetComponent<CircleCollider2D>().enabled = false;
					gameObject.GetComponent<PolygonCollider2D>().enabled = false;
					gameObject.GetComponent<BoxCollider2D>().enabled = true;

					break;
				}
			case E_Block_Type.Tri:
				{
					transform.rotation = m_qDefault;
					m_sr.sprite = m_arBlockSprite[(int)E_Block_Type.Tri];
					gameObject.GetComponent<BoxCollider2D>().enabled = false;
					gameObject.GetComponent<CircleCollider2D>().enabled = false;
					gameObject.GetComponent<PolygonCollider2D>().enabled = true;

					break;
				}
			case E_Block_Type.Cir:
				{
					transform.rotation = m_qDefault;
					m_sr.sprite = m_arBlockSprite[(int)E_Block_Type.Cir];
					gameObject.GetComponent<BoxCollider2D>().enabled = false;
					gameObject.GetComponent<PolygonCollider2D>().enabled = false;
					gameObject.GetComponent<CircleCollider2D>().enabled = true;

					break;
				}
			default:
				{
					Debug.Log("에러");
					break;
				}
		}

		//SetWeapon(eType);
	}

	//public void SetShield_Left(bool shield)
	//{
	//	if (shield)
	//	{
	//		m_Shield_Left.SetActive(true);
	//	}
	//	else
	//	{
	//		m_Shield_Left.SetActive(false);
	//	}
	//}
	//public void SetShield_Right(bool shield)
	//{
	//	if (shield)
	//	{
	//		m_Shield_Right.SetActive(true);
	//	}
	//	else
	//	{
	//		m_Shield_Right.SetActive(false);
	//	}
	//}
	//public void SetShield_Up(bool shield)
	//{
	//	if (shield)
	//	{
	//		m_Shield_Up.SetActive(true);
	//	}
	//	else
	//	{
	//		m_Shield_Up.SetActive(false);
	//	}
	//}
	//public void SetShield_Down(bool shield)
	//{
	//	if (shield)
	//	{
	//		m_Shield_Down.SetActive(true);
	//	}
	//	else
	//	{
	//		m_Shield_Down.SetActive(false);
	//	}
	//}

	public void SetReinforce(bool isReinforce)
	{
		m_isReinforce = isReinforce;
		if (m_isReinforce)
		{
			m_sr.color = Color.blue;
			//m_sr.sprite = C_SpriteMgr.Instance.GetBlock(C_GameMgr.Instance.GetStage(), E_Block_IMG.Block_Reinforce);
			m_nMaxHp = (int)((float)C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nDefaultBlockMaxHp *
								 C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).fReinforceIncrease);
		}
		else
		{
			m_sr.color = Color.white;
			//m_sr.sprite = C_SpriteMgr.Instance.GetBlock(C_GameMgr.Instance.GetStage(), E_Block_IMG.Block_Default);
			m_nMaxHp = C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nDefaultBlockMaxHp;
		}
		m_nHp = m_nMaxHp;
	}

	//public void SetWeapon(E_Block_Type eType)
	//{
	//	m_eType = eType;
	//	if (eType == E_Block_Type.None)
	//	{
	//		m_Weapon_sr.sprite = C_SpriteMgr.Instance.GetNone();
	//		return;
	//	}
	//	m_Weapon_sr.sprite = C_SpriteMgr.Instance.GetBlockWeapon(eType, E_Block_Weapon_State.Default);
	//}

	//IEnumerator coroutineAttack()
	//{
	//	float fUnitTime = 0.1f;

	//	WaitForSeconds UnitTime = new WaitForSeconds(fUnitTime);
	//	float fAttackTime = C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nBlockAttackTime;
	//	float felapseTime = 0.0f;
	//	while(true)
	//	{
	//		yield return UnitTime;
	//		felapseTime += fUnitTime;
	//		if (felapseTime >= fAttackTime - 1.0f)
	//		{
	//			if (m_Weapon_sr.sprite != C_SpriteMgr.Instance.GetBlockWeapon(m_eType, E_Block_Weapon_State.launch))
	//			{
	//				m_Weapon_sr.sprite = C_SpriteMgr.Instance.GetBlockWeapon(m_eType, E_Block_Weapon_State.launch);
	//			}
	//		}
	//		if (felapseTime >= fAttackTime)
	//		{
	//			m_delAttack[(int)m_eType]();
	//			m_Weapon_sr.sprite = C_SpriteMgr.Instance.GetBlockWeapon(m_eType, E_Block_Weapon_State.Default);
	//			felapseTime = 0.0f;
	//		}
	//	}
	//}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.tag == "DestroyBlockArea")
		{
			C_Effect effect = C_EffectMgr.Instance.efPool_Dmg.GetFromPool();
			effect.gameObject.transform.position = gameObject.transform.position;
			effect.gameObject.SetActive(true);
			C_Ball.Instance.GetDMG(20);
			Deactivation();

		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ball")
		{
			collision.GetContacts(m_arContactPoint2D);

			//E_Dir eDir = CheckColSide(m_arContactPoint2D[0].normal);

			C_Ball.Instance.OnColBlock(this, m_arContactPoint2D[0].normal);

			if(C_Ball.Instance.GetIsBurst())
			{
				C_Effect effect = C_EffectMgr.Instance.efPool_B.GetFromPool();
				effect.gameObject.transform.position = collision.transform.position;
				effect.gameObject.SetActive(true);
				C_Camera.Instance.ShakeCamera(0.15f, 0.3f);
			}
			else
			{
				C_Effect effect = C_EffectMgr.Instance.efPool_N.GetFromPool();
				effect.gameObject.transform.position = collision.transform.position;
				effect.gameObject.SetActive(true);
				C_Camera.Instance.ShakeCamera(0.15f, 0.1f);
			}

			GetDMG(C_Ball.Instance.GetPower());

		}
	}

	//private E_Dir CheckColSide(Vector2 vec2)
	//{

	//	if (vec2.y > -0.5f && vec2.y <= 0.5f)
	//	{
	//		if (vec2.x > 0.5f && vec2.x <= 1.5f)
	//		{
	//			//Debug.Log("Right");
	//			return E_Dir.Right;
	//		}
	//		if (vec2.x > -1.5f && vec2.x <= -0.5f)
	//		{
	//			//Debug.Log("Left");
	//			return E_Dir.Left;
	//		}
	//	}
	//	else if (vec2.x > -0.5f && vec2.x <= 0.5f)
	//	{
	//		if (vec2.y > 0.5f && vec2.y <= 1.5f)
	//		{
	//			//Debug.Log("Up");
	//			return E_Dir.Up;
	//		}
	//		if (vec2.y > -1.5f && vec2.y <= -0.5f)
	//		{
	//			//Debug.Log("Down");
	//			return E_Dir.Down;
	//		}
	//	}
	//	else if (vec2.y > 0)
	//	{
	//		Debug.Log(vec2.x.ToString() + "," + vec2.y.ToString());
	//		return E_Dir.Up;
	//	}
	//	else
	//	{
	//		Debug.Log(vec2.x.ToString() + "," + vec2.y.ToString());
	//		return E_Dir.Down;
	//	}



	//	return E_Dir.Max;

	//}



	//E_Dir e_Dir;
	//m_fShortestDist =
	//	Mathf.Abs(C_Ball.Instance.transform.position.x + (C_Ball.Instance.transform.localScale.x * C_Ball.Instance.GetRadius())
	//			- transform.position.x - fHalfSizeX);
	//e_Dir = E_Dir.Right;
	//m_fTempDist =
	//	Mathf.Abs(C_Ball.Instance.transform.position.x - (C_Ball.Instance.transform.localScale.x * C_Ball.Instance.GetRadius())
	//			- transform.position.x + fHalfSizeX);
	//if(m_fShortestDist > m_fTempDist)
	//{
	//	m_fShortestDist = m_fTempDist;
	//	e_Dir = E_Dir.Left;
	//}
	//m_fTempDist =
	//	Mathf.Abs(C_Ball.Instance.transform.position.y - (C_Ball.Instance.transform.localScale.y * C_Ball.Instance.GetRadius())
	//			- transform.position.y + fHalfSizeY);
	//if(m_fShortestDist > m_fTempDist)
	//{
	//	m_fShortestDist = m_fTempDist;
	//	e_Dir = E_Dir.Down;
	//}
	//m_fTempDist =
	//	Mathf.Abs(C_Ball.Instance.transform.position.y + (C_Ball.Instance.transform.localScale.y * C_Ball.Instance.GetRadius())
	//			- transform.position.y - fHalfSizeY);
	//if(m_fShortestDist > m_fTempDist)
	//{
	//	m_fShortestDist = m_fTempDist;
	//	e_Dir = E_Dir.Up;
	//}
	//return e_Dir;


//private void Del_Attack_Cannon_1()
//	{
//		//Debug.Log("캐논 1 공격");
//	}
//	private void Del_Attack_Cannon_2()
//	{
//		//Debug.Log("캐논 2 공격");
//	}
//	private void Del_Attack_Cannon_3()
//	{
//		//Debug.Log("캐논 3 공격");
//	}

//	private void Del_Attack_Laser_1()
//	{
//		//Debug.Log("레이저 1 공격");
//	}
//	private void Del_Attack_Laser_2()
//	{
//		//Debug.Log("레이저 2 공격");
//	}
//	private void Del_Attack_Laser_3()
//	{
//		//Debug.Log("레이저 3 공격");
//	}
}
