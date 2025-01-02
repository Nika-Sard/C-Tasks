namespace GameOfLife
{
    /// <summary>
    /// Represents Conway's Game of Life in a parallel version.
    /// The class provides methods to simulate the game's evolution based on simple rules.
    /// </summary>
    public sealed class GameOfLifeParallelVersion
    {
        private readonly bool[,] initialGrid;
        private readonly int rows;
        private readonly int columns;
        private bool[,] currentGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOfLifeParallelVersion"/> class with the specified number of rows and columns of the grid.
        /// The initial state of the grid is randomly set with alive or dead cells.
        /// </summary>
        /// <param name="rows">The number of rows in the grid.</param>
        /// <param name="columns">The number of columns in the grid.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of rows or columns is less than or equal to 0.</exception>
        public GameOfLifeParallelVersion(int rows, int columns)
        {
            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), "Rows must be greater than zero.");
            }

            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), "Columns must be greater than zero.");
            }

            this.rows = rows;
            this.columns = columns;
            this.initialGrid = new bool[rows, columns];
            this.currentGrid = new bool[rows, columns];
            this.InitializeRandomGrid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOfLifeParallelVersion"/> class with the given grid.
        /// </summary>
        /// <param name="grid">The 2D array representing the initial state of the grid.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="grid"/> is null.</exception>
        public GameOfLifeParallelVersion(bool[,] grid)
        {
            this.initialGrid = grid ?? throw new ArgumentNullException(nameof(grid));
            this.rows = grid.GetLength(0);
            this.columns = grid.GetLength(1);
            this.currentGrid = (bool[,])this.initialGrid.Clone();
        }

        /// <summary>
        /// Gets the current generation grid as a separate copy.
        /// </summary>
        public bool[,] CurrentGeneration => this.currentGrid;

        /// <summary>
        /// Gets the current generation number.
        /// </summary>
        public int Generation { get; private set; }

        /// <summary>
        /// Restarts the game by resetting the current grid to the initial state.
        /// </summary>
        public void Restart()
        {
            this.currentGrid = (bool[,])this.initialGrid.Clone();
            this.Generation = 0;
        }

        /// <summary>
        /// Advances the game to the next generation based on the rules of Conway's Game of Life.
        /// </summary>
        public void NextGeneration()
        {
            var nextGrid = new bool[this.rows, this.columns];

            Parallel.For(0, this.rows, row =>
            {
                for (int col = 0; col < this.columns; col++)
                {
                    int aliveNeighbors = this.CountAliveNeighbors(row, col);
                    bool isAlive = this.currentGrid[row, col];

                    nextGrid[row, col] = isAlive switch
                    {
                        true => false,
                        false when aliveNeighbors == 3 => true,
                        _ => false,
                    };
                }
            });

            this.currentGrid = nextGrid;
            this.Generation++;
        }

        /// <summary>
        /// Counts the number of alive neighbors for a given cell in the grid.
        /// </summary>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="column">The column index of the cell.</param>
        /// <returns>The number of alive neighbors for the specified cell.</returns>
        private int CountAliveNeighbors(int row, int column)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int newRow = row + i;
                    int newCol = column + j;

                    if (newRow >= 0 && newRow < this.rows && newCol >= 0 && newCol < this.columns && this.currentGrid[newRow, newCol])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void InitializeRandomGrid()
        {
            var rand = new Random();
            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.columns; col++)
                {
                    this.currentGrid[row, col] = rand.Next(2) == 1;
                }
            }
        }
    }
}
