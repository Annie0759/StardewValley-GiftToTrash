using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.GarbageCans;
using System.Collections.Generic;
using HarmonyLib;

namespace GiftToTrash{
    
    public partial class ModEntry : Mod {
        
        public static IMontior SMonitor;
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
            var harmony = new HarmonyLib(ModManifest.UniqueID);
            harmony.PatchAll();
        }

        // load gift to the trashcan loot
        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e){

            if(e.NameWithoutLocale.IsEquivalentTo("Data/GarbageCans")){
                e.Edit(asset =>
                {
                    // convert data to editable dictionary
                    var data = asset.AsDictionary<string, GarbageCanData>().Data;
                })
            }
        }
    }
}