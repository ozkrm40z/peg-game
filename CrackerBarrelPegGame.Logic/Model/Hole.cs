namespace CrackerBarrelPegGame.Logic.Model;
public class Hole(int position, HashSet<int> neighborHoles, HashSet<int> validMoves, bool containsPeg)
{
	public int Position { get; } = position;
	public HashSet<int> NeighborHoles { get; } = neighborHoles;

	public HashSet<int> ValidMoves { get; } = validMoves;

	public bool ContainsPeg { get; internal set; } = containsPeg;

	public void SetPeg()
	{
		ContainsPeg = true;
	}

	public void RemovePeg()
	{
		ContainsPeg = false;
	}
}
