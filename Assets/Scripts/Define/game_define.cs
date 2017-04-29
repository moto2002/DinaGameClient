/*
compile by protobuf, please don't edit it manually. 
Any problem please contact tongxuehu@gmail.com, thx.
*/

using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Lib.Net;

namespace Assets.Scripts.Define
{
	public enum KSceneObjectType
	{
		sotInvalid = 0,
		sotHero = 1,
		sotDoodad = 2,
		sotTotal = 3,
	}

	public enum KDoodadType
	{
		dddInvalid = 0,
		dddCollect = 1,
		dddDrop = 2,
	}

	public enum KHeroObjectType
	{
		hotInvalid = 0,
		hotPlayer = 1,
		hotNpc = 2,
		hotMonster = 3,
		hotTotal = 4,
	}

	public enum KMoveState
	{
		mosInvalid = 0,
		mosStand = 1,
		mosMove = 2,
		mosDeath = 3,
		mosForceMove = 4,
		mosTotal = 5,
	}

	public enum KHeroItemPos
	{
		eipEquipBegin = 0,
		eipHeadWear = eipEquipBegin,
		eipChest = 1,
		eipPants = 2,
		eipBangle = 3,
		eipBelt = 4,
		eipBoots = 5,
		eipOrnament = 6,
		eipNeck = 7,
		eipRing = 8,
		eipSachet = 9,
		eipAmulet = 10,
		eipWeapon = 11,
		eipMask = 12,
		eipBackOrnament = 13,
		eipOutDress1 = 14,
		eipOutDress2 = 15,
		eipTotal = 16,
	}

	public enum KAttributeType
	{
		atInvalid = 0,
		atMaxHP = 1,
		atMaxHPPrecent = 2,
		atMaxMP = 3,
		atMaxMPPrecent = 4,
		atAttack = 5,
		atAttackPrecent = 6,
		atDefence = 7,
		atDefencePrecent = 8,
		atReflex = 9,
		atReflexPrecent = 10,
		atCrit = 11,
		atCritPrecent = 12,
		atCritHurt = 13,
		atCritHurtPrecent = 14,
		atReduceCrit = 15,
		atReduceCritPrecent = 16,
		atReduceCritHurt = 17,
		atReduceCritHurtPrecent = 18,
		atHpRecover = 19,
		atMpRecover = 20,
		atAttackSpeed = 21,
		atAttackSpeedPrecent = 22,
		atReduceDamage = 23,
		atReduceDamagePrecent = 24,
		atReduceDefence = 25,
		atReduceDefencePrecent = 26,
		atMiss = 27,
		atMissPrecent = 28,
		atDamageMore = 29,
		atDamageLess = 30,
		atDamageBack = 31,
		atAttackRecover = 32,
		atExtDamage = 33,
		atUpAttack = 34,
		atMoveSpeed = 35,
		atMoveSpeedPrecent = 36,
		atNumAtrribute = 37,
		atCurrentHP = 38,
		atCurrentHPPrecent = 39,
		atCurrentMP = 40,
		atCurrentMPPrecent = 41,
		atForbitMove = 42,
		atForbitSkill = 43,
		atCostSkillMPValue = 44,
		atContrlExemptRate = 45,
		atDamageExemptRate = 46,
		atScript = 47,
		atTotal = 48,
	}

	public enum KRelationType
	{
		rltNone = 0,
		rltMe = 1,
		rltUs = 2,
		rltEnemy = 4,
		rltTotal = 5,
	}

	public enum KConstDefine
	{
		cdNoTeamWinID = 255,
		cdMaxHeroLevel = 60,
		cdRoomPasswordMaxLen = 32,
		cdMaxItemBaseAttrCount = 6,
		cdLadderRoomCountDownSeconds = 5,
		cdInitPlayerPageSize = 32,
		cdMaxPlayerPageSize = 40,
		cdPerExtendPlayerPackageSize = 5,
		cdExtendPlayerPackageMaxCount = 25,
		cdInitPageCount = 2,
		cdMaxPageCount = 7,
		cdMaxEventParamCount = 3,
		cdInvalidFelllowshipGroupID = 250,
		cdMaxVIPLevel = 10,
		cdMaxQuestEventCount = 2,
		cdQuestParamCount = 8,
		cdMaxAcceptQuestCount = 25,
		cdShotcutSize = 3,
		cdDefalutBattleTime = 180,
		cdMaxUsingHeroCount = 3,
		cdDefaultTalkDataLength = 512,
		cdMaxLadderTopListMemberCount = 1000,
		cdLadderTopListCountPerPage = 10,
		cdPlayerPageSize = 25,
		cdMaxMapEventCout = 4,
		cdDefaultGroupID = 1,
	}

