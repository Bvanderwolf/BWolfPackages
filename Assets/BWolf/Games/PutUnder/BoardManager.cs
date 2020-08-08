using BWolf.Utilities.SingletonBehaviour;
using UnityEngine;

namespace BWolf.Games.PutUnder
{
    public class BoardManager : SingletonBehaviour<BoardManager>
    {
        [Header("Settings")]
        [SerializeField]
        private float[] timestamps = null;

        [SerializeField]
        private float wait = 5f;

        [Header("References")]
        [SerializeField]
        private Pawn playerOne = null;

        [SerializeField]
        private Pawn playerTwo = null;

        private const int collums = 3;
        private const int size = collums * collums;

        private Board board;

        public float[] TimeStamps
        {
            get { return timestamps; }
        }

        public float Wait
        {
            get { return wait; }
        }

        private void Start()
        {
            board = new Board(transform);

            playerOne.PlaceAsPlayer(1, board.GetHole(board.Min).position, board.Min);
            playerTwo.PlaceAsPlayer(2, board.GetHole(board.Max).position, board.Max);
        }

        private void Update()
        {
            if (!playerOne.IsBusy)
            {
                foreach (Move move in Move.PlayerOne)
                {
                    if (Input.GetKeyDown(move.Key))
                    {
                        playerOne.DoMove(board, move);
                    }
                }
            }

            if (!playerTwo.IsBusy)
            {
                foreach (Move move in Move.PlayerTwo)
                {
                    if (Input.GetKeyDown(move.Key))
                    {
                        playerTwo.DoMove(board, move);
                    }
                }
            }
        }

        public readonly struct Move
        {
            public readonly KeyCode Key;
            public readonly Vector2Int Shift;
            public readonly string Scroll;

            public Move(KeyCode key, Vector2Int shift, string scroll = null)
            {
                Key = key;
                Shift = shift;
                Scroll = scroll;
            }

            public static readonly Move[] PlayerOne = new Move[] {
            new Move(KeyCode.A, new Vector2Int(-1, 0), "left"),
            new Move(KeyCode.W, new Vector2Int(0, -1), "up"),
            new Move(KeyCode.S, new Vector2Int(0, 1)),
            new Move(KeyCode.D, new Vector2Int(1, 0), "right")
            };

            public static readonly Move[] PlayerTwo = new Move[] {
            new Move(KeyCode.LeftArrow, new Vector2Int(-1, 0), "left"),
            new Move(KeyCode.UpArrow, new Vector2Int(0, -1), "up"),
            new Move(KeyCode.DownArrow, new Vector2Int(0, 1)),
            new Move(KeyCode.RightArrow, new Vector2Int(1, 0), "right")
            };
        }

        public readonly struct Board
        {
            public readonly Vector2Int Min;
            public readonly Vector2Int Max;

            private readonly Transform[,] grid;

            public Board(Transform root)
            {
                grid = new Transform[collums, collums];
                for (int y = 0, i = 0; y < collums; y++)
                {
                    for (int x = 0; x < collums; x++, i++)
                    {
                        grid[x, y] = root.GetChild(i);
                    }
                }

                Min = new Vector2Int(0, 0);
                Max = new Vector2Int(collums - 1, collums - 1);
            }

            public bool TryGetShift(Pawn pawn, Vector2Int shift, out Vector2Int newPos)
            {
                newPos = new Vector2Int(pawn.GridPosition.x + shift.x, pawn.GridPosition.y + shift.y);
                return newPos.x >= 0 && newPos.x < collums && newPos.y >= 0 && newPos.y < collums;
            }

            public Transform GetHole(Vector2Int pos)
            {
                return grid[pos.x, pos.y];
            }
        }
    }
}