using System;
using UnityEngine;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Manager;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Logic.RemoteCall;
using Assets.Scripts.Define;

/************************************************************************/
/* 填充数据data>>KGiftData
 * 绑定listeners
 * 初始化UIComponentReferences
 * author@wuheyang
 * /
/************************************************************************/
public class GiftItem : MonoBehaviour
{
    public KGiftData GiftData = null;
    private bool isCounting = false;
    private bool resize = false;
    public static GiftItem curCountingGiftItem = null;
    private MonoBehaviour curActiveMono = null;

    public UILabel UnGainLabel = null;
    public UILabel GainedBtnLabel = null;
    public UILabel UnReachLabel = null;
    public UILabel CountTimeLabel = null;
    public UILabel TimeLabel = null;
    public UISprite UnGainBackground = null;
    public UISprite GainedBtnBackground = null;
    public UISprite UnReachBackground = null;
    public UISprite CountTimeBackground = null;
    public UISprite GoodsItemSprite = null;
    public UIButton UnGainBtn = null;
    public UIButton GainedBtn = null;
    public UIButton UnReachBtn = null;
    public UIButton CountTimeBtn = null;
    public UIDragPanelContents RewardListItemUI = null;
    public UIGrid GoodsList = null;

    public void Init(KGiftData giftData)
    {
        InitUIComponentReferences();
        this.GiftData = giftData;
        UpdateUIOnDataChanged();
        InitUIListener();
    }

    private void InitUIComponentReferences()
    {
        UnGainLabel = FindUIObject<UILabel>("UnGainLabel");
        GainedBtnLabel = FindUIObject<UILabel>("GainedBtnLabel");
        UnReachLabel = FindUIObject<UILabel>("UnReachLabel");
        CountTimeLabel = FindUIObject<UILabel>("CountTimeLabel");
        TimeLabel = FindUIObject<UILabel>("TimeLabel");
        UnGainBackground = FindUIObject<UISprite>("UnGainBackground");
        GainedBtnBackground = FindUIObject<UISprite>("GainedBtnBackground");
        UnReachBackground = FindUIObject<UISprite>("UnReachBackground");
        CountTimeBackground = FindUIObject<UISprite>("CountTimeBackground");
        GoodsItemSprite = FindUIObject<UISprite>("GoodsItemSprite");
        UnGainBtn = FindUIObject<UIButton>("UnGainBtn");
        GainedBtn = FindUIObject<UIButton>("GainedBtn");
        UnReachBtn = FindUIObject<UIButton>("UnReachBtn");
        CountTimeBtn = FindUIObject<UIButton>("CountTimeBtn");
        RewardListItemUI = FindUIObject<UIDragPanelContents>("RewardListItemUI");
        GoodsList = FindUIObject<UIGrid>("GoodsList");

        Disable(UnReachBtn);
        Disable(UnReachBtn);
        Disable(CountTimeBtn);
        Disable(GainedBtn);
        ActiveRedio(UnGainBtn);
    }

    public void UpdateUIOnDataChanged()
    {
        MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
        if (player.rewardData == null || player.rewardData[GiftData.nID] == null)
            return;

        bool isReward = player.rewardData[GiftData.nID];
        //Debug.Log(GiftData.nID + " " + isReward);
        if (isReward)
        {
            ActiveRedio(GainedBtn);
        }
        else
        {
            if (GiftData.eType == KGiftType.gtOnlineTime)
            {
                int timeCount = GiftData.nOnlineTime * 60 - (int)player.onlineTime;

                if (timeCount <= 0)
                {
                    ActiveRedio(UnGainBtn);
                }
                else
                {
                    if (curCountingGiftItem == null || curCountingGiftItem == this)
                    {
                        curCountingGiftItem = this;
                        ActiveRedio(CountTimeBtn);
                    }
                    else
                    {
                        ActiveRedio(UnReachBtn);
                    }
                    isCounting = true;
                }

            }
            else if (GiftData.eType == KGiftType.gtLevel)
            {
                if (player.levelCurrent < GiftData.nLevelLimit)
                {
                    ActiveRedio(UnReachBtn);
                }
                else
                {
                    ActiveRedio(UnGainBtn);
                }
            }
            else if (GiftData.eType == KGiftType.gtCombat)
            {
                if (player.combat < GiftData.nCombatLimit)
                {
                    ActiveRedio(UnReachBtn);
                }
                else
                {
                    ActiveRedio(UnGainBtn);
                }
            }
            else
            {
                Disable(curActiveMono);
            }
        }

        TimeLabel.text = GiftData.Name;
        resize = true;

    }


    void FixedUpdate()
    {
        if (curCountingGiftItem == null && this.isCounting)
        {
            curCountingGiftItem = this;
            ActiveRedio(CountTimeBtn);
        }

        if (curCountingGiftItem == this && this.isCounting)
        {
            MajorPlayer player = PlayerManager.GetInstance().MajorPlayer;
            int timeCount = GiftData.nOnlineTime * 60 - (int)player.onlineTime;
            if (timeCount > 0)
            {
                CountTimeLabel.text = FormatTime(timeCount);
            }
            else
            {
                this.isCounting = false;
                curCountingGiftItem = null;
                ActiveRedio(UnGainBtn);
            }
        }

        if (resize)
        {
            //float x = TimeLabel.width + TimeLabel.transform.localPosition.x + (GoodsList.cellWidth * GoodsList.transform.childCount / 2) + 10;
            //Vector3 v = GoodsList.transform.localPosition;
            //GoodsList.transform.localPosition = new Vector3(x, v.y, v.z);
            resize = false;
        }

    }

    private void InitUIListener()
    {
        UIEventListener.Get(UnGainBtn.gameObject).onClick += OnGetGiftBag;
    }


    public void OnGetGiftBag(GameObject go)
    {
        //go == UnGainBtn not go != RewardListItem
        RemoteCallLogic.GetInstance().CallGS("OnGetGiftBag", GiftData.nID);
        EventDispatcher.GameWorld.Regist(ControllerCommand.GIFT_ITEM_UPDATE, UpdateGiftItem);
    }

    public object UpdateGiftItem(params object[] objs)
    {
        if (this == curCountingGiftItem)
            curCountingGiftItem = null;
        UpdateUIOnDataChanged();
        EventDispatcher.GameWorld.Remove(ControllerCommand.GIFT_ITEM_UPDATE, UpdateGiftItem);
        return null;
    }

    private T FindUIObject<T>(string name) where T : Component
    {
        T[] coms = this.GetComponentsInChildren<T>(true);
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

    private void ActiveRedio(MonoBehaviour mo)
    {
        if (curActiveMono != null)
        {
            Disable(curActiveMono);
        }
        curActiveMono = mo;
        Active(mo);
    }

    private void Active(MonoBehaviour mo)
    {
        if (mo != null)
        {
            mo.gameObject.SetActive(true);
        }
    }

    private void Disable(MonoBehaviour mo)
    {
        if (mo != null)
        {
            mo.gameObject.SetActive(false);
        }
    }

    public static string FormatTime(int second)
    {
        int hh = 60 * 60;
        int mm = 60;
        int h = (int)second / hh;
        int m = (int)(second - h * hh) / mm;
        int s = (int)(second - h * hh - m * mm);

        String sh = h < 10 ? "0" + h : h + "";
        String sm = m < 10 ? "0" + m : m + "";
        String ss = s < 10 ? "0" + s : s + "";

        if (h > 0)
        {
            return sh + ":" + sm + ":" + ss;
        }
        else
        {
            return sm + ":" + ss;
        }
    }
}