	public enum KGender
	{
		gNone = 0,
		gMale = 1,
		gFemale = 2,
	}

	public enum KChatType
	{
		trInvalid = 0,
		trWhisper = 1,
		trFace = 2,
		trRoom = 3,
		trTeam = 4,
		trScene = 5,
		trPhysicalServer = 6,
		trBigHorn = 7,
		trGmMessage = 8,
		trLocalSys = 9,
		trGlobalSys = 10,
		trGmAnnounce = 11,
		trToPlayerGmAnnounce = 12,
		trSmallHorn = 13,
		trWorld = 14,
		trAllFriend = 15,
		trClub = 16,
		trNews = 17,
		trReceiveMessage = 18,
		trRefuseMessage = 19,
		trCustomCh1 = 100,
		trCustomCh2 = 101,
		trCustomCh3 = 102,
		trCustomCh4 = 103,
		trCustomCh5 = 104,
		trCustomCh6 = 105,
		trCustomCh7 = 106,
		trCustomCh8 = 107,
		trTotal = 108,
	}

	public enum KItemTableType
	{
		ittInvalid = 0,
		ittEquip = 1,
		ittOther = 2,
		ittTotal = 3,
	}

	public enum KItemBindType
	{
		ibtInvalid = 0,
		ibtNeverBind = 1,
		ibtBindOnEquipped = 2,
		ibtBindOnPicked = 3,
		ibtTotal = 4,
	}

	public enum KEquipMentSubType
	{
		espBegin = 0,
		espHeadWear = espBegin,
		espChest = 1,
		espPants = 2,
		espBangle = 3,
		espBelt = 4,
		espBoots = 5,
		espOrnament = 6,
		espNeck = 7,
		espRing = 8,
		espSachet = 9,
		espAmulet = 10,
		espWeapon = 11,
		espMask = 12,
		espBackOrnament = 13,
		espOutDress1 = 14,
		espOutDress2 = 15,
		espTotal = 16,
	}

	public class KCommonItemData: KProtoBuf
	{
		public byte byTabType;
		public ushort wTabIndex;
		public byte byBind;
		public uint uGenTime;
		public uint uStackNum;

		public KCommonItemData()
		{
			Register("byTabType", typeof(uint).ToString() , 1, 1);
			Register("wTabIndex", typeof(uint).ToString() , 2, 1);
			Register("byBind", typeof(uint).ToString() , 1, 1);
			Register("uGenTime", typeof(uint).ToString() , 4, 1);
			Register("uStackNum", typeof(uint).ToString() , 4, 1);
		}
	}

	public class KAttrData: KProtoBuf
	{
		public ushort wKey;
		public ushort wValue;

		public KAttrData()
		{
			Register("wKey", typeof(uint).ToString() , 2, 1);
			Register("wValue", typeof(uint).ToString() , 2, 1);
		}
	}

	public class KEquipDataForClient: KCommonItemData
	{
		public byte uLevel;
		public byte uStrengthenLevel;
		public byte uHole;

		public KEquipDataForClient()
		{
			Register("uLevel", typeof(uint).ToString() , 1, 1);
			Register("uStrengthenLevel", typeof(uint).ToString() , 1, 1);
			Register("uHole", typeof(uint).ToString() , 1, 1);
		}
	}

	public enum KFellowShipRequest
	{
		KFellowRequestFriend = 1,
		KFellowRequestBlackList = 2,
	}

	public enum KPackageType
	{
		ePlayerPackage = 1,
		eHeroPackage = 2,
	}

	public enum KPlayerPackageIndex
	{
		eppiBegin = 0,
		eppiPlayerItemBox = eppiBegin,
		eppiTotal = 1,
	}

