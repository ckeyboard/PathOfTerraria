using PathOfTerraria.Common.Waypoints.UI;
using PathOfTerraria.Content.Items.Placeable;
using PathOfTerraria.Core.UI;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ObjectData;

namespace PathOfTerraria.Content.Tiles;

public class ArcaneObeliskTile : ModTile
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();

		Main.tileLavaDeath[Type] = false;
		Main.tileWaterDeath[Type] = false;
		Main.tileBlockLight[Type] = true;
		Main.tileFrameImportant[Type] = true;

		TileID.Sets.HasOutlines[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);

		TileObjectData.newTile.DrawYOffset = 4;

		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };

		TileObjectData.newTile.Origin = Point16.Zero;

		TileObjectData.addTile(Type);

		DustType = DustID.Stone;
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(142, 136, 174), CreateMapEntryName());
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		base.NumDust(i, j, fail, ref num);

		num = fail ? 1 : 3;
	}

	public override void MouseOver(int i, int j)
	{
		base.MouseOver(i, j);

		Main.LocalPlayer.cursorItemIconEnabled = true;
		Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<ArcaneObeliskItem>();
	}

	public override bool RightClick(int i, int j)
	{
		UIManager.TryToggleOrRegister(UIWaypointMenu.Identifier, "Vanilla: Mouse Text", new UIWaypointMenu(), 1);

		return true;
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
	{
		return true;
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		base.PostDraw(i, j, spriteBatch);

		Tile tile = Framing.GetTileSafely(i, j);

		Rectangle frame = new(tile.TileFrameX, tile.TileFrameY, 18, 18);
		TileObjectData data = TileObjectData.GetTileData(tile);

		Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

		Vector2 dataOffset = new(data.DrawXOffset, data.DrawYOffset);
		Vector2 screenOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

		Vector2 position = new Vector2(i, j) * 16f - Main.screenPosition + screenOffset + dataOffset;

		spriteBatch.Draw(texture, position, frame, Color.White);
	}
}