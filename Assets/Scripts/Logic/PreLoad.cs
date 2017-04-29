using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using UnityEngine;
using Assets.Scripts.Data;
using Assets.Scripts.Model.Player;

namespace Assets.Scripts.Logic
{
    public class PreLoad : MonoBehaviour
    {
        public bool OnEnterScene()
        {
            // int nNextScene = GetNextScene();
            // 以下预加载场景

            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetScenePath(4));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetScenePath(2));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetScenePath(3));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetScenePath(1));

            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2001"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2002"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2003"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2007"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2008"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2009"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2010"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2011"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2013"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2014"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2015"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2016"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2017"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2018"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2020"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2021"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2022"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2023"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2024"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2044"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("2045"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("3001"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetHeroPath("3002"));

            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_chuangsongmen"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_chuanshong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_dianji"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_guangquan_green"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_guangquan_red"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_guangquan_yellow"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_hero_light"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_npc_light"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("player_light"));

            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_bianfu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_guangxian01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_guangxian02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_huodui"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_plant_grass001"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_plant_grass001a"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_pop_fireheap002_huoba"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_pubu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_pubu_shuibo01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_pubu_shuibo02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_pubu_shuihua01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_pubu_shuihua02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_shuimian"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_cave_shuimian_guangdian"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_Hlz_building_bridge"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_Hlz_building_weiwu_02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_crow"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_Hlz_dapubu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_hudie01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_hudie02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_prop_disc001"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_prop_light001"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_prop_stone017"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_yangguang"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_hlz_yun"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_pvp_huodui"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_Pvp_rongyan"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_Pvp_shuimian"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_Pvp_shuimian_anbu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_Pvp_shuiwen"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_shuye01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_shuye02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_shuye03"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_wuziliushui"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_yuanjingpubu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_zhiwutexiao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_changjing_zhongpubu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_chuangsongmen"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_chuanshong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_dianji"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_guangquan_green"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_guangquan_red"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_guangquan_yellow"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_levelup"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_npc_light"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_shubiaodianjitexiao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_badao01_lipihuashantexiao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_badao02_nuzhanbahuang_gongji"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_badao03_xuanfengzhan"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_badao03_xuanfengzhan_mingzhong2"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_badao04_yuanyuewandao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_badao04_yuanyuewandao_mingzhong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_badao04_yuanyuewandao_wuqifeixing"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_boss_H_2013_Qiangdaotouzi_gongji_01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_boss_H_2013_Qiangdaotouzi_gongji_02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_pugong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_pugong_Female"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_pugong_gfemale"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_pugong_gmale"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_pugong_mingzhong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_pugong_nan"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_shunpi"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_shunpi_Female"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_shunpi_gfemale"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_shunpi_gmale"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_shunpi_mingzhong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_shunpi_mingzhong_g1"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_01_shunpi_nan_g"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_02_dafengche"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_02_dafengche_mingzhong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_03_qiaodiban"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_03_qiaodiban_chaofeng"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_03_qiaodiban_g1"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_03_qiaodiban_mingzhong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_04_chongfeng"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_04_chongfeng_mingzhong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_04_chongfeng_xuanyun"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_05_zhenfei"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_05_zhenfei_g1"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_05_zhenfei_jiansu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_06_nuhou_buff"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_daobin_06_nuhou_shifa"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_H2004_ELang_gongji01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_H2004_ELang_mingzhong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_H2004_ELang_shentiyanwutexiao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_H_2003_Zhangfei_jineng"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_h_2008_qiangdao_gongji01"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_h_2008_qiangdao_gongji02"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_h_2009_heiyiren_gongji"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_skill_H_2045_Jingongsishi_pugong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_tanhao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_create_badao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_create_blue"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_create_red"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_create_shitian"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_create_wolong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_create_yellow"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_renwu_xinrenwu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_shuzitanchu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_shuzitanchu_baoji2"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_shuzitanchu_putong"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_ui_renwu_wanchengrenwu"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_wenhao"));
            AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetEffectPath("effect_wenhao_hui"));


            return true;
        }

        private int GetNextScene()
        {
            return 0;
        }

		private bool LoadEffects(string effects, int nLoadLevel)
		{
			if (effects == null)
				return true;
			if (effects.Equals(""))
				return true;

			string[] effectSplits = effects.Split(new char[]{'|'});
			foreach (string effectSplit in effectSplits)
			{
				if (nLoadLevel == AssetLoaderLevel.IMMEDIATELY)
					AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + effectSplit);
				else if (nLoadLevel == AssetLoaderLevel.WAIT_A_MOMENT)
					AssetLoader.GetInstance().PreLoad(URLUtil.GetResourceLibPath() + effectSplit);
				else if (nLoadLevel == AssetLoaderLevel.ONLY_DOWNLOAD)
					AssetLoader.GetInstance().PreLoadOnlyDownload(URLUtil.GetResourceLibPath() + effectSplit);
			}
			return true;
		}

		public bool OnLoadMajorPlayer()
		{
			MajorPlayer majorPlayer = PlayerManager.GetInstance().MajorPlayer;

			Dictionary<string, KSkillDisplay> allSkillDisplayData = KConfigFileManager.GetInstance().skillDisplay.getAllData();
			foreach (KeyValuePair<string, KSkillDisplay> pair in allSkillDisplayData)
			{
				int nLoadLevel = AssetLoaderLevel.WAIT_A_MOMENT;
				if (pair.Value.IsHeroSkill.Equals("FALSE"))
					nLoadLevel = AssetLoaderLevel.ONLY_DOWNLOAD;
				LoadEffects(pair.Value.BeginEffect, nLoadLevel);
				LoadEffects(pair.Value.EndEffect, nLoadLevel);
				LoadEffects(pair.Value.BulletEffect, nLoadLevel);
				LoadEffects(pair.Value.HitEffect, nLoadLevel);
			}
			return true;
		}

        private static PreLoad mInstance;
        public static PreLoad GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = Object.FindObjectOfType(typeof(PreLoad)) as PreLoad;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_PreLoader");
					go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<PreLoad>();
                }
            }
            return mInstance;
        }
    }
}
