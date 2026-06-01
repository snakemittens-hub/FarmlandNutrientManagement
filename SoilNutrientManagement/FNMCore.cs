using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace FarmlandNutrientManagement;

public class FNMCore : ModSystem
{
    // Called on server and client
    public override void Start(ICoreAPI api)
    {
        api.RegisterBlockBehaviorClass("UpgradeFarmland", typeof(UpgradeFarmlandBehavior));
        api.Logger.Notification("Farmland Nutrient Management Mod: Started.");
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        ModConfig.tryToLoadConfig(api);
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        ModConfig.tryToLoadConfig(api);
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        if (api.Side != EnumAppSide.Server)
        {
            return;
        }

        foreach (Block block in api.World.Blocks)
        {
            // Make sure block or its code is not null
            if (block == null || block.Code == null)
                continue;

            // Only apply behavior to crops
            if (block is not BlockCrop)
                continue;

            UpgradeFarmlandBehavior blockBehavior = new UpgradeFarmlandBehavior(block);

            // Add UpgradeFarmland behavior to all crops
            block.CollectibleBehaviors = block.CollectibleBehaviors.Append(blockBehavior);
            block.BlockBehaviors = block.BlockBehaviors.Append(blockBehavior);

        }
    }
}
