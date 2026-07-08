using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.GarbageCans;
using System.Collections.Generic;
using HarmonyLib;

namespace GiftToTrash{
    
    public partial class ModEntry : Mod {
        
        public static IMonitor SMonitor;
        public static IModHelper SHelper;
        public static ModConfig Config;

        public override void Entry(IModHelper helper){

            SMonitor = Monitor;
            SHelper = helper;

            Config = Helper.ReadConfig<ModConfig>();
            if (!Config.ModEnabled) { return; }

            // when pull out new garbage can data
            Helper.Events.Content.AssetRequested += this.OnAssetRequested;

            // run harmony patches
            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.PatchAll();
        }

        // load gift to the trashcan loot
        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e){

            if(e.NameWithoutLocale.IsEquivalentTo("Data/GarbageCans")){
                if (!Context.IsWorldReady) return;
                e.Edit(asset =>
                {
                    // convert data to editable dictionary
                    var data = asset.AsDictionary<string, GarbageCanData>().Data;

                    string[] allTrashCans = {"JodiAndKent", "EmilyAndHaley", "Mayor", "Museum", "Blacksmith", "Saloon", "Evelyn", "JojaMart"};
                    
                    foreach (string canId in allTrashCans){
                        string itemKey = $"GiftToTrash.TrashItem.{canId}";

                        // if there're gifts for that trashcan in modData
                        if (Game1.player.modData.TryGetValue(itemKey, out string? giftListStr) && !string.IsNullOrWhiteSpace(giftListStr)){
                            // if there's can information
                            if (data.TryGetValue(canId, out var canData)){
                                // ??= if it's null
                                canData.CustomItems ??= new List<GarbageCanItemData>();
                                string[] itemIds = giftListStr.Split(',');

                                int i = 0;
                                foreach (string id in itemIds){
                                    var customEntry = new GarbageCanItemData{
                                        Id = $"GiftToTrash_Loot_{canId}_{i}",
                                        ItemId = id,
                                        Condition = null,
                                        Chance = 0.01f * i + 0.3f
                                    };

                                    canData.CustomItems.Add(customEntry);
                                    i++;
                                }

                            }
                        }
                    }
                });
            }
        }
    }
}