using System;
using UnityEngine.Assertions;
using Utilities;
using Random = UnityEngine.Random;

namespace Models
{
    public class BoardFactory
    {
        private readonly BoardConfig boardConfig = ServiceLocator.Instance.Resolve<BoardConfig>();

        public BoardModel Get()
        {
            Random.InitState(Environment.TickCount);

            BoardModel board = new(boardConfig.Width, boardConfig.Height);

            SetFields(board);

            return board;
        }

        private void SetFields(BoardModel board)
        {
            Assert.IsTrue(boardConfig.blockedChance < 1f);

            while (true)
            {
                for (var x = 0; x < boardConfig.Width; x++)
                {
                    for (var y = 0; y < boardConfig.Height; y++)
                    {
                        FieldType type = Random.value <= boardConfig.blockedChance ? FieldType.Blocked : FieldType.Open;

                        board.SetField(new BoardPosition(x, y), type);
                    }
                }

                if (board.CountFields(FieldType.Blocked) == board.Width * board.Height)
                    continue;

                break;
            }
        }
    }
}
