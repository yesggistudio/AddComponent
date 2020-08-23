using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Jaeyun.Script.Level;
using DG.Tweening;
using System.Net.Http.Headers;
using UnityTemplateProjects.Jaeyun.Script.Actor;
using UnityTemplateProjects.Jaeyun.Script;

public class BossAttack : MonoBehaviour
{
    private static BossAttack instance;
    public static BossAttack Instance
    {
        get { return instance; }
    }
    public float HandHight;

    public Animator bossani;
    public Animator lefthand;
    public Animator righthand;


    private GameObject target;
    public GameObject lefthandObj;
    public GameObject righthandObj;


    private bool checkbool;
    private bool bossattackbool;

    public Image RedImg;
    private int bossDmg;

    MakeButton btnboom;
    public GameObject BombOBJ;

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {

        target = GameObject.Find("CharacterPlatformer");

        StartCoroutine(bossCheck());

        RedImg.fillAmount = (float)(5) / (float)(5);
    }

    IEnumerator bossCheck()
    {
        yield return new WaitForSeconds(15f);
        if (!checkbool)
        {
            bossTalkFx();
        }
    }

    
    public void bossTalkFx()
    {
        checkbool = true;

        StartCoroutine(AttackStartCoroutine());

    }

    IEnumerator AttackStartCoroutine()
    {
        SoundManager.Instance.SoundFx(4);
        bossani.Play("bossTalk");

        //사운드 재생.

        yield return new WaitForSeconds(4f);

        StartCoroutine(MakeCompBtn());
        StartCoroutine(MakeBomb());
        bossani.Play("bossClosingVisor");
        lefthandObj.SetActive(true);
        righthandObj.SetActive(true);
        lefthand.Play("HandLeftOpen");
        righthand.Play("HandRightOpen");

        yield return new WaitForSeconds(7f);

        BossAttackOne();
    }

    public void BossAttackOne()
    {

        int t = Random.Range(0, 2);
        switch (t)
        {
            case 0:
                lefthand.Play("HandOneAttack0");
                StartCoroutine(LeftOneCoroutine());
                break;
            case 1:
                righthand.Play("HandTwoAttack0");
                StartCoroutine(RightOneCoroutine());
                break;
            default:
                break;
        }

    }

    IEnumerator MakeCompBtn()
    {

        btnboom = GameObject.FindObjectOfType<MakeButton>();
        while (bossDmg < 5)
        {
            btnboom.StartMake();
            yield return new WaitForSeconds(6f);

        }



    }

    IEnumerator MakeBomb()
    {

        while (bossDmg < 5)
        {
            GameObject ddd = Instantiate(BombOBJ);

            ddd.transform.position = new Vector3(3f, 21f, 0f);
            yield return new WaitForSeconds(7f);
        }
    }



    /*
    int q = Random.Range(0, 2);
    int t = Random.Range(0, 2);

    switch (q)
    {
        case 0:

            if (t == 0)
            {

                lefthand.Play("HandOneAttack0");


            }
            else
            {


            }
            break;
        case 1:
            if (t == 0)
            {

            }
            else
            {


            }
            break;
        default:
            break;
    }
    */


    IEnumerator LeftOneCoroutine()
    {
        yield return new WaitForSeconds(6f);

        righthand.Play("HandTwoAttack0");

        yield return new WaitForSeconds(6f);

        lefthand.Play("HandLeftIdle");
        righthand.Play("HandRightIdle");

        yield return new WaitForSeconds(2f);

        BossAttackOne();

    }



    IEnumerator RightOneCoroutine()
    {
        yield return new WaitForSeconds(6f);

        lefthand.Play("HandOneAttack0");

        yield return new WaitForSeconds(6f);

        lefthand.Play("HandLeftIdle");
        righthand.Play("HandRightIdle");

        yield return new WaitForSeconds(2f);

    }


    public void BossHpMinus()
    {
        bossDmg++;
        Debug.Log(bossDmg);
        RedImg.fillAmount = (float)(5 - bossDmg) / (float)(5);


        if (bossDmg == 0)
        {
            // 근데 이전에 보스 죽는 모션 .

            bossani.Play("bossDead");

            var levelManager = FindObjectOfType<LevelManager>();
            levelManager.GameOver();
        }
    
    }




}
