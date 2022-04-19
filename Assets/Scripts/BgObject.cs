using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class BgObject : MonoBehaviour
{
    public Color color;

    MeshRenderer mr;
    AudioSource source;

    float updateStep = 0.01f;
    int sampleDataHeight = 1024;
    float currentUpdateTime = 0f;
    float clipLoudness;
    float[] clipsampleData;

    public float sizeFactor = 15;

    float minSize = 0;
    float maxSize = 500;


    private void Awake()
    {
        mr = transform.GetChild(0).GetComponent<MeshRenderer>();
        source = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        mr.material.color = color;
        clipsampleData = new float[sampleDataHeight];
    }


    private void FixedUpdate()
    {
        currentUpdateTime += Time.fixedDeltaTime;
        if (sizeFactor > 0)
        {
            if (currentUpdateTime >= updateStep)
            {
                currentUpdateTime = 0f;
                source.clip.GetData(clipsampleData, source.timeSamples);
                clipLoudness = 0f;
                foreach (var sample in clipsampleData)
                    clipLoudness += Mathf.Abs(sample);
                clipLoudness /= sampleDataHeight;

                clipLoudness *= sizeFactor;
                clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);

                mr.transform.localScale = new Vector3(clipLoudness, clipLoudness, 0) + Vector3.one;
            }
        }
    }
}
