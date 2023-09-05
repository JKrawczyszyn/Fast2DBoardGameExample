using System;
using Random = UnityEngine.Random;

namespace Model
{
    public class BoardFactory
    {
        private BoardConfig boardConfig;

        public BoardFactory()
        {
            Inject();
        }

        private void Inject()
        {
            boardConfig = DiManager.Instance.Resolve<BoardConfig>();
        }

        public BoardModel Get()
        {
            var board = new BoardModel(boardConfig.Width, boardConfig.Height);

            Random.InitState(Environment.TickCount);

            for (var x = 0; x < boardConfig.Width; x++)
            {
                for (var y = 0; y < boardConfig.Height; y++)
                {
                    FieldType type = Random.value <= boardConfig.blockedChance ? FieldType.Blocked : FieldType.Open;

                    board.SetField(x, y, type);
                }
            }

            return board;
        }
    }
}
