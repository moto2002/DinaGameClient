using UnityEngine;
using System.Collections;

public class EnemyBlood : MonoBehaviour
{

    float from = 1f;
    float to = 0f;
    UISprite BooldBackgroud = null;
    UISprite BooldForward = null;
    UISprite BooldWhite = null;
    UISprite BooldForwardWhite = null;
    UISprite LightSprite = null;
    void Start()
    {
        BooldBackgroud = FindUIObject<UISprite>(gameObject, "BooldBackgroud");
        BooldForward = FindUIObject<UISprite>(gameObject, "BooldForward");
        BooldWhite = FindUIObject<UISprite>(gameObject, "BooldWhite");
        BooldForwardWhite = FindUIObject<UISprite>(gameObject, "BooldForwardWhite");
        LightSprite = FindUIObject<UISprite>(gameObject, "LightSprite");
        fromRate = 8f;
        toRate = 7.9f;
    }

    float fromRate = 0f;
    float toRate = 0f;
    int toRateInt = 0;
    int fromRateInt = 0;
    bool isFinished = false;
    UILabel EnemyBloodCountLabel = null;

    public void Set(float toRate, UILabel EnemyBloodCountLabel)
    {
        isFinished = false;
        toRate -= 0.0001f;
        if (toRate < 0)
            toRate = 0;
        this.toRate = toRate;
        this.toRateInt = (int)toRate;
        this.EnemyBloodCountLabel = EnemyBloodCountLabel;
    }

    public void ClearRate(float fromRate, UILabel EnemyBloodCountLabel)
    {
        isFinished = false;
        this.fromRate = fromRate;
        Set(fromRate, EnemyBloodCountLabel);
    }

    void FixedUpdate()
    {
        if (isFinished)
            return;
        fromRate = Mathf.Lerp(fromRate, toRate, Time.deltaTime * 2);
        
        fromRateInt = (int)fromRate;
        from = fromRate - fromRateInt;

        if (fromRateInt == toRateInt)
        {
            BooldForward.spriteName = toRateInt.ToString();
            BooldForward.fillAmount = toRate - toRateInt;
        }
        else
        {
            BooldForward.fillAmount = 0f;
        }
        if (fromRateInt - 1 <= 0)
        {
            BooldBackgroud.fillAmount = 0f;
        }
        else
        {
            BooldBackgroud.fillAmount = 1f;
        }

        BooldBackgroud.spriteName = (fromRateInt - 1).ToString();
        BooldForwardWhite.spriteName = fromRateInt.ToString();
        EnemyBloodCountLabel.text = "X" + fromRateInt.ToString();
        
        BooldWhite.fillAmount = from;
        BooldForwardWhite.fillAmount = from;

        LightSprite.transform.localPosition = new Vector3(BooldWhite.width * BooldWhite.fillAmount, 0, 0);

        if (fromRate <= 0.001f)
        {
            this.gameObject.SetActive(false);
        }

        if (Mathf.Abs(fromRate - toRate) < 0.001f)
        {
            isFinished = true;
        }
        else
        {
            isFinished = false;
        }
    }

    public T FindUIObject<T>(GameObject parent, string name) where T : Component
    {
        T[] coms = parent.GetComponentsInChildren<T>();
        int count = coms.Length;
        for (int i = 0; i < count; i++)
        {
            if (coms[i].gameObject.name.Equals(name))
            {
                return coms[i];
            }
        }
        return null;
    }
}
