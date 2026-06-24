using System;
using StardewModdingAPI;
using StardewValley;

namespace GiftToTrash
{
    public partial class ModEntry
    {
        private static void ThrowGiftInTrash(Item gift, NPC npc, int giftTaste){
            // connect NPC to their designated trashcan
            string? trashCanId = npc.Name switch{
                // direct residents
                "Alex" or "George" or "Evelyn" => "Evelyn",
                "Haley" or "Emily" => "EmilyAndHaley",
                "Sam" or "Jodi" or "Kent" or "Vincent" => "JodiAndKent",
                "Clint" => "Blacksmith",
                "Lewis" => "Mayor",
                "Gus" => "Saloon",
                "Shane" or "Pam" => "JojaMart",

                // Other residents
                "Robin" or "Willy" or "Abigail" or "Harvey" or "Pierre" or "Caroline" or "Penny" => "Saloon",
                "Dwarf" or "Jas" or "Wizard" or "Maru" or "Demetrius" => "Museum",
                "Marnie" => "Mayor",
                "Sebastian" => "JodiAndKent",
                "Linus" => "Evelyn",

                _ => null
                
            };

            if (trashCanId == null){ return; }

            if (ShouldBeInTrash(giftTaste)){
                SaveItemToData(gift, trashCanId);
            } 

            // delete this after testing
            else {
                SMonitor.Log($"{npc.Name} destroyed the {gift.Name} completely. (Luck roll failed).", LogLevel.Debug);
            }
        }

        private static bool ShouldBeInTrash(int giftTaste){
            double baseChance = giftTaste switch{
                6 => 0.8,
                4 => 0.6,
                8 => 0.3,
                _ => 0
            };

            double luckBonus = Game1.player.DailyLuck;
            double finalChance = luckBonus + baseChance;

            Random r = new Random();
            return r.NextDouble() <= finalChance;
        }

        private static void SaveItemToData(Item gift, string trashCanId){
            string itemKey = $"GiftToTrash.TrashItem.{trashCanId}";

            // check if a gift already exists, add to it
            if (Game1.player.modData.TryGetValue(itemKey, out string? existingGifts) && !string.IsNullOrWhiteSpace(existingGifts)){
                Game1.player.modData[itemKey] = $"{existingGifts},{gift.QualifiedItemId}";
            } else {
                Game1.player.modData[itemKey] = gift.QualifiedItemId;
            }

            SHelper.GameContent.InvalidateCache("Data/GarbageCans");
        }
    }
}