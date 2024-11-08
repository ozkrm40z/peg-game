using CrackerBarrelPegGame.Logic.Model;

namespace CrackerBarrelPegGame.Helpers;
public static class GameHelper
{
	public static string ConvertToString(this Hole hole) => hole.ContainsPeg ? "1" : "0";
}
