# Conway’s Game of Life

Beginner level task for practicing of the Task Parallel Library features.

Estimated time to complete the task - 3h.

The task requires .NET 8 SDK installed.

## Task description

Implement a simulation of [Conway's Game of Life](https://www.wikiwand.com/en/Conway%27s_Game_of_Life), a classic cellular automaton devised by mathematician John Conway. This zero-player game involves a grid of cells that can be in one of two states: alive (populated) or dead. The game evolves over generations based on a set of simple rules, and no further input is required once the initial state is set. The rules determine the state of each cell in the next generation based on the current state of the cell and the states of its eight neighboring cells.

### Task Details

1. Implement `GameOfLifeSequentialVersion` class that represents the sequencial version of the game and provides methods to simulate the evolution of the grid based on the rules. The class offers the following functionalities:
- `Constructors`: You must implement two constructors. The first constructor initializes a new game grid with the specified number of rows and columns. The initial state of the grid is randomly set with alive or dead cells. The second constructor initializes a new game grid using a given 2D array representing the initial state.
- `CurrentGeneration` property: This property returns a copy of the current generation grid as a separate 2D array.
- `Generation` property: This property gets the current generation number.
- `Restart` method: This method resets the game by setting the current grid to the initial state.
- `NextGeneration` method: This method advances the game to the next generation based on the rules of Conway's Game of Life.
- `CountAliveNeighbors` method: This method counts the number of alive neighbors for a given cell in the grid.

2. Implement `GameOfLifeParallelVersion` class that represents the parallel version of the game and provides methods to simulate the evolution of the grid based on the rules. The class offers the following functionalities:
- `Constructors`: You must implement two constructors. The first constructor initializes a new game grid with the specified number of rows and columns. The initial state of the grid is randomly set with alive or dead cells. The second constructor initializes a new game grid using a given 2D array representing the initial state.
- `CurrentGeneration` property: This property returns a copy of the current generation grid as a separate 2D array.
- `Generation` property: This property gets the current generation number.
- `Restart` method: This method resets the game by setting the current grid to the initial state.
- `NextGeneration` method: This method advances the game to the next generation based on the rules of Conway's Game of Life.
- `CountAliveNeighbors` method: This method counts the number of alive neighbors for a given cell in the grid.

_Notes_:
- Use a 2D array, a list of cells, or any other suitable data structure to grid representation.
- Use the following evolution rules:
    - Any live cell with fewer than two live neighbors dies (underpopulation).
    - Any live cell with two or three live neighbors lives on to the next generation.
    - Any live cell with more than three live neighbors dies (overpopulation).
    - Any dead cell with exactly three live neighbors becomes a live cell (reproduction).

3. Implement an `GameOfLifeExtension` extension class that provides extention methods to simulate and visualize the evolution of Conway's Game of Life using the provided `GameOfLifeSequentialVersion` and `GameOfLifeParallelVersion` classes. The extension methods should run the simulation for a specified number of generations and display the grid at each step.
- The `Simulate` method should run the simulation for the specified number of generations, calling the `NextGeneration` method of the `GameOfLifeSequentialVersion` class. At each generation, it should write the generation number and the current grid state to the `writer` using the provided characters for alive and dead cells.
- The `SimulateAsync` method should function similarly to the `Simulate` method, but with the additional capability of asynchronous writing. It should use the `await` keyword to write the generation number and the current grid state asynchronously to the `writer` at each generation.
