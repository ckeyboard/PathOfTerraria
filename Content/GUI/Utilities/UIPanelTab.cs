﻿using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace PathOfTerraria.Content.GUI.Utilities;

public class UIPanelTab(string name, LocalizedText text, float textScale = 1, bool large = false)
	: UITextPanel<LocalizedText>(text, textScale, large)
{
	public readonly string Name = name;
}