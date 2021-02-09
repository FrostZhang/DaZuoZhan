using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainPanel : MonoBehaviour, IPanel
{
    public SuperTextMesh scoll;
    public Button exit;

    public Transform Tr { get; set; }

    public int value;

    private int to;
    Coroutine cor;
    public void Start()
    {
        exit.onClick.AddListener(() =>
        {
            MessageBox.Ins.ShowMessage(new MesSetting()
            {
                 title="",
                  content ="退出？",
                   btnoktext ="是的",
                    btncanceltext ="取消",
                      btnType = BtnType.Cancel,
                       action = (res) =>{
                           if (res==1)
                           {
                               Application.Quit();
                           }
                       }
            });
        });
    }

    public void OnDestroy()
    {

    }

    public void SetV(int v)
    {
        value = v;
        if (cor != null)
        {
            StopCoroutine(cor);
        }
        cor = StartCoroutine(Tween());
    }

    IEnumerator Tween()
    {
        while (value != to)
        {
            var c = value - to;
            var a = Mathf.Clamp(c / 20, 1, 10);
            to += a;
            if (to > value)
            {
                to = value;
                scoll.text = to.ToString();
                yield break;
            }
            scoll.text = to.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
