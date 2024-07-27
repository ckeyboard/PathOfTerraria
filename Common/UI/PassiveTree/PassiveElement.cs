﻿using PathOfTerraria.Common.Loaders.UILoading;
using PathOfTerraria.Common.Systems.TreeSystem;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace PathOfTerraria.Common.UI.PassiveTree;

internal class PassiveElement : SmartUIElement
{
	private readonly Passive _passive;

	private int _flashTimer;
	private int _redFlashTimer;

	private TreeState UiTreeState => UILoader.GetUIState<TreeState>();
	
	public PassiveElement(Passive passive)
	{
		float halfSizeX = passive.Size.X / 2;
		float halfSizeY = passive.Size.Y / 2;

		if (passive.TreePos.X - halfSizeX < UiTreeState.TopLeftTree.X)
		{
			UiTreeState.TopLeftTree.X = passive.TreePos.X - halfSizeX;
		}
		
		if (passive.TreePos.Y - halfSizeY < UiTreeState.TopLeftTree.Y)
		{
			UiTreeState.TopLeftTree.Y = passive.TreePos.Y - halfSizeY;
		}

		if (passive.TreePos.X + halfSizeX > UiTreeState.BotRightTree.X)
		{
			UiTreeState.BotRightTree.X = passive.TreePos.X + halfSizeX;
		}
		
		if (passive.TreePos.Y + halfSizeY > UiTreeState.BotRightTree.Y)
		{
			UiTreeState.BotRightTree.Y = passive.TreePos.Y + halfSizeY;
		}

		_passive = passive;
		Left.Set(passive.TreePos.X - halfSizeX, 0.5f);
		Top.Set(passive.TreePos.Y - halfSizeY, 0.5f);
		Width.Set(passive.Size.X, 0);
		Height.Set(passive.Size.Y, 0);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		_passive.Draw(spriteBatch, GetDimensions().Center());
		DrawOnto(spriteBatch, GetDimensions().Center());

		if (_flashTimer > 0)
		{
			Texture2D glow = ModContent.Request<Texture2D>($"{nameof(PathOfTerraria)}/Assets/UI/GlowAlpha").Value;
			Texture2D star = ModContent.Request<Texture2D>($"{nameof(PathOfTerraria)}/Assets/UI/StarAlpha").Value;

			float prog = _flashTimer / 20f;

			var glowColor = new Color(255, 230, 150)
			{
				A = 0
			};

			glowColor *= prog * 0.5f;

			spriteBatch.Draw(glow, GetDimensions().Center(), null, glowColor, 0, glow.Size() / 2f, 1 + (1f - prog), 0, 0);
			spriteBatch.Draw(star, GetDimensions().Center(), null, glowColor * 0.5f, 0, star.Size() / 2f, 1 + (1f - prog), 0, 0);

			_flashTimer--;
		}

		if (_redFlashTimer > 0)
		{
			Texture2D glow = ModContent.Request<Texture2D>($"{nameof(PathOfTerraria)}/Assets/UI/GlowAlpha").Value;
			Texture2D star = ModContent.Request<Texture2D>($"{nameof(PathOfTerraria)}/Assets/UI/StarAlpha").Value;

			float prog = _redFlashTimer / 20f;

			var glowColor = new Color(255, 60, 60)
			{
				A = 0
			};

			glowColor *= prog * 0.5f;

			spriteBatch.Draw(glow, GetDimensions().Center(), null, glowColor, 0, glow.Size() / 2f, 1 + (1f - prog), 0, 0);
			spriteBatch.Draw(star, GetDimensions().Center(), null, glowColor * 0.5f, 0, star.Size() / 2f, 1 + prog, 0, 0);

			_redFlashTimer--;
		}

		if (IsMouseHovering)
		{
			string name = _passive.DisplayName;

			if (_passive.MaxLevel > 1)
			{
				name += $" ({_passive.Level}/{_passive.MaxLevel})";
			}

			Tooltip.SetName(name);
			Tooltip.SetTooltip(_passive.DisplayTooltip);
		}

		Recalculate();
	}

	public override void SafeClick(UIMouseEvent evt)
	{
		if (!_passive.CanAllocate(Main.LocalPlayer) || !CheckMouseContained())
		{
			return;
		}

		_passive.Level++;
		Main.LocalPlayer.GetModPlayer<TreePlayer>().Points--;

		_flashTimer = 20;

		switch (_passive.MaxLevel)
		{
			case 1:
				switch (_passive.Level)
				{
					case 1: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
					default: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
				}

				return;
			case 2:
				switch (_passive.Level)
				{
					case 1: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier2")); break;
					case 2: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
					default: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
				}

				return;
			case 3:
				switch (_passive.Level)
				{
					case 1: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier1")); break;
					case 2: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier3")); break;
					case 3: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
					default: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
				}

				return;
			case 5:
				switch (_passive.Level)
				{
					case 1: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier1")); break;
					case 2: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier2")); break;
					case 3: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier3")); break;
					case 4: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier4")); break;
					case 5: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
					default: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
				}

				return;
			case 6:
				switch (_passive.Level)
				{
					case 1: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier1")); break;
					case 2: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier2")); break;
					case 3: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier3")); break;
					case 4: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier4")); break;
					case 5: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
					case 6: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier1")); break;
					default: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
				}

				return;
			case 7:
				switch (_passive.Level)
				{
					case 1: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier1")); break;
					case 2: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier2")); break;
					case 3: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier3")); break;
					case 4: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier4")); break;
					case 5: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
					case 6: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier2")); break;
					case 7: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier1")); break;
					default: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
				}

				return;
			default:
				switch (_passive.Level)
				{
					case 1: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier1")); break;
					case 2: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier2")); break;
					case 3: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier3")); break;
					case 4: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier4")); break;
					case 5: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
					default: SoundEngine.PlaySound(new SoundStyle($"{nameof(PathOfTerraria)}/Sounds/Tier5")); break;
				}

				break;
		}
	}

	public override void SafeRightClick(UIMouseEvent evt)
	{
		if (!_passive.CanDeallocate(Main.LocalPlayer) || !CheckMouseContained())
		{
			return;
		}

		_passive.Level--;
		Main.LocalPlayer.GetModPlayer<TreePlayer>().Points++;

		_redFlashTimer = 20;

		SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath);
	}
}