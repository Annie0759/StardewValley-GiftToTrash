# Gift To Trash (Stardew Valley Mod)

A [SMAPI](https://smapi.io/) mod for Stardew Valley. When you give a villager a gift they don't like, they might throw it away in a trash can near their home, and you might find it later while digging through the trash.

> **Status:** In development. The data and trash-can logic are written, but the hook into the game's gift-giving event is not connected yet.

## How it works

- **Villager-to-trash mapping:** Each villager is linked to a nearby trash can (for example, Haley and Emily use the trash can outside their house, and Clint uses the one by the Blacksmith).
- **Chance by gift taste:** Hated gifts have an 80% base chance of being thrown out, disliked gifts 60%, and neutral gifts 30%. The player's daily luck adjusts the chance.
- **Saved per player:** Discarded items are stored in the player's `modData`, so they persist in the save file.
- **Trash loot:** The mod edits the game's `Data/GarbageCans` asset to add discarded gifts to the matching trash can's loot table, and refreshes the asset whenever a new item is thrown away.

## Tech

C#, .NET 6, SMAPI 4, Harmony

## Building

Requires Stardew Valley, SMAPI 4.0 or later, and the .NET 6 SDK.

```bash
dotnet build
```

[Pathoschild.Stardew.ModBuildConfig](https://www.nuget.org/packages/Pathoschild.Stardew.ModBuildConfig) finds your game folder and deploys the mod to `Mods/` automatically. The mod can be turned off with `ModEnabled` in `config.json`.
