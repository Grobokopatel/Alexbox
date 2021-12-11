using System.Collections.Generic;
using System.Linq;

namespace Alexbox.Domain
{
    public class Game : Entity

    {
    private string Instruction;
    public List<Player> _players { get; }
    private GameStatus _gameStatus = GameStatus.WaitingForPlayers;

    public Game(List<Player> players)
    {
        _players = players;
    }

    public Player GetBestPlayer()
    {
        var bestPlayer = _players.First();
        foreach (var player in _players)
        {
            if (bestPlayer.Score < player.Score)
            {
                bestPlayer = player;
            }
        }

        return bestPlayer;
    }

    public void Start()
    {
        _gameStatus = GameStatus.Playing;
    }
    }
}