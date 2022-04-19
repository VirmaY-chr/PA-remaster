using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct Event
{
    public string name;
    public float startTime;
    public float duration;
    public enum Type
    {
        Move,
        Scale,
        Rotate,
        Color,
    }
    public Type type;
    public Ease ease;
    public List<float> values;
    public Color color;
}

public class GObject : MonoBehaviour
{
    public enum Type
    {
        Normal,
        Helper
    }
    public Type type;
    public List<Event> events = new List<Event>() { new Event() { name = "first", type = Event.Type.Move, startTime = 0, duration = 3, values = new List<float>() { 4, 0 }, ease = Ease.OutSine } };
    public Sequence seq;

    public BoxCollider2D bc;
    public SpriteRenderer sr;

    private void Awake()
    {
        TryGetComponent(out sr);
        TryGetComponent(out bc);
        SetType();

    }
    void OnEnable()
    {
        seq = DOTween.Sequence();

        for (int i = 0; i < events.Count; i++)
        {
            switch (events[i].type)
            {
                case Event.Type.Move:
                    seq.Insert(events[i].startTime, transform.DOLocalMove(new Vector2(events[i].values[0], events[i].values[1]), events[i].duration).SetEase(events[i].ease)).SetTarget(gameObject);
                    break;
                case Event.Type.Scale:
                    seq.Insert(events[i].startTime, transform.DOScale(new Vector2(events[i].values[0], events[i].values[1]), events[i].duration).SetEase(events[i].ease)).SetTarget(gameObject);
                    break;
                case Event.Type.Rotate:
                    seq.Insert(events[i].startTime, transform.DORotate(new Vector3(0, 0, events[i].values[0]), events[i].duration).SetEase(events[i].ease)).SetTarget(gameObject);
                    break;
                case Event.Type.Color:
                    seq.Insert(events[i].startTime, sr.DOColor(new Color (events[i].color.r, events[i].color.g, events[i].color.b, 1), events[i].duration).SetEase(events[i].ease)).SetTarget(gameObject);
                    break;
            }
        }
        seq.AppendCallback(() => transform.localScale = Vector2.zero);
    }

    public void SetType()
    {
        switch (type)
        {
            case Type.Normal:
                bc.enabled = true;
                sr.sprite = LevelEngine.inst.typesSpr[0];         
                    break;
            case Type.Helper:
                bc.enabled = false;
                sr.sprite = LevelEngine.inst.typesSpr[1];
                break;
        }
    }
}
