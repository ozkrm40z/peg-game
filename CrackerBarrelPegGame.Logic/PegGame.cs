using CrackerBarrelPegGame.Logic.Model;

namespace CrackerBarrelPegGame.Logic;

public class PegGame
{
	public PegGame(int initialPosition)
	{
		Holes = [];
		for (var i = 1; i <= 15; i++)
		{
			Holes.Add(i, new Hole(i, _neighborHoles[i], _validMoves[i], containsPeg: i != initialPosition ));
		}

		IsAbandoned = false;
		MovementHistory = [$"Initial position: {initialPosition}, Remaining Pegs: {GetRemainingHolesWithPegs().Count}"];
	}

	public Enums.GameState State {
		get
		{
			if (GetAvailableMovementsCount() == 0 && GetRemainingHolesWithPegs().Count > 1)
				return Enums.GameState.GameOver;

			if (GetAvailableMovementsCount() == 0 && GetRemainingHolesWithPegs().Count == 1)
				return Enums.GameState.Victory;

			if (IsAbandoned)
				return Enums.GameState.Abandoned;

			return Enums.GameState.InProgress;
		}
	}

	private bool IsAbandoned { get; set; }

	public MovementResult Move(int initialPosition, int finalPosition)
	{
		if (State == Enums.GameState.Victory)
			return new MovementResult(false, "Victory");

		if (State == Enums.GameState.GameOver)
			return new MovementResult(false, "Game over, no more movements available");

		if (initialPosition is < 1 or > 15)
			return new MovementResult(false, "Invalid position movement");

		var initialPositionHole = Holes[initialPosition];
		var finalPositionHole = Holes[finalPosition];

		var result = IsAValidMove(initialPositionHole, finalPositionHole);

		if (result.Succeeded)
		{

			var commonNeighbor = GetCommonNeighborHole(initialPositionHole, finalPositionHole);

			initialPositionHole.RemovePeg();
			commonNeighbor.RemovePeg();
			finalPositionHole.SetPeg();
		}

		MovementHistory.Add($"Move: {initialPosition}-{finalPosition}, Remaining Pegs: {GetRemainingHolesWithPegs().Count}, Message: {(result.Succeeded ? "Succeeded": result.ErrorDescription)}");

		return result;
	}

	public void Abandon()
	{
		IsAbandoned = true;
	}

	public IReadOnlyList<string> GetMovementsHistory()
	{
		return MovementHistory.AsReadOnly();
	}

	private int GetAvailableMovementsCount()
	{
		var remainingMovements = 0;

		var holesWithPegs = GetRemainingHolesWithPegs();

		foreach (var hole in holesWithPegs)
		{
			foreach (var validMovePosition in hole.ValidMoves)
			{
				var finalPosition = Holes[validMovePosition];
				if (IsAValidMove(hole, finalPosition).Succeeded) 
					remainingMovements++;
			}
		}

		return remainingMovements;
	}

	private List<Hole> GetRemainingHolesWithPegs()
	{
		return Holes
			.Where(h => h.Value.ContainsPeg)
			.Select(h => h.Value)
			.ToList();
	}

	private MovementResult IsAValidMove(Hole initialPosition, Hole finalPosition)
	{
		if (!initialPosition.ContainsPeg)
			return new MovementResult(false, "Initial position is empty");

		if (!initialPosition.ValidMoves.Contains(finalPosition.Position))
			return new MovementResult(false, "Peg can't be moved to the desired location");

		if (finalPosition.ContainsPeg)
			return new MovementResult(false, "Position to move is not empty");

		return !GetCommonNeighborHole(initialPosition, finalPosition).ContainsPeg
			? new MovementResult(false, "Can't jump over an empty hole")
			: new MovementResult(true);
	}

	private Hole GetCommonNeighborHole(Hole initialPosition, Hole finalPosition)
	{
		var commonNeighborPosition = initialPosition.NeighborHoles.Intersect(finalPosition.NeighborHoles).Single();
		return Holes[commonNeighborPosition];
	}

	public Dictionary<int, Hole> Holes { get; set; }

	public List<string> MovementHistory { get; set; }

	private readonly Dictionary<int, HashSet<int>> _validMoves = new()
	{
		{ 1, [4, 6] },
		{ 2, [7, 9] },
		{ 3, [8, 10] },
		{ 4, [1, 11, 6, 13] },
		{ 5, [12, 14] },
		{ 6, [1, 4, 13, 15] },
		{ 7, [2, 9] },
		{ 8, [3, 10] },
		{ 9, [2, 7] },
		{ 10, [3, 8] },
		{ 11, [4, 13]},
		{ 12, [5, 14]},
		{ 13, [4, 6, 11, 15]},
		{ 14, [5, 12]},
		{ 15, [6, 13]}
	};

	private readonly Dictionary<int, HashSet<int>> _neighborHoles = new()
	{
		{ 1, [2, 3] },
		{ 2, [1, 3, 5, 4] },
		{ 3, [1, 2, 6, 5] },
		{ 4, [2, 5, 7, 8] },
		{ 5, [2, 3, 4, 6, 8, 9] },
		{ 6, [3,5,9,10] },
		{ 7, [4,8,12,11] },
		{ 8, [7,4,5,9,13,12] },
		{ 9, [5,6,8,10,14,13] },
		{ 10, [6,9,14,15] },
		{ 11, [7,12]},
		{ 12, [7,8,11,13]},
		{ 13, [8,9,12,14]},
		{ 14, [9,10,13,15]},
		{ 15, [10,14]}
	};
}