	public enum KPlayerPackageExchangeResult
	{
		eppeInvaild = 0,
		eppeSrcItemCanNotPutInHeroPackage = 1,
		eppeHeroLevelInconformityRequireLevel = 2,
		eppeSrcItemCanNotPutInPlayerPackage = 3,
		eppeSrcItemCanNotPutInShotcutBar = 4,
		eppeSrcItemCanNotUseInScene = 5,
		eppeDstItemCanNotPutInHeroPackage = 6,
		eppeDstItemCanNotPutInPlayerPackage = 7,
		eppeDstItemCanNotPutInShotcutBar = 8,
		eppeDstItemCanNotUseInScene = 9,
		eppeDstItemNotEqualToSrcItem = 10,
		eppeShotcutBarHasSameItem = 11,
		eppeSuccessed = 12,
		eppeTotal = 13,
	}

	public enum KPlayerEvent
	{
		peInvalid = 0,
		peAcceptQuest = 1,
		peClickNPC = 2,
		peNPCDie = 3,
		peOpenBag = 4,
		peRunWithBall = 5,
		pePassBall = 6,
		pePassEnemyWithBall = 7,
		peUseHandInAir = 8,
		peUseFootInAir = 9,
		peUseHandInHurt = 10,
		peUseFootInHurt = 11,
		peUseHandInRun = 12,
		peUseFootInRun = 13,
		peAWSDAllClick = 14,
		peTakeBall = 15,
		peTakeTyre = 16,
		peTakeLadder = 17,
		peFinishDramaDialog = 18,
		peViewSkill = 19,
		peClientEnd = 20,
		peShootFirstSuccess = 21,
		peShootSecondSuccess = 22,
		peShootThirdSuccess = 23,
		peKillSaMa = 24,
		peKillLiZheng = 25,
		peKillSaMaAndLiZheng = 26,
		peFinishLadderPVP = 27,
		peWinFreePVP = 28,
		peWinLadderPVP = 29,
		peDownBasket = 30,
		peWin10ScoreAt30Second = 31,
		peNotUseItemAtGame = 32,
		peUseSceneObj = 33,
		peWin40Score = 34,
		peEnemy0Score = 35,
		peNotDieAndWin = 36,
		peUseTire = 37,
		peUseLadder = 38,
		peUseSkillShoot = 39,
		peUseNormalShoot = 40,
		peUseSkillSlam = 41,
		peInstallBasket = 42,
		peFinishEasyKanshousuo = 43,
		peFinishNormalKanshousuo = 44,
		peFinishHardKanshousuo = 45,
		peFinishEasyTangRenJie = 46,
		peFinishNormalTangRenJie = 47,
		peFinishHardTangRenJie = 48,
		peFinishEasyTangGuoDuShi = 49,
		peFinishNormalTangGuoDuShi = 50,
		peFinishHardTangGuoDuShi = 51,
		peFinishEasyYangguanghaitan = 52,
		peFinishNormalYangguanghaitan = 53,
		peFinishHardlYangguanghaitan = 54,
		peFinishEasyWuLiYanJiuSuo = 55,
		peFinishNormalWuLiYanJiuSuo = 56,
		peFinishHardlWuLiYanJiuSuo = 57,
		peSlamDunk = 58,
		peWinPVE = 59,
		pePutOnEquipment = 60,
		peEmployHero = 61,
		peMakeEquipment = 62,
		peFinishKanShouSuo = 63,
		peFinishBangQiuChang = 64,
		peFinishTangGuoDuShi = 65,
		peFinishWuLiYanJiuSuo = 66,
		peFinishYangGuangHaiTan = 67,
		peFinishTangRenJie = 68,
		peFinishMoTianDaLou = 69,
		peFinishHangKongMuJian = 70,
		peFinishEasyBangQiuChang = 71,
		peFinishNormalBangQiuChang = 72,
		peFinishHardBangQiuChang = 73,
		peFinishEasyMoTianDaLou = 74,
		peFinishNormalMoTianDaLou = 75,
		peFinishHardMoTianDaLou = 76,
		peFinishEasyHangKongMuJian = 77,
		peFinishNormalHangKongMuJian = 78,
		peFinishHardHangKongMuJian = 79,
		peFinishDailyQuest = 80,
		peObtainMoneyInBusinessStreet = 81,
		peConsumeItem = 82,
		peConsumeMoney = 83,
		peObtainItem = 84,
		peObtainMoney = 85,
		peAddFriend = 86,
		peBuyCheerleading = 87,
		peBuyFashion = 88,
		peHeroLevelup = 89,
		peExtendBag = 90,
		peVIPLevelup = 91,
		peFinishBattle = 92,
		peFinishQuest = 93,
		peSafeBoxLevelup = 94,
		peUseGymMachine = 95,
		peGymMachineLevelup = 96,
		peViewReport = 97,
		peUseHeroTraining = 98,
		peMakingMachineLevelup = 99,
		peBuyLand = 100,
		peBuildStore = 101,
		peStoreLevelup = 102,
		peStrengthenEquipFailed = 103,
		peStrengthenEquipSuccess = 104,
		peFinishAchievement = 105,
		peWinContinuousChallenge = 106,
		peBasketballCourtLevelup = 107,
		peFinishPVE = 108,
		peSelectHeroSkill = 109,
		peFinishQianChongQuan = 110,
		peFinishKongZhongJiao = 111,
		peFinishQiShenQuan = 112,
		peBuyItem = 113,
		peEquipFashion = 114,
		peEquipCheerleading = 115,
		peSelectMainHero = 116,
		peOnline2Hour = 117,
		peCastFatiguePoint100 = 118,
		peDailySign = 119,
		peInjoinPart = 120,
		peFinishOneChainQuest = 121,
		peLadderLevelup = 122,
		peAddVIPExp = 123,
		peFightDownLiZheng = 124,
		peFightDownSaMa = 125,
		peFightDownSiMaTe = 126,
		peFightDownNuoCong = 127,
		peFightDownHengLi = 128,
		peFightDownSiTa = 129,
		peFightDownShanXia = 130,
		peFightDownCanDi = 131,
		peFinishRandomQuest = 132,
		peSelectQianChongQuan = 133,
		peSelectKongZhongJiao = 134,
		peFightDownNpc = 135,
		peCastNirvanaSkill = 136,
		peDecomposeEquip = 137,
		peBeAttackBySceneObj = 138,
		peEnemyBeAttackBySceneObj = 139,
		peNpcStaminaEmpty = 140,
		peShootSuccess = 141,
		peDoDamage = 142,
		peBearDamage = 143,
		peScore = 144,
		peContinueLadderPVPLose = 145,
		peContinueLadderPVPWin = 146,
		peHeroLearnSkill = 147,
		peGetOnlineAward = 148,
		peEquipLevelUp = 149,
		peCollect = 150,
		peTotal = 151,
	}

