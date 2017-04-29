using UnityEngine;
using System.Collections;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Mission;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Utils;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Mission
{
    public class PlotMissionView : PlotDialogueUIDetail
    {
        private bool bInited = false;
        private KPlotInfo m_plotInfo;
        private SceneCamera camera;

        private static PlotMissionView instance;
        public static PlotMissionView GetInstance()
        {
            if (instance == null)
                instance = new PlotMissionView();
            return instance;
        }

        public PlotMissionView()
            : base(0, 0)
        {
        }
            
        protected override void PreInit()
        {
            base.PreInit();
            bInited = true;

            // 放与UI不同一层
            viewGo.transform.parent = LayerManager.GetInstance().EffectLayer.transform;
            Reset();
        }

        protected override void Init()
        {
            UIEventListener.Get(FindGameObject("PlotBackground")).onClick += OnClickBtnHandler;
            UIEventListener.Get(SkipButton.gameObject).onClick += OnSkipBtnHandler;
            camera = SceneView.GetInstance().GetSceneCamera();

            RenderInfo();
        }

        public void Open(int plotID)
        {
            Reset();

            KPlotInfo plotInfo = KConfigFileManager.GetInstance().GetPlotInfo(plotID);

            if (plotInfo == null)
            {
                Debug.LogError("PlotMissionView::GetPlotInfo Error plotID = " + plotID.ToString());
            }

            m_plotInfo = plotInfo;
            RenderInfo();
            Show(true);

            bEnter = true;
        }

        public void Close()
        {
            bOut = true;
        }

        private void Reset()
        {
            if (bInited)
            {
                DiologueNpcNameLabel.text = "";
                DialogueComtentLabel.text = "";

                fLnkTopStartY = LnkTop.transform.position.y;
                fLnkBottomStartY = LnkBottom.transform.position.y;
                fLnkTopEndY = fLnkTopStartY - LnkTop.height * LnkTop.transform.localScale.y;
                fLnkBottomEndY = fLnkBottomStartY + LnkBottom.height * LnkBottom.transform.localScale.y;
            }
        }

        private void RenderInfo()
        {
            if (bInited && m_plotInfo != null)
            {
                LayerManager.GetInstance().UILayer.SetActive(false);

                KHeroSetting heroSetting = KConfigFileManager.GetInstance().GetHeroSetting(m_plotInfo.nNpcID);

                if (heroSetting != null)
                {
                    DiologueNpcNameLabel.text = heroSetting.Name;
                }

                fNappTime = 0;
                DialogueComtentLabel.text = m_plotInfo.Content;

                if (camera != null)
                {
                    if (m_plotInfo.strCameraPosition != null && m_plotInfo.strCameraPosition != "0")
                    {
                        string[] posTemp = m_plotInfo.strCameraPosition.Split(':');
                        if (posTemp.Length == 3 && camera != null)
                        {
                            Vector3 vecPosition = MapUtils.GetMetreFromInt(int.Parse(posTemp[0]), int.Parse(posTemp[2]), int.Parse(posTemp[1]));
                            camera.PlayPlot(m_plotInfo.plotType, vecPosition, m_plotInfo.fCameraDis);
                        }
                   }
                }
            }
        }

        private void OnClickBtnHandler(GameObject go)
        {
            if (m_plotInfo != null)
            {
                if (m_plotInfo.nextID != 0)
                {
                    KPlotInfo plotInfo = KConfigFileManager.GetInstance().GetPlotInfo(m_plotInfo.nextID);

                    if (plotInfo != null)
                    {
                        m_plotInfo = plotInfo;
                        RenderInfo();
                    }
                    else
                    {
                        OnSkipBtnHandler(go);
                    }
                }
                else
                {
                    OnSkipBtnHandler(go);
                }
            }
        }

        private void OnSkipBtnHandler(GameObject go)
        {
            if (camera != null)
            {
                camera.StopPlot();
            }

            Close();
        }

        private float fNappTime = 0;
        private float fMouseTimeUse = 0;
        private float fLnkTimeUse = 0;

        private float fLnkBottomStartY;
        private float fLnkTopStartY;
        private float fLnkBottomEndY;
        private float fLnkTopEndY;

        private bool bEnter = false;
        private bool bOut = false;

        public override void FixedUpdate()
        {
            if (!bInited || !isOpen())
            {
                return;
            }

            fNappTime += Time.deltaTime;

            if (fNappTime > 5.0f)
            {
                OnClickBtnHandler(null);
                fNappTime = 0;
            }

            if (bEnter)
            {
                fLnkTimeUse += Time.deltaTime;

                float bottomY = Mathf.Lerp(fLnkBottomStartY, fLnkBottomEndY, fLnkTimeUse / camera.fPlayTime);
                LnkBottom.transform.position = new Vector3(LnkBottom.transform.position.x, bottomY, LnkBottom.transform.position.z);

                float TopY = Mathf.Lerp(fLnkTopStartY, fLnkTopEndY, fLnkTimeUse / camera.fPlayTime);
                LnkTop.transform.position = new Vector3(LnkTop.transform.position.x, TopY, LnkTop.transform.position.z);
            }
            else if (bOut)
            {
                fLnkTimeUse += Time.deltaTime;

                float bottomY = Mathf.Lerp(fLnkBottomEndY, fLnkBottomStartY, fLnkTimeUse / camera.fPlayTime);
                LnkBottom.transform.position = new Vector3(LnkBottom.transform.position.x, bottomY, LnkBottom.transform.position.z);

                float TopY = Mathf.Lerp(fLnkTopEndY, fLnkTopStartY, fLnkTimeUse / camera.fPlayTime);
                LnkTop.transform.position = new Vector3(LnkTop.transform.position.x, TopY, LnkTop.transform.position.z);
            }

            if (fLnkTimeUse > camera.fPlayTime)
            {
                if (bOut)
                {
                    Reset();
                    Hide();
                    LayerManager.GetInstance().UILayer.SetActive(true);
                }

                bEnter = false;
                bOut = false;
                fLnkTimeUse = 0;
            }

            fMouseTimeUse += Time.deltaTime;
            if (fMouseTimeUse > 0.3f)
            {
                MouseClickedSprite.gameObject.SetActive(!MouseClickedSprite.gameObject.activeSelf);
                fMouseTimeUse = 0;
            }
        }
    }
}
