using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.UIElements;

namespace MT2StartingDraft.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool>? startingDraft;
        public static ConfigEntry<int>? starterCards;
        public static ConfigEntry<int>? stewardCards;

        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        public void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            startingDraft = Config.Bind("General",
                "StartingDraftEnabled",
                true,
                "Draft cards at the start of a run");

            starterCards = Config.Bind("General",
                "StarterCardsToAdd",
                0,
                "How many of each starter card to add");

            stewardCards = Config.Bind("General",
                "StewardCardsToAdd",
                0,
                "How many of each train steward card to add");

            // Uncomment if you need harmony patches, if you are writing your own custom effects.
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddStartOfRunRewards
    {
        public static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("AddStartOfRunRewards");
        public static void Postfix(SaveManager __instance, AllGameData ___allGameData, List<SaveManager.StartOfRunReward> ___startOfRunRewards, List<CardData> ___startOfRunShowcaseCards)
        {
            // Check if plugin is enabled
            if (Plugin.startingDraft != null && !Plugin.startingDraft.Value)
            {
                Log.LogInfo("Starting draft is disabled. Skipping start of run rewards.");
                return;
            }

            // Check if the run is an endless run and skip adding rewards if it is
            if (__instance.IsEndlessRun)
            {
                Log.LogInfo("Endless run detected. Skipping start of run rewards.");
                return;
            }

            // Check if the PyreArtifactData "Pyre Heart Dominion" exists
            PyreArtifactData pyreHeartDominion = ___allGameData.FindPyreArtifactData("c0c09adc-b23e-422e-bb75-689ee82cfa36");
            if (pyreHeartDominion == null)
            {
                Log.LogError("PyreArtifactData 'c0c09adc-b23e-422e-bb75-689ee82cfa36' not found. Cannot add start of run rewards.");
                return;
            }

            // Check if the Pyre Heart Dominion is already active
            if (__instance.GetActiveSavePyreArtifactState().GetRelicDataID() == pyreHeartDominion.GetID())
            {
                Log.LogInfo("Pyre Heart Dominion is active. Skipping start of run rewards.");
                return;
            }

            List<GrantableRewardData> startOfRunRewards = new List<GrantableRewardData>();

            // Add specific rewards to the start of run rewards
            List<String> rewardNamesToAdd = [];
            ClassData mainClass = __instance.GetMainClass();
            ClassData subClass = __instance.GetSubClass();

            rewardNamesToAdd.Add("CardDraftMainClassReward");
            rewardNamesToAdd.Add("CardDraftMainClassReward");
            rewardNamesToAdd.Add("CardDraftSubClassReward");
            rewardNamesToAdd.Add("CardDraftSubClassReward");
            rewardNamesToAdd.Add("CardDraftMainClassUncommonReward");
            rewardNamesToAdd.Add("CardDraftMainClassUncommonReward");
            rewardNamesToAdd.Add("CardDraftSubClassUncommonReward");
            rewardNamesToAdd.Add("CardDraftSubClassUncommonReward");
            rewardNamesToAdd.Add("CardDraftRareReward");

            if (mainClass.Cheat_GetNameEnglish() == "Awoken" || subClass.Cheat_GetNameEnglish() == "Awoken")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitAwoken");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitAwoken");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Banished" || subClass.Cheat_GetNameEnglish() == "Banished")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitBanished");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitBanished");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Hellhorned" || subClass.Cheat_GetNameEnglish() == "Hellhorned")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitHellhorned");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitHellhorned");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Lazarus League" || subClass.Cheat_GetNameEnglish() == "Lazarus League")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitLazarusLeague");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitLazarusLeague");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Luna Coven" || subClass.Cheat_GetNameEnglish() == "Luna Coven")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitLunaCoven");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitLunaCoven");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Melting Remnant" || subClass.Cheat_GetNameEnglish() == "Melting Remnant")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitRemnant");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitRemnant");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Pyreborne" || subClass.Cheat_GetNameEnglish() == "Pyreborne")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitPyreborne");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitPyreborne");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Stygian Guard" || subClass.Cheat_GetNameEnglish() == "Stygian Guard")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitStygian");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitStygian");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Umbra" || subClass.Cheat_GetNameEnglish() == "Umbra")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitUmbra");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitUmbra");
            }
            if (mainClass.Cheat_GetNameEnglish() == "Underlegion" || subClass.Cheat_GetNameEnglish() == "Underlegion")
            {
                rewardNamesToAdd.Add("CardDraftLevelUpUnitUnderlegion");
                rewardNamesToAdd.Add("CardDraftLevelUpUnitUnderlegion");
            }
            rewardNamesToAdd.Add("CardDraftLevelUpUnitMainOrAllied");

            foreach (String rewardName in rewardNamesToAdd)
            {
                GrantableRewardData rewardData = ___allGameData.FindRewardDataByName(rewardName);
                if (rewardData != null)
                {
                    DraftRewardData draftRewardData = rewardData as DraftRewardData;
                    startOfRunRewards.Add(rewardData);
                    Traverse.Create(draftRewardData).Field("draftOptionsCount").SetValue(3U);

                    if (rewardName == "CardDraftLevelUpUnitMainOrAllied")
                    {
                        Traverse.Create(draftRewardData).Field("useRunRarityFloors").SetValue(false);
                        Traverse.Create(draftRewardData).Field("rarityFloorOverride").SetValue(CollectableRarity.Rare);
                    }
                }
                else
                {
                    Log.LogWarning($"GrantableRewardData '{rewardName}' not found in AllGameData.");
                }
            }

            // Remove all non-champion, non-blight cards from the deck
            Log.LogInfo("Removing all non-champion, non-blight, cards from the deck...");
            List<CardState> cardsToRemove = [];
            foreach (CardState card in __instance.GetDeckState())
            {
                if (!card.IsChampionCard() && card.GetCardType() != CardType.Blight)
                {
                    cardsToRemove.Add(card);
                    Log.LogInfo($"Removing Card: {card.GetAssetName()}, ID: {card.GetID()}");
                }
            }
            foreach (CardState card in cardsToRemove)
            {
                __instance.RemoveCardFromDeck(card);
            }

            // Add the main and sub champion starter cards to the deck
            if (Plugin.starterCards != null && Plugin.starterCards.Value > 0)
            {
                CardData mainChampionStarterCard = __instance.GetMainChampionData().starterCardData;
                CardData subChampionStarterCard = __instance.GetSubChampionData().starterCardData;
                __instance.AddCardToDeck(mainChampionStarterCard, null, false, Plugin.starterCards.Value - 1, false, false, true, false, true);
                __instance.AddCardToDeck(subChampionStarterCard, null, false, Plugin.starterCards.Value - 1, false, false, true, false, true);
            }

            // Add the steward cards to the deck
            if (Plugin.stewardCards != null && Plugin.stewardCards.Value > 0)
            {
                CardData shieldSteward = ___allGameData.FindCardDataByName("TrainStewardShield");
                CardData spearSteward = ___allGameData.FindCardDataByName("TrainStewardSpear");
                __instance.AddCardToDeck(shieldSteward, null, false, Plugin.stewardCards.Value - 1, false, false, true, false, true);
                __instance.AddCardToDeck(spearSteward, null, false, Plugin.stewardCards.Value - 1, false, false, true, false, true);
            }

            // Add the start of run rewards to the SaveManager
            foreach (GrantableRewardData rewardData in startOfRunRewards)
            {
                Log.LogInfo($"Adding start of run reward: {rewardData.name}, Type: {rewardData.GetType()}, ID: {rewardData.GetID()}");
                SaveManager.StartOfRunReward startOfRunReward = new SaveManager.StartOfRunReward
                {
                    rewardData = rewardData
                };

                ___startOfRunRewards.Add(startOfRunReward);
            }

            ___startOfRunShowcaseCards.Clear();
        }
    }

    [HarmonyPatch(typeof(MapScreen), "TryShowRunOpeningRewards")]
    public class TryShowRunOpeningRewards
    {
        public static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("TryShowRunOpeningRewards");
        public static bool Prefix(ref bool __result, MapScreen __instance, SaveManager ___saveManager, ScreenManager ___screenManager)
        {
            Log.LogInfo("Checking if run opening rewards should be shown...");
            if (!___saveManager.AreRunOpeningRewardsCompleted() && ___saveManager.StartOfRunRewards.Count > 0)
            {
                List<RewardState> rewardStates = new List<RewardState>();
                foreach (SaveManager.StartOfRunReward startOfRunReward in ___saveManager.StartOfRunRewards)
                {
                    RewardState rewardState = new RewardState(startOfRunReward.rewardData, ___saveManager);
                    rewardStates.Add(rewardState);
                    if (startOfRunReward.relicEffectSource != null && startOfRunReward.relicEffectSource.CanShowNotifications)
                    {
                        rewardState.SetRelicIdToNotify(startOfRunReward.relicEffectSource.GetRelicDataId());
                    }
                }
                ___screenManager.ShowScreen(ScreenName.Reward, delegate (IScreen screen)
                {
                    ((RewardScreen)screen).Show(rewardStates, GrantableRewardData.Source.Map, null, false, null);
                });
                ___screenManager.OnScreenCompleted(ScreenName.Reward, delegate (IScreen screen)
                {
                    Traverse.Create(__instance).Method("HandleCurrentGameSequence").GetValue();
                });
                ___saveManager.SetRunOpeningRewardsCompleted();
                __result = true;
            }
            __result = false;
            Log.LogInfo("Run opening rewards check completed. Result: " + __result);
            return false;
        }
    }
}
