using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{

    public GameObject setenemy, message;
    public ShootingController shootingController;
    public Animator playeranimator;
    public int enemynumber, scene;
    public Vector3[] setpos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartSetting");
    }

    // Update is called once per frame
    void Update()
    {

    }
    //敵がダウンした時呼び出す関数
    public void EnemyDown()
    {
        enemynumber--;

        if (enemynumber < 1)
        {
            scene++;

            if (scene == 3)
            {
                Debug.Log("クリア");
                Text text = message.GetComponent<Text>();
                Animator messageanimator = message.GetComponent<Animator>();
                text.text = "Clear";
                messageanimator.SetTrigger("Clear");
                return;
            }

            StartCoroutine("MessageChange");
        }
    }

    //敵の配置
    IEnumerator EnemySet()
    {

        for (; enemynumber < 1; enemynumber++)
        {
            Instantiate(setenemy, setpos[enemynumber+scene], Quaternion.identity);
            yield return 5f;
        }
    }

    //2回目以降の移動演出
    IEnumerator MessageChange()
    {
        //スタンバイにの変更、テキストのアニメーション変更、アニメーションが完了されるまで待機、ショットに変更、アクションをショットに変更、敵の配置
        Text text = message.GetComponent<Text>();
        Animator messageanimator = message.GetComponent<Animator>();
        playeranimator.SetTrigger("Move");
        text.text = "Standby!";
        messageanimator.SetTrigger("Standby");
        //何故か3秒待たないとシュートに書き換わってしまう
        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => playeranimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        text.text = "shot!";
        messageanimator.SetTrigger("Shot");
        shootingController.action = Action.shot;
        yield return StartCoroutine("EnemySet");
    }
    //初回の移動演出
    IEnumerator StartSetting()
    {
        Text text = message.GetComponent<Text>();
        Animator messageanimator = message.GetComponent<Animator>();
        text.text = "Standby!";
        yield return new WaitUntil(() => playeranimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        text.text = "shot!";
        messageanimator.SetTrigger("Shot");
        shootingController.action = Action.shot;
        yield return StartCoroutine("EnemySet");
    }
}