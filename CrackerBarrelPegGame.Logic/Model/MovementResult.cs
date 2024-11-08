namespace CrackerBarrelPegGame.Logic.Model;
public class MovementResult(bool succeeded, string? errorDescription = null)
{
	public bool Succeeded { get; } = succeeded;

	public string? ErrorDescription { get; } = errorDescription;
}
