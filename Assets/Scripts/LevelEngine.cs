using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class ObjectData
{
    public string name;
    public float startTime;
    public GObject obj;
}

public class LevelEngine : MonoBehaviour
{
    public Sprite[] typesSpr;
    public Player player;
    public static LevelEngine inst;
    public int curID;
    public List<ObjectData> pool;

    bool gameOver = false;


    private void Awake()
    {
        inst = this;
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (AudioManager.inst.source.isPlaying)
        {
            if(curID < pool.Count)
            {
                if(AudioManager.inst.source.time >= pool[curID].startTime)
                {
                    curID++;
                    pool[curID - 1].obj.gameObject.SetActive(false);
                    pool[curID - 1].obj.gameObject.SetActive(true);
                }
            }
        }

        if (gameOver && AudioManager.inst.source.time > 0.1)
        {
            SeekTo();
            AudioManager.inst.source.time = Mathf.Clamp(AudioManager.inst.source.time, 0.1f, AudioManager.inst.source.clip.length);
        }

    }


    public void SeekTo()
    {
        curID = 0;
        foreach (var i in pool)
        {
            if (i.startTime < AudioManager.inst.source.time)
                curID++;
            else break;
        }
        foreach(var i in pool)
        {
            if (i.startTime > AudioManager.inst.source.time)
                i.obj.gameObject.SetActive(false);
            else i.obj.gameObject.SetActive(true);
            if (i.obj.gameObject.activeInHierarchy)
                i.obj.seq.Goto(AudioManager.inst.source.time - i.startTime);
        }

    }

    public void Playback()
    {
        StartCoroutine(Playback());

        IEnumerator Playback()
        {
            gameOver = true;
            AudioManager.inst.source.pitch = -10;

            yield return new WaitUntil(() => AudioManager.inst.source.time <= 0.2);
            AudioManager.inst.source.pitch = 1;
            gameOver = false;
            UI.inst.ResetHp();
            player.gameObject.SetActive(true);
        }
    }

    public void SetType()
    {

    }
}