	public enum KQuestResultCode
	{
		qrcInvalid = 0,
		qrcSuccess = 1,
		qrcFailed = 2,
		qrcErrorQuestIndex = 3,
		qrcErrorQuestID = 4,
		qrcQuestListFull = 5,
		qrcErrorQuestState = 6,
		qrcAlreadyAcceptQuest = 7,
		qrcAlreadyFinishedQuest = 8,
		qrcCannotFindQuest = 9,
		qrcErrorRepeat = 10,
		qrcTooLowLevel = 11,
		qrcTooHighLevel = 12,
		qrcErrorGender = 13,
		qrcErrorJob = 14,
		qrcPrequestUnfinished = 15,
		qrcErrorItemCount = 16,
		qrcErrorTaskValue = 17,
		qrcAddItemFailed = 18,
		qrcAddMoneyFailed = 19,
		qrcNotEnoughFreeRoom = 20,
		qrcNeedAccept = 21,
		qrcNoNeedAccept = 22,
		qrcNotEnoughMoney = 23,
		qrcMoneyLimit = 24,
		qrcClientCannotModify = 25,
		qrcDailyQuestFull = 26,
		qrcFatiguePointNotEnough = 27,
		qrcTotal = 28,
	}

	public enum KQuestState
	{
		qsInvalid = -1,
		qsUnfinished = 0,
		qsFinished = 1,
	}

	public enum KQuestType
	{
		qtInvalid = 0,
		qtTrunk = 1,
		qtBranches = 2,
		qtDaily = 3,
	}

