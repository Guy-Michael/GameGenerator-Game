public enum Player
{
    Astronaut,
    Alien
}

public static class PlayerEnumExtensions
{
    public static SetOutcome ToOutcome(this Player player)
    {
        return player == Player.Astronaut ? SetOutcome.AstronautWin : SetOutcome.AlienWin;
    }

    public static Player Other(this Player player)
    {
        return player == Player.Alien ? Player.Astronaut : Player.Alien;
    }
}