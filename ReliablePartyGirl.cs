using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ReliablePartyGirl
{
	public class ReliablePartyGirl : Mod
	{
	}

	public class PartyWorld : ModWorld
	{
		public bool SpawnPartyGirlReady = false;

		public override void PreUpdate()
		{
			// At night, reset PartyGirl spawn cooldown. Genuine parties can never occur at night, so its a good spot to reset
			// A cooldown is desired as multiple party girls should not spawn at the same party (ei. if a party girl dies)
			if (!Main.dayTime) {
				SpawnPartyGirlReady = true;
			}
			
			// If a genuine party is occuring and we can spawn a new party girl, find an existing Party Girl if off cooldown
			// GenuineParty assumes PartyIsUp is true
			if (BirthdayParty.GenuineParty && SpawnPartyGirlReady && NPC.FindFirstNPC(NPCID.PartyGirl) < 0) {
				// Spawn a smoke cloud
				for (int j = 0; j < 4; j++) {
					Utils.PoofOfSmoke(new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16));
				}

				// Create a new Party Girl NPC
				int partyGirl = NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, NPCID.PartyGirl);

				// Broadcast that a Party Girl has arrived
				string npcName = Main.npc[partyGirl].GivenName;
				string npcTitle = Main.npc[partyGirl].TypeName;
				Color pink = new Color(255, 110, 160);
				WorldGen.BroadcastText(NetworkText.FromLiteral($"{npcName} the {npcTitle} has arrived to the party!"), pink);

				// Put the Party Girl spawn on cooldown
				// Will only ever turn back on when night occurs
				SpawnPartyGirlReady = false;
			}
		}
	}
}