	public enum KQuestDataType
	{
		qdtInvalid = -1,
		qdtQuestState = 0,
		qdtQuestList = 1,
		qdtDailyQuest = 2,
	}

	public enum KMapType
	{
		mapInvalid = 0,
		mapNormal = 1,
		mapPVEMap = 2,
		mapTotal = 3,
	}

	public enum KNumMoneyType
	{
		emotBegin = 0,
		emotMoney = emotBegin,
		emotCoin = 1,
		emotMenterPoint = 2,
		emotReserved2 = 3,
		emotReserved3 = 4,
		emotReserved4 = 5,
		emotReserved5 = 6,
		emotReserved6 = 7,
		emotTotal = 8,
	}

	public enum KGateWayHandShakeCode
	{
		ghcHandshakeSucceed = 1,
		ghcGatewayVersionError = 2,
		ghcGameWorldMaintenance = 3,
		ghcAccountSystemLost = 4,
		ghcGameWorldVersionError = 5,
	}

	public enum KCreateRoleRespondCode
	{
		eCreateRoleSucceed = 1,
		eCreateRoleNameAlreadyExist = 2,
		eCreateRoleInvalidRoleName = 3,
		eCreateRoleNameTooLong = 4,
		eCreateRoleNameTooShort = 5,
		eCreateRoleUnableToCreate = 6,
	}

	public enum KDeleteRoleRespondCode
	{
		eDeleteRoleSucceed = 1,
		eDeleteRoleDelay = 2,
		eDeleteRoleFreezeRole = 3,
		eDeleteRoleUnknownError = 4,
	}

	public enum KGameLoginRespondCode
	{
		eGameLoginSucceed = 1,
		eGameLoginSystemMaintenance = 2,
		eGameLoginQueueWait = 3,
		eGameLoginOverload = 4,
		eGameLoginRoleFreeze = 5,
		eGameLoginRoleCenterSwitching = 6,
		eGameLoginRoleChangeAccount = 7,
		eGameLoginUnknownError = 8,
	}

	public enum KRenameRespondCode
	{
		eRenameSucceed = 1,
		eRenameNameAlreadyExist = 2,
		eRenameNewNameError = 3,
		eRenameNewNameTooLong = 4,
		eRenameNewNameTooShort = 5,
		eRenameUnknownError = 6,
	}

	public enum KAutoMatchLeaveReason
	{
		KAutoMatchLeaveReasonUnknow = 0,
		KAutoMatchLeaveReasonSelfRequest = 1,
		KAutoMatchLeaveReasonSelfConnectionLost = 2,
		KAutoMatchLeaveReasonTeammateLeave = 3,
		KAutoMatchLeaveReasonAutoMatchSuccess = 4,
	}

	public enum KEnumEquipQuality
	{
		eqWhite = 0,
		eqGreen = 1,
		eqBlue = 2,
		eqPurple = 3,
		eqPink = 4,
		eqOrange = 5,
		eqGold = 6,
		eqTotal = 7,
	}

	public enum KMailType
	{
		mtPlayer = 0,
		mtSystem = 1,
		mtShop = 2,
		mtTotal = 3,
	}

	public class KMailItemDesc: KProtoBuf
	{
		public byte bAcquired;
		public byte byDataLen;
		public byte byQuality;

		public KMailItemDesc()
		{
			Register("bAcquired", typeof(uint).ToString() , 1, 1);
			Register("byDataLen", typeof(uint).ToString() , 1, 1);
			Register("byQuality", typeof(uint).ToString() , 1, 1);
		}
	}

	public class KMailContent: KProtoBuf
	{
		public int nMoney;
		public ushort wTextLen;
		public KMailItemDesc[] ItemDesc = new KMailItemDesc[6]{new KMailItemDesc(),new KMailItemDesc(),new KMailItemDesc(),new KMailItemDesc(),new KMailItemDesc(),new KMailItemDesc()};
		public List<byte> byData = new List<byte>();

		public KMailContent()
		{
			Register("nMoney", typeof(int).ToString() , 4, 1);
			Register("wTextLen", typeof(uint).ToString() , 2, 1);
			Register("ItemDesc", typeof(KMailItemDesc).ToString() , 0, 6);
			Register("byData", typeof(uint).ToString() , 1, 0);
		}
	}

