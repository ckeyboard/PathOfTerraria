using System.Collections.Generic;
using JetBrains.Annotations;
using PathOfTerraria.Common.NPCs.Components;
using Terraria.Localization;

// This may not work for vanilla NPCs, but I believe we will not need that anyways.
namespace PathOfTerraria.Common.NPCs.Dialogue;

/// <summary>
///		Provides registration and handles dialogue options of town NPCs upon interaction.
/// </summary>
[Autoload(Side = ModSide.Client)]
public sealed class NPCTownDialogue : NPCComponent
{
	public readonly struct DialogueEntry
	{
		/// <summary>
		///		The localization key of this dialogue entry.
		/// </summary>
		public readonly string Key;

		/// <summary>
		///		The predicate of this dialogue entry.
		/// </summary>
		public readonly Func<bool>? Predicate;

		public DialogueEntry(string key, Func<bool>? predicate = null)
		{
			Key = key;
			Predicate = predicate;
		}
	}

	/// <summary>
	///		The current dialogue index of this component.
	/// </summary>
	public int Dialogue
	{
		get => dialogue;
		set => dialogue = value >= Pool.Count ? 0 : value;
	}

	private int dialogue;

	/// <summary>
	///		The list of registered <see cref="DialogueEntry"/> instances in this component.
	/// </summary>
	public readonly List<DialogueEntry> Pool = [];

	/// <summary>
	///		Adds a new dialogue entry to the pool.
	/// </summary>
	/// <param name="entry"></param>
	public void AddDialogue(in DialogueEntry entry)
	{
		Pool.Add(entry);
	}

	public override void GetChat(NPC npc, ref string chat)
	{
		if (TryGetDialogue(out chat))
		{
			return;
		}

		throw new Exception();
	}
	
	private bool TryGetDialogue(out string dialogue)
	{
		dialogue = string.Empty;

		if (!HasAvailableDialogue())
		{
			return false;
		}

		DialogueEntry entry = Pool[Dialogue++];

		while (entry.Predicate?.Invoke() == false)
		{
			entry = Pool[++Dialogue];
		}

		dialogue = Language.GetTextValue(entry.Key);

		return true;
	}

	private bool HasAvailableDialogue()
	{
		bool success = false;
		
		for (int i = 0; i < Pool.Count; i++)
		{
			DialogueEntry entry = Pool[i];

			if (entry.Predicate == null || entry.Predicate?.Invoke() == true)
			{
				success = true;
				break;
			}
		}
		
		return success;
	}
}