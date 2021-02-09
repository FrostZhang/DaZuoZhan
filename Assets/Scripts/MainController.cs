using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public Transform start;
    public Transform yindaoxian;

    public PyObject[] prefabs;

    public Transform objpa;

    public int nextState;

    public int currentNum;
    MainPanel mianp;

    public Transform[] limt;
    void Start()
    {
        MVC.OnCollision = GetAPY;
        MVC.OnchangeScoll = OnchangeScoll;
        mianp = GameApp.ui.LoadPanel<MainPanel>(CanvasType.Main);
        Shownext();
    }

    private void OnchangeScoll(int add)
    {
        Debug.Log(mianp.value + add);
        mianp.SetV(mianp.value + add);

    }

    void OnDestroy()
    {
        if (GameApp.Ins)
        {
            GameApp.ui.RemovePanel<MainPanel>();
        }
    }

    void RandomState()
    {
        if (currentNum < 3)
        {
            nextState = 0;
        }
        else
        {
            nextState = UnityEngine.Random.Range(0, 3);
        }
        currentNum++;
        Shownext();
    }

    GameObject next;
    private void Shownext()
    {
        if (next)
        {
            Destroy(next);
        }
        next = Instantiate(prefabs[nextState], start.position, Quaternion.identity, objpa).gameObject;
        var p = next.GetComponent<PyObject>();
        p.c.enabled = false;
        p.r.simulated = false;
        var r = p.GetComponent<SpriteRenderer>();
        r.color = new Color(1, 1, 1, 0.3f);
    }

    // Update is called once per frame
    float interval = 0;
    void Update()
    {
        if (GameApp.ui.IsPointerOverGameObject())
        {
            return;
        }
        foreach (var item in Input.touches)
        {
            if (GameApp.ui.IsPointerOverGameObject(item.fingerId))
            {
                return;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Input.mousePosition;
            pos.z = -GameApp.camera.cameraTr.position.z;
            if (GameApp.camera.ScreenPointToWorld(ref pos))
            {
                var c = CanPlay(pos);
                yindaoxian.gameObject.SetActive(c);
                yindaoxian.position = new Vector3(pos.x, yindaoxian.position.y, pos.z);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            var pos = Input.mousePosition;
            pos.z = -GameApp.camera.cameraTr.position.z;
            if (GameApp.camera.ScreenPointToWorld(ref pos))
            {
                var c = CanPlay(pos);
                yindaoxian.gameObject.SetActive(c);
                yindaoxian.position = new Vector3(pos.x, yindaoxian.position.y, pos.z);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (yindaoxian.gameObject.activeSelf)
            {
                if (interval > 0)
                {
                    GetAPY(nextState, new Vector3(yindaoxian.position.x, start.position.y, start.position.z));
                    RandomState();
                    interval = -1;
                }

            }
            yindaoxian.gameObject.SetActive(false);
        }

        interval += Time.deltaTime;
        if (interval > 1000)
        {
            interval = 0;
        }
    }

    public bool CanPlay(Vector3 pos)
    {
        if (pos.x < limt[0].position.x || pos.y < limt[0].position.y || pos.x > limt[1].position.x || pos.y > limt[1].position.y)
        {
            return false;
        }
        return true;
    }

    public void GetAPY(int state, Vector3 pos)
    {
        if (state >= prefabs.Length)
        {
            return;
        }
        Instantiate(prefabs[state], pos, Quaternion.identity, objpa);
        Checkend(state, pos);
    }

    private void Checkend(int state, Vector3 pos)
    {
        if (objpa.childCount > 15)
        {
            MessageBox.Ins.ShowOk("hello", "游戏结束！分数 " + mianp.value, "好的", (a) =>
            {
                foreach (Transform item in objpa)
                {
                    Destroy(item.gameObject);
                }
                mianp.SetV(0);
                currentNum = 0;
                nextState = 0;
                yindaoxian.gameObject.SetActive(false);
            });
        }
    }
}