	public enum KMessage
	{
		KMessageSuccess = 0,
		KMessageTest = 1,
		KMessageUnknowError = 2,
		KMessageWhisperTargetNotFound = 3,
		KMessageLadderTopListTeamNotExist = 4,
		KMessageLadderTopListSelfNotIn = 5,
		KMessageLadderTopListTeamNotIn = 6,
	}

	public enum KTopListType
	{
		KTopListTypeLadder = 1,
		KTopListTypeHeroEquip = 2,
		KTopListTypeTeamEquip = 3,
	}

	public enum KRPCType
	{
		rpcInvalid = -1,
		rpcByte = 0,
		rpcInteger32 = 1,
		rpcInteger64 = 2,
		rpcFloat = 3,
		rpcString = 4,
		rpcTypeTotal = 5,
	}

	public enum KAttackEvent
	{
		aeNormal = 0,
		aeMiss = 1,
		aeCrit = 2,
		aeDeBuf = 3,
	}

	public enum KSkillTargetType
	{
		sttNone = 0,
		sttObj = 1,
		sttPos = 2,
		sttSelf = 3,
	}

	public enum KEffectArea
	{
		eaNone = 0,
		eaSingle = 1,
		eaRect = 2,
		eaCirc = 3,
		eaFan = 4,
	}

	public enum KEffectTarget
	{
		etNone = 0,
		etSelf = 1,
		etEnemy = 2,
		etFriend = 3,
		etAll = 4,
	}

	public enum KJob
	{
		jobNone = 0,
		JobBlade = 1,
		JobLance = 2,
		JobHammer = 3,
		JobBoxer = 4,
	}

	public enum KItemGenre
	{
		igNone = 0,
		igEquip = 1,
		igCommon = 2,
		igEnd = 3,
	}

	public enum KItemQuality
	{
		iqNone = 0,
		iqWhite = 1,
		iqGreen = 2,
		iqBlue = 3,
		iqPurple = 4,
		iqOrange = 5,
		iqEnd = 6,
	}

	public enum KItemPrefix
	{
		ipNone = 0,
		ipNormal = 1,
		ipFine = 2,
		ipPerfect = 3,
		ipEnd = 4,
	}

	public enum KItemJob
	{
		kjNone = 0,
		kjMagic = 1,
		kjPhysic = 2,
		kjAssassin = 3,
		kjEnd = 4,
	}

	public enum KItemSex
	{
		ksNone = 0,
		ksMail = 1,
		ksFemail = 2,
		ksEnd = 3,
	}

	public enum KItemBind
	{
		kbNone = 0,
		kbUse = 1,
		kbGet = 2,
		kbEnd = 3,
	}

	public enum KItemUseSign
	{
		kuNone = 0,
		kuUse = kuNone,
		kuUnuse = 1,
		kuEnd = 2,
	}

	public enum KItemDropSign
	{
		kdNone = 0,
		kdYes = kdNone,
		kdNo = 1,
		kdEnd = 2,
	}

	public enum KItemWashSign
	{
		kwNone = 0,
		kwNo = kwNone,
		kwYes = 1,
		kwEnd = 2,
	}

	public enum KItemBroadcastSign
	{
		kbsNone = 0,
		kbsNo = kbsNone,
		kbsYes = 1,
		kbsEnd = 2,
	}

	public enum AI_ACTION
	{
		eakInvalid = 0,
		eakGoToState = 1,
		eakSetPrimaryTimer = 2,
		eakSetSecondaryTimer = 3,
		eakSetTertiaryTimer = 4,
		eakRandomBiBranch = 5,
		eakRandomTriBranch = 6,
		eakSetRandomTimer = 7,
		eakNoneOp = 8,
		eakLog = 9,
		eakCheckLoop = 10,
		eakSetLoopCount = 11,
		eakSaveCurPos = 12,
		eakFindEnemy = 13,
		eakSetMoveTarget = 14,
		eakMoveToTarget = 15,
		eakStand = 16,
		eakCastSkill = 17,
		eakAutoMove = 18,
		eakStopAutoMove = 19,
		eakSetUseSkill = 20,
		eakNpcAddSkill = 21,
		eakNpcDelSkill = 22,
		eakCheckSkillToEnemy = 23,
		eakCheckSkillRangeToEnemy = 24,
		eakCheckNearTarget = 25,
		eakCheckSelfHP = 26,
		eakCheckTargetHeroState = 27,
		eakCheckSelfState = 28,
		eakCheckCastingSkill = 29,
		eakCheckCurSkillCD = 30,
		eakCheckCastCurSkill = 31,
		eakCheckChaseRange = 32,
		eakCheckStiff = 33,
		eakCheckEnemyDistance = 34,
		eakTotal = 35,
	}

