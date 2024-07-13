using System.Collections.Generic;
using Terraria.UI;

namespace PathOfTerraria.Content.GUI;

[Autoload(Side = ModSide.Client)]
public sealed class UIArmorInventoryManager : ModSystem
{
	// Terraria doesn't provide any game time instance during rendering, so we keep track of it ourselves.
	private static GameTime lastGameTime;
	
	public static UserInterface UserInterface { get; private set; }

	public override void Load()
	{
		UserInterface = new UserInterface();
		UserInterface.SetState(new UIArmorInventory());
	}

	public override void Unload()
	{
		UserInterface?.SetState(null);
		UserInterface = null;
	}

	public override void UpdateUI(GameTime gameTime)
	{
		UserInterface.Update(gameTime);
		
		lastGameTime = gameTime ?? new GameTime();
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		var index = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");

		if (index == -1)
		{
			return;
		}
		
		layers.Insert(
			index, 
			new LegacyGameInterfaceLayer(
				$"{nameof(PathOfTerraria)}:{nameof(UIArmorInventory)}",
				() =>
				{
					UserInterface.Draw(Main.spriteBatch, lastGameTime);
					
					return true;
				}
			)
		);
	}
}