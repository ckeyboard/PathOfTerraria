﻿using PathOfTerraria.Core.Systems.Affixes;
using PathOfTerraria.Core.Systems.Questing.Quests.TestQuest;
using PathOfTerraria.Core.Systems.Questing.QuestStepTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader.IO;

namespace PathOfTerraria.Core.Systems.Questing;
internal class QuestHandler : ModPlayer
{
	// need a list of what npcs start what quests
	private readonly List<Quest> _enabledQuests = [];

	public void RestartQuestTest()
	{
		_enabledQuests.Clear();
		Quest quest = new TestQuest();

		quest.StartQuest(Player);

		_enabledQuests.Add(quest);
	}

	public override void PostUpdate()
	{
		// _enabledQuests.ForEach(q => Console.WriteLine(q.CurrentQuestString()));
	}

	public override void SaveData(TagCompound tag)
	{
		List<TagCompound> questTags = [];
		foreach (Quest quest in _enabledQuests)
		{
			var newTag = new TagCompound();
			quest.Save(newTag);
			questTags.Add(newTag);
		}

		tag.Add("questTags", questTags);
	}
	public override void LoadData(TagCompound tag)
	{
		List<TagCompound> questTags = tag.Get<List<TagCompound>>("questTags");

		questTags.ForEach(tag => { Quest q = Quest.LoadFrom(tag, Player); if (q is not null) { _enabledQuests.Add(q); } });
	}
}
