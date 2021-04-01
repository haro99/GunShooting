using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Action {
    standby = 0,
    shot = 1,
}

public class ShootingController : MonoBehaviour
{
    RectTransform canvasRect;
    public GameObject site;
    public Camera camera;
    public AudioSource audioSource;
    public float hp;
    public Slider slider;
    public Action action;
    public Animator Dagamepanel, playeranimtor;

    public Action GetAction {
        get { return action; }
        set { action = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        playeranimtor = GetComponent<Animator>();
        hp = 100;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        //マウスポイントから変換
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //デバッグ用のドローレイ
        Debug.DrawRay(transform.position, ray.direction * 100, Color.red);

        var mousePos = Input.mousePosition;
        var magnification = canvasRect.sizeDelta.x / Screen.width;
        mousePos.x = mousePos.x * magnification - canvasRect.sizeDelta.x / 2;
        mousePos.y = mousePos.y * magnification - canvasRect.sizeDelta.y / 2;
        mousePos.z = transform.localPosition.z;

        site.transform.localPosition = mousePos;

        //アクションの状態
        if (Action.shot == action)
        {
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
            {
                audioSource.Play();
                Transform objectHit = hit.transform;

                // レイに当たったオブジェクトに何かをする
                Debug.Log(objectHit.name);
                if (objectHit.tag == "Enemy")
                    objectHit.GetComponent<EnemyController>().HitDown();
            }
        }
    }

    public void Damage()
    {
        Dagamepanel.SetTrigger("Dagame");
        hp -= 30;
        if (hp < 0)
            hp = 0;
        slider.value = (float)(hp / 100f);

    }
}