	public enum AI_EVENT
	{
		aevInvalid = 0,
		aevOnPrimaryTimer = 1,
		aevOnSecondaryTimer = 2,
		aevOnTertiaryTimer = 3,
		aevOnBeAttacked = 4,
		aevTotal = 5,
	}

	public enum KHeroType
	{
		htInvalid = 0,
		htPlayer = 1,
		htNpc = 2,
		htMonster = 3,
	}

	public enum KMonsterGrade
	{
		mgInvalid = 0,
		mgNormal = 1,
		mgMiniBoss = 2,
		mgBigBoss = 3,
		mgGoldBoss = 4,
		mgQuestBoss = 5,
	}

	public enum KSkillProcess
	{
		spInvalid = 0,
		spBegin = 1<<0,
		spFirst = 1<<1,
		spEvery = 1<<2,
		spEnd = 1<<3,
	}

	public enum KForceMoveType
	{
		fmtInvalid = 0,
		fmtJump = 1,
		fmtRush = 2,
		fmtPull = 3,
		fmtBack = 4,
		fmtKeepLength = 5,
		fmtPlayerFuKong = 6,
	}

	public enum KForceMoveObj
	{
		fmoInvalid = 0,
		fmoSelf = 1,
		fmoOther = 2,
	}

	public enum KForceMoveDes
	{
		fmdInvalid = 0,
		fmdCaster = 1,
		fmdSkillTarget = 2,
		fmdSkillPos = 3,
	}

	public enum KDropType
	{
		dtInvalid = 0,
		dtEquip = 1,
		dtItem = 2,
		dtMoney = 3,
	}

	public enum KPickUpItemResult
	{
		puirInvalid = 0,
		puirSuccess = 1,
		puirDeleteTime = 2,
		puirCantPickUp = 3,
		puirDistance = 4,
		puirAddFail = 5,
	}

	public enum KReliveType
	{
		rltInvalid = 0,
		rltJustHero = 1,
		rltMapRule = 2,
	}

	public enum KPlayerLogAction
	{
		plaInvalid = 0,
		plaAcceptQuest = 1,
		plaFinishQuest = 2,
		plaCancelQuest = 3,
		plaPickUpItem = 4,
		plaPutOnEquip = 5,
		plaSetPoint = 6,
		plaSetQuestValue = 7,
		plaActiveSkillUp = 8,
		plaPassiveSkillUp = 9,
		plaLevelUp = 10,
		plaUseItem = 11,
	}

	public enum KPlayerLogType
	{
		pltInvalid = 0,
		pltAddExp = 1,
		pltAddItem = 2,
		pltCostItem = 3,
		pltAddMoney = 4,
		pltCostMoney = 5,
		pltCostSkillPoint = 6,
	}

	public enum KStlOpCode
	{
		stlFind = 0,
		stlBegin = 1,
		stlNext = 2,
		stlErase = 3,
		stlRemove = 4,
		stlLength = 5,
	}

	public enum KGiftType
	{
		gtOnlineTime = 0,
		gtCombat = 1,
		gtLevel = 2,
	}

	public enum KCollectResult
	{
		crInvalid = 0,
		crSuccess = 1,
		crFailed = 2,
	}

	public enum KSceneState
	{
		ssInvalid = 0,
		ssLoading = 1,
		ssCompleteLoading = 2,
		ssFailedLoading = 3,
		ssWaitingClientLoading = 4,
		ssCountDown = 5,
		ssFighting = 6,
		ssWatingDelete = 7,
		ssTotal = 8,
	}

}
