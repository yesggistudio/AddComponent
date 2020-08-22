using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{

    public Animator bossani;
    public Animator lefthand;
    public Animator righthand;


    private GameObject target;
    public GameObject handObj;

    private bool checkbool;

    private void Start()
    {
        
        target = GameObject.Find("CharacterPlatformer");
        StartCoroutine(bossCheck());

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

        bossani.Play("bossTalk");

        //사운드 재생.

        yield return new WaitForSeconds(4f);


        bossani.Play("bossClosingVisor");
        handObj.SetActive(true);
        yield return new WaitForSeconds(6f);



        BossAttackOne();



    }
    


    public void BossAttackOne()
    {
         
        

    }


    IEnumerator OneCoroutine()
    {


        //ani stop
        //5.5까지 이동
        //플레이어 탐색
        //내려찍기.
        //다시 idle 모드 돌입. 
        yield return new WaitForSeconds(3f);



    }


    public void BossAttackTwo()
    {


    }

    IEnumerator TwoCoroutine()
    {

        yield return new WaitForSeconds(1f);

    }


    public void BossAttackThree()
    {



    }








}
