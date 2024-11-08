using CrackerBarrelPegGame.Helpers;
using CrackerBarrelPegGame.Logic;
using CrackerBarrelPegGame.Logic.Model;

Play();

return;

void Play()
{
	string? continuePlaying;
	do
	{
		Console.Clear();

		int initialPosition;
		var isValidInitialPosition = false;
		do
		{
			var initialPositionText = GetCommand("Set the initial position. Range: 1-15");
			if (int.TryParse(initialPositionText, out initialPosition) && initialPosition is >= 1 and <= 15)
				isValidInitialPosition = true;
			else
				Console.WriteLine("Invalid initial position, input value should be between 1 and 15");

		} while (!isValidInitialPosition);

		var game = new PegGame(initialPosition);
		
		do
		{
			PrintBoard(game);
			
			var command = GetCommand("type 'e' to abandon or type your move, example: 4,1");
			switch (command)
			{
				case "e":
					game.Abandon();
					break;
				default:
					try
					{
						var movePositions = command.Split(",");
						game.Move(int.Parse(movePositions[0]), int.Parse(movePositions[1]));
					}
					catch (Exception ex)
					{
						Console.WriteLine("Invalid input, try again.");
					}

					break;
			}
		} while (game.State == Enums.GameState.InProgress);

		Console.WriteLine($"Game result: {game.State}.");

		continuePlaying = GetCommand("Type 'c' to play again or any other key to exit");
		
	} while (continuePlaying.Equals("c"));
}

void PrintBoard(PegGame game)
{
	Console.Clear();
	Console.WriteLine($"""
	                    ##  PEG GAME ##
	                        
	                           {game.Holes[1].ConvertToString()}
	                          {game.Holes[2].ConvertToString()} {game.Holes[3].ConvertToString()}
	                         {game.Holes[4].ConvertToString()} {game.Holes[5].ConvertToString()} {game.Holes[6].ConvertToString()}
	                        {game.Holes[7].ConvertToString()} {game.Holes[8].ConvertToString()} {game.Holes[9].ConvertToString()} {game.Holes[10].ConvertToString()}
	                       {game.Holes[11].ConvertToString()} {game.Holes[12].ConvertToString()} {game.Holes[13].ConvertToString()} {game.Holes[14].ConvertToString()} {game.Holes[15].ConvertToString()}
	                       
	                   """);

	Console.WriteLine("History:");
	var index = 1;
	foreach (var move in game.GetMovementsHistory())
	{
		Console.WriteLine($"{index}.- {move}");
		index++;
	}
}

string GetCommand(string text)
{
	Console.WriteLine("");
	Console.WriteLine(text);
	return Console.ReadLine() ?? "";
}
