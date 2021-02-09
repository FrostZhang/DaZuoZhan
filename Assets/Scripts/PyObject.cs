using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PyObject : MonoBehaviour
{
    public int objState;
    public Collider2D c;
    public Rigidbody2D r;

    private bool isDes;
    static int max = 8;
    void Awake()
    {
        c = GetComponent<Collider2D>();
        r = GetComponent<Rigidbody2D>();

        transform.DOScale(0.15f, 0.15f).From();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDes)
        {
            return;
        }
        var p = collision.transform.GetComponent<PyObject>();
        if (p)
        {
            if (p.isDes)
            {
                return;
            }
            if (p.objState == objState)
            {
                if (p.objState == max)
                {
                    Ding();
                    return;
                }
                Transform tr = null;
                var cal = p.transform.position.y - transform.position.y;
                if (cal > 0)
                {
                    tr = transform;
                    isDes = true;
                    p.isDes = true;
                    Destroy(p.gameObject);
                    Destroy(gameObject);
                    c.enabled = false;
                    p.c.enabled = false;
                    GameApp.audio.PlayOneShot(AppAudio.AudioPath.eff, CashData.pao);
                    MVC.OnCollision?.Invoke(objState + 1, tr.position);
                    MVC.OnchangeScoll(objState * 4 + 2);
                }
            }
            else
            {
                Ding();
            }
        }
        else
        {
            Ding();
        }
    }

    float interval;
    private void Ding()
    {
        //if (Time.time - interval > 0.5f)
        //{
        //    interval = Time.time;
        //    //GameApp.audio.PlayOneShot(AppAudio.AudioPath.eff, CashData.zj);
        //}
    }
}
