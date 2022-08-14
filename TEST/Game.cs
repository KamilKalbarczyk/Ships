using System;
using System.Collections.Generic;
using System.Linq;

namespace TEST
{
    class Game
    {
        const int BoardSize = 10;
        private readonly Field[,] _board = new Field[BoardSize, BoardSize];
        private readonly Dictionary<string,int> Coordinates = new Dictionary<string, int>()
        {
            ["A"] = 0,
            ["B"] = 1,
            ["C"] = 2,
            ["D"] = 3,
            ["E"] = 4,
            ["F"] = 5,
            ["G"] = 6,
            ["H"] = 7,
            ["I"] = 8,
            ["J"] = 9
        };

        public Game()
        {
            for (int i = 0; i < BoardSize; i++)
                for (int j = 0; j < BoardSize; j++)
                {
                    _board[i, j] = new Field();
                }
        }

        public void DisplayBoard()
        {
            Console.Clear();
            DisplayHeader();
            DisplayShips();
            DisplayHelp();
        }

        private void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("O - unknown / you've never shot here");
            Console.WriteLine("! - sink ship");
            Console.WriteLine("X - miss shot");
        }

        public void SetShips(int[] ships)
        {
            foreach (var ship in ships) CreateShip(ship);
        }

        public void Play()
        {
            bool playerWon;

            var allowedLetters = Coordinates
                    .Select(x => x.Key)
                    .ToArray();
            var allowedNumbers = Coordinates
                .Select(x => x.Value + 1)
                .ToArray();

            do
            {
                DisplayBoard();

                playerWon = true;

                for (int i = 0; i < BoardSize - 1; i++)
                    for (int j = 0; j < BoardSize - 1; j++)
                    {
                        if (_board[i, j].IsShip)
                        {
                            playerWon = false;
                            break;
                        }
                    }

                if (playerWon) continue;

                bool targetIsCorrect;

                do
                {
                    TryShot(allowedLetters, allowedNumbers, out targetIsCorrect);
                }
                while (!targetIsCorrect);
            }
            while (!playerWon);

            Console.WriteLine("WIN!");
            Console.ReadKey();
        }

        public void TryShot(string[] allowedLetters, int[] allowedNumbers, out bool targetIsCorrect)
        {
            targetIsCorrect = false;

            Console.Write("Type coordinates (LetterNumber, i.e A1): ");
            var target = Console.ReadLine();

            if (target.Length >= 2)
            {
                var letter = target[0].ToString().ToUpper();
                int.TryParse(target.Substring(1, target.Length - 1), out var number);

                if (allowedLetters.Contains(letter) && number >= 1 && number <= 10)
                {
                    var y = number - 1;
                    var x = Coordinates[letter];

                    _board[y, x].Shot();

                    targetIsCorrect = true;
                }
            }

            if (!targetIsCorrect)
            {
                Console.WriteLine("Incorrect coordinates");
            }
        }

        private void CreateShip(int size)
        {
            bool shipIsPlaced;
            bool shipCannotBePlaced;

            do
            {
                shipIsPlaced = false;
                shipCannotBePlaced = false;

                var index = new Random().Next(0, BoardSize - 1);
                var height = new Random().Next(0, BoardSize - 1);

                var start = index + size < BoardSize
                    ? index
                    : index - size;

                for (int i = 0; i < size; i++)
                {
                    if (_board[height, start + 1].IsNotAvailable)
                    {
                        shipCannotBePlaced = true;
                    }
                }

                if (!shipCannotBePlaced)
                {
                    var isRowAbove = height > 0;
                    var isRowBottom = height < 9;
                    var isLeftColumn = start >= 1;
                    var isRightColumn = start < 9;

                    for (int i = 0; i < size; i++)
                    {
                        _board[height, start + i].Ship();

                        if (isRowAbove)
                        {
                            _board[height - 1, start + i].NotAvailable();
                        }

                        if (isRowBottom)
                        {
                            _board[height + 1, start + i].NotAvailable();
                        }
                    }

                    if (isRowAbove && isLeftColumn)
                    {
                        _board[height - 1, start - 1].NotAvailable();
                    }

                    if (isRowAbove && isRightColumn)
                    {
                        _board[height - 1, start + 1].NotAvailable();
                    }

                    if (isRowBottom && isLeftColumn)
                    {
                        _board[height + 1, start - 1].NotAvailable();
                    }

                    if (isRowBottom && isRightColumn)
                    {
                        _board[height + 1, start + 1].NotAvailable();
                    }

                    if (isRowAbove)
                    {
                        _board[height + 1, start + 1].NotAvailable();
                    }

                    if (isLeftColumn)
                    {
                        _board[height, start - 1].NotAvailable();
                    }

                    if (isRightColumn)
                    {
                        _board[height, start + size].NotAvailable();
                    }

                    shipIsPlaced = true;
                }
            }
            while (!shipIsPlaced);

            Console.WriteLine();
            DisplayBoard();
        }

        private void DisplayHeader()
        {
            Console.Write("\t");

            for (int i = 0; i < BoardSize; i++)
            {
                var letter = Coordinates
                    .First(x => x.Value == i)
                    .Key;

                //Console.Write("");
                Console.Write(letter);
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private void DisplayShips()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                Console.Write((i + 1).ToString());
                Console.Write("\t");

                for (int j = 0; j < BoardSize; j++)
                {
                    _board[i, j].Display();
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}