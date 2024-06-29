﻿using PathOfTerraria.Core.Systems.Questing.Quests.TestQuest;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;

namespace PathOfTerraria.Core.Systems.Questing;
internal class QuestModPlayer : ModPlayer
{
	// need a list of what npcs start what quests
	private readonly Dictionary<string, Quest> _enabledQuests = [];

	public void RestartQuestTest()
	{
		_enabledQuests.Clear();
		Quest quest = new TestQuest();
		Quest quest2 = new TestQuestTwo();

		quest.StartQuest(Player);
		quest2.StartQuest(Player);

		_enabledQuests.Add(quest.Name, quest);
		_enabledQuests.Add(quest2.Name, quest2);
	}

	public string GetQuestSteps(string name)
	{
		return _enabledQuests[name].AllQuestStrings();
	}

	public string GetQuestStep(string name)
	{
		return _enabledQuests[name].CurrentQuestString();
	}

	public override void SaveData(TagCompound tag)
	{
		List<TagCompound> questTags = [];
		foreach (KeyValuePair<string, Quest> quest in _enabledQuests)
		{
			var newTag = new TagCompound();
			quest.Value.Save(newTag);
			questTags.Add(newTag);
		}

		tag.Add("questTags", questTags);
	}
	
	public override void LoadData(TagCompound tag)
	{
		List<TagCompound> questTags = tag.Get<List<TagCompound>>("questTags");

		questTags.ForEach(tag => { Quest q = Quest.LoadFrom(tag, Player); if (q is not null) { _enabledQuests.Add(q.Name, q); } });
	}

	public List<string> GetQuests()
	{
		return _enabledQuests.ToList().Select(q => q.Key).ToList();
	}

	public int GetQuestCount()
	{
		return _enabledQuests.Count;
	}

	public List<Quest> GetAllQuests()
	{
		return null;
		// return _enabledQuests;
	}
	
	public List<Quest> GetCompletedQuests()
	{
		return null;
		// return _enabledQuests.ToList().Select().FindAll(q => q.Completed);
	}
	
	public List<Quest> GetIncompleteQuests()
	{
		return null;
		// return _enabledQuests.ToList().FindAll(q => !q.Completed);
	}
}
