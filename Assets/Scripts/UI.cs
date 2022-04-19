using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Canvas canvas;
    [Header("HP")]
    public int hpCount = 3;
    public int hp;
    public Image hpUnitPrefab;
    public float hpRot;
    public List<Image> hpUnits = new List<Image>();
    

    public static UI inst;

    void Start()
    {
        inst = this;
        //hp
        hp = hpCount;
        for(int i = 0; i < hpCount; i++)
        {
            Image iter = Instantiate(hpUnitPrefab, canvas.transform.GetChild(0), false);
            iter.gameObject.SetActive(true);
            iter.transform.localPosition = new Vector2(30 * i, 0);
            iter.transform.Rotate(new Vector3(0, 0, 45*i));
            hpUnits.Add(iter);
        }
    }

    void FixedUpdate()
    {
        foreach (Image i in hpUnits)
            i.transform.eulerAngles -= new Vector3(0, 0, hpRot);
    }

    public void Hit()
    {
        hp--;
        StartCoroutine(Hit());
        IEnumerator Hit()
        {
            hpUnits[hp].pixelsPerUnitMultiplier = 0.01f;
            yield return new WaitForSeconds(0.08f);
            hpUnits[hp].transform.localScale = Vector2.zero;
        }

        if(hp == 0)
        {
            LevelEngine.inst.Playback();
        }
    }

    public void ResetHp()
    {
        hp = hpCount;
        foreach(var i in hpUnits)
        {
            i.transform.localScale = Vector2.one;
            i.pixelsPerUnitMultiplier = 0.3f;
        }
    }
}
