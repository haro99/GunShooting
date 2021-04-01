using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public GameObject player, Bullet;
    public GameObject gameManger;
    public int HitTime;
    public float count;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameManger = GameObject.Find("GameManger");
        HitTime = 10;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);

        if (player.GetComponent<ShootingController>().GetAction == Action.standby)
        {
            return;
        }
        count += Time.deltaTime;
    }

    //当たった時の処理
    public void HitDown()
    {
        transform.LookAt(null);
        animator.SetTrigger("Hit");
        Destroy(this.gameObject, 3f);
        gameManger.GetComponent<GameManger>().EnemyDown();
    }

    //プレイヤーにダメージを与える処理
    public void DamageShot()
    {
        Instantiate(Bullet, transform.position + new Vector3(0f,1f, 0f), transform.rotation * Quaternion.Euler(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f));
        if (count > HitTime)
        {
            Debug.Log("ダメージ");
            player.GetComponent<ShootingController>().Damage();
            count = 0f;
        }
    }
}
