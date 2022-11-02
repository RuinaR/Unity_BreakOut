using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class C_Ball : MonoBehaviour
{
	public static C_Ball Instance;

	private bool m_isActive;

	[SerializeField]
	private C_BlockMgr m_cBlockMgr;
	[SerializeField]
	private C_GameUI m_cGameUI;
    [SerializeField]
    private Rigidbody2D m_rb;
	[SerializeField]
	private SpriteRenderer m_sr;
	[SerializeField]
	private Transform m_tfRender;
	[SerializeField]
	private Transform m_tfRayPos;
	[SerializeField]
	private C_Line m_line;
	[SerializeField]
	private GameObject Dark;
	[SerializeField]
	private C_Text_BurstCntPool cBurstTextPool;
	[SerializeField]
	private C_BurstCnt cOrigin;
	[SerializeField]
	private GameObject m_cBurstImg;
	[SerializeField]
	private C_Text_DMG cDmgOrigin;
	[SerializeField]
	private GameObject Red;
	[SerializeField]
	private Text GameEndText;

	private bool m_bIsBurst;
	private int m_nBurstMaxCnt;
	private int m_nBurstCnt;
	private WaitForFixedUpdate m_MoveTime;
	private ContactPoint2D[] m_arContactPoint2D;

	private float m_fRadius;
	private RaycastHit2D m_hit;

	private Vector2 m_vec2Reflect;

	private Sprite[] m_arSpriteAttack;
	private WaitForSeconds m_attackUnitTime;

	private int m_nBlockCreateCount;

	private int m_nMaxHp;
	private int m_nHp;
	private int m_nPower;
	private float m_fSpeed;
	private float m_fInvincibilityTime;

	private bool m_isInvincibility;

	private Vector2 m_vec2Dir;
	private bool m_bTimeSlow;
	//private bool m_bFrameCollisionCheck;
	private bool m_isAttack;
	//private bool[] m_arCanCol;
	//private WaitForSeconds m_CanColTime;
	private Coroutine m_coroutineLine;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			Destroy(gameObject);
		}
	}
	public void StartGame()
	{
		m_nBurstCnt = m_nBurstMaxCnt;
		gameObject.SetActive(true);
		m_bIsBurst = false;
		m_rb.simulated = true;
		SetSprite(C_SpriteMgr.Instance.GetBall(E_Sprite_Ball.Move_Start));
		SetDir(Vector2.down);
		//SetFrameCheck(false);
		m_isAttack = false;
		m_nBlockCreateCount = 1;

		MoveStart();
	}
	public void Init()
	{
		C_Text_DMGPool.Instance.Init(cDmgOrigin);
		m_nBurstMaxCnt = 6;
		m_nBurstCnt = m_nBurstMaxCnt;
		cBurstTextPool.Init(cOrigin);
		m_bTimeSlow = false;
		m_fRadius = GetComponent<CircleCollider2D>().radius;
		m_MoveTime = new WaitForFixedUpdate();
		Set_IsActive(false);
		SetDefaultData();
		SetDefaultPos();
		//SetFrameCheck(false);
		m_attackUnitTime = new WaitForSeconds(0.05f);
		m_arContactPoint2D = new ContactPoint2D[5];
		m_arSpriteAttack = C_SpriteMgr.Instance.GetBall_Attack();
		m_nBlockCreateCount = 1;
	}
	public void EndGame()
	{
		m_rb.simulated = false;
		Set_IsActive(false);
		SetDefaultPos();
		MoveStop();
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	public void SetDir(Vector2 vec2Dir)
	{
		m_vec2Dir = vec2Dir;
	}

	public Vector2 GetDir()
	{
		return m_vec2Dir;
	}

	public void Set_IsActive(bool isActive)
	{
		m_isActive = isActive;
	}
	public bool IsActive()
	{
		return m_isActive;
	}
	//public void SetFrameCheck(bool check)
	//{
	//	m_bFrameCollisionCheck = check;
	//}
	//public bool GetFrameCheck()
	//{
	//	return m_bFrameCollisionCheck;
	//}

	public void SetDefaultData()
	{
		SetHpDefault();
		SetPowerDefault();
		SetSpeedDefault();
		SetInvincibilityTimeDefault();
		m_rb.simulated = false;
		SetInvincibility(false);
	}
	public void SetDefaultPos()
	{
		m_rb.MovePosition(Vector2.zero);
		transform.position = Vector2.zero;
	}

	public void GetDMG(int nDmg)
	{
		if (!m_isInvincibility)
		{
			Red.SetActive(true);
			if (m_nHp - nDmg < 0)
			{
				m_nHp = 0;
				m_cGameUI.SetBallHp(m_nHp);
				GameEndText.text = "게임 오버...";
				GameEndText.gameObject.SetActive(true);
				C_GameMgr.Instance.GameEnd();
			}
			else
			{
				m_nHp -= nDmg;
				m_cGameUI.SetBallHp(m_nHp);

				//StartCoroutine(coroutineInvincibility(m_fInvincibilityTime));
			}
		}
	}
	public void GetHeal(int nHeal)
	{
		if (m_nHp + nHeal > m_nMaxHp)
		{
			m_nHp = m_nMaxHp;
		}
		else
		{
			m_nHp += nHeal;
		}
		m_cGameUI.SetBallHp(m_nHp);
	}

	IEnumerator coroutineInvincibility(float fTime)
	{
		WaitForSeconds waitTime = new WaitForSeconds(fTime);

		SetInvincibility(true);
		m_sr.color = Color.grey;

		yield return waitTime;

		m_sr.color = Color.white;
		SetInvincibility(false);
	}

	public void SetSprite(Sprite sprite)
	{
		m_sr.sprite = sprite;
	}
	public void SetSpriteFlipX(bool flip)
	{
		m_sr.flipX = flip;
	}
	public void Rotate(Quaternion quaternion)
	{
		m_tfRender.rotation = quaternion;
	}
	public void MoveStart()
	{
		m_rb.velocity = m_vec2Dir * m_fSpeed;
	}
	public void MoveStop()
	{
		m_rb.velocity = Vector2.zero;
	}
	public Rigidbody2D GetRb()
	{
		return m_rb;
	}
	private void SetHpDefault()
	{
		m_nMaxHp = C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nDefaultBallMaxHp;
		m_nHp = m_nMaxHp;
		m_cGameUI.SetBallHp(m_nHp);
	}
	private void SetPowerDefault()
	{
		m_nPower = C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nDefaultBallPower;
	}
	public void SetSpeedDefault()
	{
		m_fSpeed = C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).fDefaultBallSpeed;
	}
	private void SetInvincibilityTimeDefault()
	{
		m_fInvincibilityTime = C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).fDefaultInvincibilityTime;
	}

	public void SetInvincibility(bool invincibility)
	{
		m_isInvincibility = invincibility;
	}

	public void SetMaxHp(int nMaxHp)
	{
		if (nMaxHp <= 0)
		{
			return;
		}

		m_nMaxHp = nMaxHp;
		m_cGameUI.SetBallMaxHp(m_nMaxHp);
		if(m_nHp > m_nMaxHp)
		{
			m_nHp = m_nMaxHp;
			m_cGameUI.SetBallHp(m_nHp);
		}
	}
	public void SetPower(int nPower)
	{
		if(nPower <= 0)
		{
			return;
		}
		m_nPower = nPower;
	}
	public void SetSpeed(float fSpeed)
	{
		if (fSpeed <= 0.0f)
		{
			return;
		}
		m_fSpeed = fSpeed;
	}
	public void SetInvincibilityTime(float fTime)
	{
		if(fTime <= 0.0f)
		{
			return;
		}
		m_fInvincibilityTime = fTime;
	}

	public int GetMaxHp()
	{
		return m_nMaxHp;
	}
	public int GetHp()
	{
		return m_nHp;
	}
	public int GetPower()
	{
		return m_nPower;
	}
	public float GetSpeed()
	{
		return m_fSpeed;
	}
	public float GetInvincibilityTime()
	{
		return m_fInvincibilityTime;
	}

	public float GetRadius()
	{
		return m_fRadius;
	}

	public void OnBurst()
	{
		m_bIsBurst = true;
		m_cBurstImg.SetActive(true);
		SetPower(GetPower() * 3);
		SetSpeed(GetSpeed() * 2);
	}
	public void OffBurst()
	{
		m_bIsBurst = false;
		m_cBurstImg.SetActive(false);
		SetPowerDefault();
		SetSpeedDefault();
	}
	public void OnColFrame(E_Dir eColDir)
	{
		if (IsActive())
		{
			if (!m_bIsBurst)
			{
				m_nBurstCnt--;
				if (m_nBurstCnt > 0)
				{
					cBurstTextPool.GetFromPool().SetTextAndActive
						(Vector2.zero, m_nBurstCnt.ToString());
				}
				if (m_nBurstCnt == 0)
				{
					cBurstTextPool.GetFromPool().SetTextAndActive
							   (Vector2.zero, "버스트!!");
					OnBurst();
				}
			}

			Vector2 vec2Dir = GetDir();
			if(eColDir == E_Dir.Left)
			{
				vec2Dir.x *= -1;
				if(vec2Dir.x > 0)
				{
					vec2Dir.x *= -1;
				}
			}
			if (eColDir == E_Dir.Right)
			{
				vec2Dir.x *= -1;
				if (vec2Dir.x <= 0)
				{
					vec2Dir.x *= -1;
				}
			}
			if (eColDir == E_Dir.Up)
			{
				vec2Dir.y *= -1;
				if (vec2Dir.y <= 0)
				{
					vec2Dir.y *= -1;
				}
			}
			if (eColDir == E_Dir.Down)
			{
				vec2Dir.y *= -1;
				if (vec2Dir.y > 0)
				{
					vec2Dir.y *= -1;
				}
			}
			Move(vec2Dir);
		}
	}
	public void OnColPlatform(Vector2 vec2Normal)
	{
		if(IsActive())
		{
			m_nBurstCnt = m_nBurstMaxCnt;
			if (m_bIsBurst)
			{
				OffBurst();
			}

			m_nBlockCreateCount--;
			if (m_nBlockCreateCount <= 0)
			{
				m_cBlockMgr.CreateBlockLine();
				m_nBlockCreateCount = C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).nBlockICreateCount;
			}
			m_cGameUI.SetBlockCreateCount(m_nBlockCreateCount.ToString());

			MoveReflect(vec2Normal);
		}
	}
	public void OnColBlock(C_Block block, Vector2 vec2)
	{
		if (IsActive())
		{
			if (!m_bIsBurst)
			{
				m_nBurstCnt--;
				if (m_nBurstCnt > 0)
				{
					cBurstTextPool.GetFromPool().SetTextAndActive
						(Vector2.zero, m_nBurstCnt.ToString());
				}
				if (m_nBurstCnt == 0)
				{
					cBurstTextPool.GetFromPool().SetTextAndActive
							   (Vector2.zero, "버스트!!");
					OnBurst();
				}
			}
			Stop();
			StartCoroutine(coroutineAttackAndMove(block, vec2));
		}
	}
	
	
	private void Stop()
	{
		MoveStop();
	}
	private void MoveReflect(Vector2 vec2Normal)
	{
		//collision.GetContacts(m_arContactPoint2D);
		//m_vec2Normal = m_arContactPoint2D[0].normal;
		m_vec2Reflect = Vector2.Reflect(GetDir(), vec2Normal);
		//m_vec2Reflect = m_vec2Reflect.normalized;

		Move(m_vec2Reflect);
	}
	private void Move(Vector2 vec2Dir)
	{
		SetDir(vec2Dir);
		float fAngle = Mathf.Atan2(GetDir().y, GetDir().x) * Mathf.Rad2Deg;
		Rotate(Quaternion.AngleAxis(fAngle - 90.0f, Vector3.forward));
		if (GetDir().x >= 0)
		{
			SetSpriteFlipX(true);
		}
		else
		{
			SetSpriteFlipX(false);
		}
		MoveStart();

		if (IsWillAttack(GetDir()))
		{
			SetSprite(C_SpriteMgr.Instance.GetBall(E_Sprite_Ball.Move_Attack));
		}
		else
		{
			SetSprite(C_SpriteMgr.Instance.GetBall(E_Sprite_Ball.Move));
		}
	}
	private bool IsWillAttack(Vector2 vec2Direction)
	{
		m_hit = Physics2D.Raycast(m_tfRayPos.position, vec2Direction, C_BaseDataMgr.Instance.GetRayMaxDistance());

		//Debug.DrawRay(m_tfRayPos.position, vec2Direction * C_BaseDataMgr.Instance.GetRayMaxDistance(), Color.red, 0.5f);
		//Debug.Log(m_hit.collider.gameObject.tag + m_hit.distance);
		if (m_hit.collider == null)
		{
			Debug.Log("Ray_Null");
			return false;
		}
		if (m_hit.collider.gameObject.tag == "Block")
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	IEnumerator coroutineAttackAndMove(C_Block block, Vector2 vec2)
	{
		block.GetComponent<Collider2D>().GetContacts(m_arContactPoint2D);
		m_isAttack = true;

		for (int i = 0; i < m_arSpriteAttack.Length; i++)
		{
			SetSprite(m_arSpriteAttack[i]);

			yield return m_attackUnitTime;
		}
		MoveReflect(vec2);

		//MoveReflect(m_arContactPoint2D[0].normal * (-1.0f));

		//Vector2 vec2Dir = GetDir();
		//if (eColDir == E_Dir.Left || eColDir == E_Dir.Right)
		//{
		//	vec2Dir.x *= -1;
		//}
		//if (eColDir == E_Dir.Up || eColDir == E_Dir.Down)
		//{
		//	vec2Dir.y *= -1;
		//}
		//Move(vec2Dir);

		m_isAttack = false;
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		//Debug.Log(col.gameObject.tag);
		if (col.gameObject.tag == "Platform")
		{
			//Time.timeScale = 1.0f;
			SetSpeedDefault();
			GetRb().velocity = GetDir() * GetSpeed();
			//m_line.ClearLine();
			StopCoroutine(m_coroutineLine);
			m_line.ClearLine();
			Dark.GetComponent<Animator>().SetTrigger("close");
		}
	}

	//private void OnTriggerEnter2D(Collision2D collision)
	//{
	//	Debug.Log("1");
	//	Debug.Log(collision.gameObject.tag);
	//	if (collision.gameObject.tag == "DestroyBlockArea")
	//	{
	//		m_bTimeSlow = true;
	//	}
	//}
	//private void OnTriggerExit2D(Collision2D collision)
	//{
	//	if (collision.gameObject.tag == "DestroyBlockArea")
	//	{
	//		Debug.Log("2");
	//		m_bTimeSlow = false;
	//	}
	//}
	public bool GetIsBurst()
	{
		return m_bIsBurst;
	}
	public void DrawLine()
	{
		m_coroutineLine = StartCoroutine(CoroutineLine());
	}
	IEnumerator CoroutineLine()
	{
		m_line.DrawLine();
		while (true)
		{
			m_line.ClearLine();
			m_line.DrawLine();
			yield return null;
		}
	}
	private void FixedUpdate()
	{
		
		
	}
	private void Update()
	{
		//if (m_bTimeSlow)
		//{
		//	Time.timeScale = 0.3f;
		//}
		//else
		//{
		//	Time.timeScale = 1.0f;
		//}
	}

}

