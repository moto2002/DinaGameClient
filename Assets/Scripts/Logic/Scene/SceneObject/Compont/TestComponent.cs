using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Logic.Scene.SceneObject;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Data;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Define;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;

public class TestComponent : BaseComponent {
	
	public static int TestId = 0;
	public static Dictionary<uint,SceneEntity> testEntity = new Dictionary<uint, SceneEntity>();
	Ticker ticker = new Ticker(2500);
	public int skillIndex = 0;
	public override string GetName()
    {
        return GetType().Name;
    }
	
	public static void CreateNewHero(uint curJob)
	{
		if (null == SceneLogic.GetInstance().MainHero)
		{
			return;
		}
		KHeroSetting heroSetting = KConfigFileManager.GetInstance().heroSetting.getData(""+curJob);
		Debug.LogWarning("curJob = "+curJob + " heroSetting = "+heroSetting);
		uint id = (uint)500000+(uint)TestId;
		SceneEntity entity = SceneLogic.GetInstance().CreateSceneObject((uint)curJob, KSceneObjectType.sotHero, heroSetting.HeroType, KDoodadType.dddInvalid, new Vector3(0, -0.7f, -1), id);
		entity.heroSetting = heroSetting;
		entity.Init();
		entity.AnimCmp.pause = true;
		entity.transform.rotation = new Quaternion(0, 180, 0,0);
		SceneLogic.GetInstance().RemoveSceneObjInfor(id);
		entity.AddEntityComponent<TestComponent>();
		testEntity[id] = entity;
		entity.transform.position = SceneLogic.GetInstance().MainHero.Position;
		
		
	}
	
	public override void OnAttachToEntity(SceneEntity ety)
    {
        BaseInit(ety);
    }
	public override void OnDetachFromEntity(SceneEntity ety)
    {
		base.OnDetachFromEntity(ety);
    }
	public override void DoUpdate()
    {
		if (ticker.IsEnable())
		{
			
			List<int> [] skillList = new List<int>[5];
			skillList[1] = new List<int>();
			skillList[1].Add(5);
			skillList[1].Add(6);
			skillList[1].Add(14);
			skillList[1].Add(13);
			
			skillList[2] = new List<int>();
			skillList[2].Add(17);
			skillList[2].Add(20);
			skillList[2].Add(16);
			skillList[2].Add(18);
			skillList[3] = new List<int>();
			skillList[3].Add(22);
			skillList[3].Add(25);
			skillList[3].Add(21);
			skillList[3].Add(23);
			skillList[4] = new List<int>();
			if (null != SceneLogic.GetInstance().MainHero)
			{
				Vector3 pos = SceneLogic.GetInstance().MainHero.Position;
				skillIndex ++ ;
				skillIndex %= 4;
				int skillId = skillList[Owner.property.Id][skillIndex];
				Owner.Action.PlayFightAnimation(SceneLogic.GetInstance().MainHero,(uint)skillId,SceneLogic.GetInstance().MainHero.Position);
				
			}
		}
		
	}
}
