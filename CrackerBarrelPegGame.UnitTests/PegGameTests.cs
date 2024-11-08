using CrackerBarrelPegGame.Logic;
using CrackerBarrelPegGame.Logic.Model;

namespace CrackerBarrelPegGame.UnitTests;

public class Tests
{
	[Test, TestCaseSource(nameof(GameMovementSet))]
	public void GivenASetOfMovementsItShouldReturnTheExpectedState(GameTestCaseData testCase)
	{
		var game = new PegGame(testCase.InitialPosition);

		foreach (var (initialPosition, finalPosition) in testCase.Moves)
		{
			var result = game.Move(initialPosition, finalPosition);

			Assert.That(result.Succeeded, Is.True);
		}

		Assert.That(game.State, Is.EqualTo(testCase.ExpectedState));
	}

	[Test, TestCaseSource(nameof(InvalidMovesCases))]
	public void GivenASetOfInvalidMovesItShouldReturnTheExpectedError(GameTestCaseData testCase)
	{
		var game = new PegGame(1);

		for (var j =0; j < testCase.Moves.Count -1; j++)
		{
			game.Move(testCase.Moves[j].InitialPosition, testCase.Moves[j].FinalPosition);
		}

		var (initialPosition, finalPosition) = testCase.Moves.Last();
		var result = game.Move(initialPosition, finalPosition);
		
		Assert.That(result.Succeeded, Is.False);
		Assert.That(result.ErrorDescription, Is.EqualTo(testCase.ExpectedErrorDescription));
		Assert.That(game.State, Is.EqualTo(testCase.ExpectedState));
	}

	private static IReadOnlyList<TestCaseData> GameMovementSet =>
	[
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.Victory,
				Moves =
				[
					(4, 1),
					(6, 4),
					(1, 6),
					(7, 2),
					(13, 4),
					(2, 7),
					(10, 8),
					(7, 9),
					(15, 13),
					(12, 14),
					(6, 13),
					(14, 12),
					(11, 13)
				]
			}).SetName("Solution for initial hole at position 1"),
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.Victory,
				Moves =
				[
					(6, 1),
					(4, 6),
					(1, 4),
					(7, 2),
					(10, 3),
					(13, 4),
					(4, 1),
					(1, 6),
					(11, 13),
					(14, 12),
					(6, 13),
					(12, 14),
					(15, 13),
				]
			}).SetName("Alternative solution for initial hole at position 1"),
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.GameOver,
				Moves =
				[
					(6, 1),
					(15, 6),
					(13, 15),
					(5, 14),
					(4, 13),
					(1, 4),
					(7, 2),
				]
			}).SetName("Game over case")
	];

	private static IReadOnlyList<TestCaseData> InvalidMovesCases =>
	[
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.InProgress,
				Moves = [(4, 5)],
				ExpectedErrorDescription = "Peg can't be moved to the desired location"
			}),
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.InProgress,
				Moves = [(0, 3)],
				ExpectedErrorDescription = "Invalid position movement"
			}),
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.InProgress,
				Moves = [(16, 1)],
				ExpectedErrorDescription = "Invalid position movement"
			}),
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.InProgress,
				Moves = [(1, 4)],
				ExpectedErrorDescription = "Initial position is empty"
			}),
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.InProgress,
				Moves = [(4, 6)],
				ExpectedErrorDescription = "Position to move is not empty"
			}),
		new TestCaseData(
			new GameTestCaseData
			{
				InitialPosition = 1,
				ExpectedState = Enums.GameState.InProgress,
				Moves = [
					(4, 1),
					(1, 4)],
				ExpectedErrorDescription = "Can't jump over an empty hole"
			}),
	];

	public class GameTestCaseData
	{
		public int InitialPosition { get; set; }
		public List<(int InitialPosition, int FinalPosition)> Moves { get; set; }
		public string? ExpectedErrorDescription { get; set; }

		public Enums.GameState ExpectedState { get; set; }
	}
}