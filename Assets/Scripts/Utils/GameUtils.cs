using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;

public static class GameUtils
{
    static LineRenderer line;

    public static LineRenderer DrawLineRendererOnWinningTriplet(BoardElementManager gameBoard, (int a, int b, int c) winningTriplet)
    {
        line = gameBoard.gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;

        Vector3 start = gameBoard[winningTriplet.a].transform.position;
        start.z = 1;

        Vector3 end = gameBoard[winningTriplet.c].transform.position;
        end.z = 1;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        return line;
    }

    public static void DestroyLineRenderer()
    {
        if(line != null)
        {
            GameObject.Destroy(line);
            line = null;
        }
    }

    public static (int, int, int) GetWinningTriplet(List<int> movesMadeInCurrentRound)
    {
        foreach((int a, int b, int c) triplet in GameUtils.GetWinningTriplets())
        {
            if(movesMadeInCurrentRound.Contains(triplet.a) &&
                movesMadeInCurrentRound.Contains(triplet.b) &&
                movesMadeInCurrentRound.Contains(triplet.c))
            {
                return triplet;
            }
        }

        return (-1, -1 , -1);
    }

    public static bool HasWonSet(List<int> currentPlayerMoves)
    {
        foreach((int a, int b, int c) triplet in GameUtils.GetWinningTriplets())
        {
            if(currentPlayerMoves.Contains(triplet.a) &&
                currentPlayerMoves.Contains(triplet.b) &&
                currentPlayerMoves.Contains(triplet.c))
                {
                    return true;
                }
        }

        return false;
    }

    public static bool IsSetTied(Dictionary<Player, List<int>> movesMadeInSet)
    {
        int astronautMoves = movesMadeInSet[Player.Astronaut].Count();
        int alienMoves = movesMadeInSet[Player.Alien].Count();
        return astronautMoves + alienMoves >= 9;
    }
    public static (int, int, int)[] GetWinningTriplets()
    {
        (int a, int b, int c)[] winningTriplets = new (int, int, int)[]
        {
            (0, 1, 2), //Top Row
            (3, 4, 5), // Middle Row
            (6, 7, 8), // Bottom Row
            (0, 3, 6), // Left Column
            (1, 4, 7), // Middle Column
            (2, 5, 8), // Right Column
            (0, 4, 8), // Main Diagonal
            (2, 4, 6) // Sub Diagonal
        };

        return winningTriplets;
    }

    public static bool AssertHebewText(string text)
    {
        foreach(char c in text)
        {
            if(c >= 'א' && c <= 'ת')
            {
                return true;
            }
        }

        return false;
    }

    public static void FlipTextComponentIfHebrew(TextMeshProUGUI text)
    {
        if(AssertHebewText(text.text))
        {
            text.isRightToLeftText = true;
        }
    }
}