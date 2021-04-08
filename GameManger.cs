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
    public Vector3[,] setpos = {
        {new Vector3(-4f, 0f, 5f), new Vector3(-3f, 0f, 15f)},
        {new Vector3(8f, 0f, 13f), new Vector3(9f, 3f, 11f) },
        {new Vector3(-5f,0f,13f), new Vector3(-3f, 0f, 14f) },
        {new Vector3(12.5f, 0f, 30f), new Vector3(22.5f, 0f, 30f) }

    };

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
         //敵残り数が0ならば移動する
        if (enemynumber == 0)
        {
            scene++;

            //シーンが4ならClear
            if (scene == 4)
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

        for (int i = 0; i < 5; i++)
        {
            Instantiate(setenemy, setpos[scene, Random.Range(0, 2)], Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }

    //2回目以降の移動演出
    IEnumerator MessageChange()
    {
        //プレイヤーの移動、メッセージをスタンバイ、テキストのアニメーション変更、プレイヤーの移動アニメーションがが完了されるまで待機、ショットに変更、アクションをショットに変更、敵の配置
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
        enemynumber = 5;
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
        enemynumber = 5;
        yield return StartCoroutine("EnemySet");
    }
}