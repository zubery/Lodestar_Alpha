using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShieldIndicatorBehaviour : MonoBehaviour
{
    private bool IndicatorActive;
    private SpriteRenderer SpriteRend;
    private Vector3 InitialScale;
    private void Awake()
    {
        InitialScale = transform.localScale;
        SpriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IndicatorActive)
        {
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 100 * Time.deltaTime, 0);
            transform.localScale = InitialScale + Vector3.one * 0.05f * ModulateScale(Time.time);
            //Color _tmp = SpriteRend.color;
            //_tmp.a = ModulateAlpha(Time.deltaTime, 0.1f);
            //SpriteRend.color = _tmp;
        }
    }

    public void Deactivate()
    {
        IndicatorActive = false;
    }
    public void Activate()
    {
        IndicatorActive = true;
    }

    private float ModulateScale(float _time, float _speed = 1.0f)
    {
        //float _t = _speed * (_time - Mathf.FloorToInt(_time));
        float _t = _time;
        return Mathf.Abs(Mathf.Sin(2*_t));
;    }

    private float ModulateAlpha(float _time, float _speed = 1.0f)
    {
        float _t = _speed * (_time - Mathf.FloorToInt(_time));
        return (1-_t)*(1-_t)*(1-_t);
    }

}
