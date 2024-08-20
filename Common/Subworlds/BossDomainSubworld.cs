﻿using PathOfTerraria.Common.Subworlds.Passes;
using PathOfTerraria.Common.Systems;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using ReLogic.Graphics;

namespace PathOfTerraria.Common.Subworlds;

/// <summary>
/// This is used to stop bosses from doing special death effects (like King Slime spawning the town slime, WoF spawning hardmode) automatically,<br/>
/// using a nicer loading screen dialogue, and setting <see cref="WorldGenerator.CurrentGenerationProgress"/> as it's not set by default.<br/>
/// The death effect system is in <see cref="BossTracker.CachedBossesDowned"/>.
/// </summary>
public abstract class BossDomainSubworld : MappingWorld
{
	public override bool ShouldSave => false;
	public override bool NoPlayerSaving => false;

	public virtual int[] WhitelistedTiles => [];

	// We are going to first set the world to be completely flat so we can build on top of that
	public override List<GenPass> Tasks => [new FlatWorldPass()];

#pragma warning disable IDE0060 // Remove unused parameter
	protected static void ResetStep(GenerationProgress progress, GameConfiguration configuration)
#pragma warning restore IDE0060 // Remove unused parameter
	{
		WorldGenerator.CurrentGenerationProgress = progress;
		WorldGen._lastSeed = DateTime.Now.Second;
		WorldGen._genRand = new UnifiedRandom(DateTime.Now.Second);
		WorldGen._genRand.SetSeed(DateTime.Now.Second);
	}

	public override void DrawMenu(GameTime gameTime)
	{
		string statusText = Main.statusText;
		GenerationProgress progress = WorldGenerator.CurrentGenerationProgress;

		if (WorldGen.gen && progress is not null)
		{
			DrawStringCentered(progress.Message, Color.LightGray, new Vector2(0, 60), 0.6f);
			double percentage = progress.Value / progress.CurrentPassWeight * 100f;
			DrawStringCentered($"{percentage:#0.##}%", Color.LightGray, new Vector2(0, 120), 0.7f);
		}

		DrawStringCentered(statusText, Color.White);
	}

	private static void DrawStringCentered(string statusText, Color color, Vector2 position = default, float scale = 1f)
	{
		Vector2 screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f + position;
		Vector2 halfSize = FontAssets.DeathText.Value.MeasureString(statusText) / 2f * scale;
		Main.spriteBatch.DrawString(FontAssets.DeathText.Value, statusText, screenCenter - halfSize, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
	}
}
