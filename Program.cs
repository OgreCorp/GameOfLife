// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments

using System.Diagnostics;

/// <summary>Convert List of strings to List of Cells</summary>
/// <param name="lines">List of strings representing input</param>
/// <returns>List of Cells</returns>
List<Cell> Initialize(List<string> lines)
{
    if (lines.First() != "#Life 1.06")
    {
        throw new Exception("Invalid header");
    }
    var initialState = new List<Cell>();
    foreach (var line in lines.TakeLast(lines.Count - 1))
    {
        var split = line.Split(' ');
        if (split.Length != 2)
        {
            throw new Exception("Invalid line");
        }
        if (!split.First().IsNumeric())
        {
            throw new Exception("X is not numeric");
        }
        if (!split.Last().IsNumeric())
        {
            throw new Exception("Y is not numeric");
        }
        var x = Convert.ToInt64(split.First());
        var y = Convert.ToInt64(split.Last());
        var cell = new Cell
        {
            X = x,
            Y = y
        };
        initialState.Add(cell);
    }
    return initialState;
}

/// <summary>Output a List of Cells to Debug</summary>
/// <param name="cells">List of Cells to output</param>
void DebugCells(List<Cell> cells)
{
    if (Debugger.IsAttached)
    {
        foreach (var cell in cells)
        {
            var neighborCount = GetLivingCellNeighborCount(cells, cell);
            Debug.Indent();
            Debug.WriteLine($"Cell: {cell} -> Living Neighbors: {neighborCount}");
            Debug.Unindent();
        }
    }
}

/// <summary>Output a List of Cells to Standard Output</summary>
/// <param name="cells">List of Cells to output</param>
void StandardOutputCells(List<Cell> cells)
{
    foreach (var cell in cells)
    {
        var neighborCount = GetLivingCellNeighborCount(cells, cell);
        Console.WriteLine($"{cell.X} {cell.Y}");
    }
}

/// <summary>Get count of living neighbor Cells for a given Cell from input Cells</summary>
/// <param name="cells">List of Cells</param>
/// <param name="cell">Cell to find neighbor count</param>
/// <returns>Short of neighbor count</returns>
/// <remarks>
/// As a possible refinement or optimization, <code>var testCell = new Cell(x,y);</code> could be replaced
/// with one declaration of <code>Cell testCell;</code>, which would move it to the stack instead of heap
/// Another possible refinement could be with a lambda, where the X and Y offsets are passed to
/// something like <code>internal Func{int, int, bool} IsCellNeighbor; ... IsCellNeighbor = (offsetX, offsetY) => { };</code>
/// Another possible refinement could be to combine all the Min/Max checks and For loop through all 8 coords
/// but I ran out of time and we are probably approaching diminishing returns vs. effort
/// </remarks>
int GetLivingCellNeighborCount(List<Cell> cells, Cell cell)
{
    int cellCount = 0;
    // x-1,y-1
    if (cell.X > long.MinValue && cell.Y > long.MinValue)
    {
        var testCell = new Cell(cell.X - 1, cell.Y - 1);
        if (cells.Contains(testCell))
        {
            cellCount++;
        }
    }
    //x,y-1
    if (cell.Y > long.MinValue)
    {
        var testCell = new Cell(cell.X, cell.Y - 1);
        if (cells.Contains(testCell))
        {
            cellCount++;
        }
    }
    //x+1,y-1
    if (cell.X < long.MaxValue && cell.Y > long.MinValue)
    {
        var testCell = new Cell(cell.X + 1, cell.Y - 1);
        if (cells.Contains(testCell))
        {
            cellCount++;
        }
    }
    //x+1,y
    if (cell.X < long.MaxValue)
    {
        var testCell = new Cell(cell.X + 1, cell.Y);
        if (cells.Contains(testCell))
        {
            cellCount++;
        }        
    }
    //x+1,y+1
    if (cell.X < long.MaxValue && cell.Y < long.MaxValue)
    {
        var testCell = new Cell(cell.X + 1, cell.Y + 1);
        if (cells.Contains(testCell))
        {
            cellCount++;
        }  
    }
    //x,y+1
    if (cell.Y < long.MaxValue)
    {
        var testCell = new Cell(cell.X, cell.Y + 1);
        if (cells.Contains(testCell))
        {
            cellCount++;
        } 
    }
    //x-1,y+1
    if (cell.X > long.MinValue && cell.Y < long.MaxValue)
    {
        var testCell = new Cell(cell.X - 1, cell.Y + 1);
        if (cells.Contains(testCell))
        {
            cellCount++;
        } 
    }
    //x-1,y
    if (cell.X > long.MinValue)
    {
        var testCell = new Cell(cell.X - 1, cell.Y);
        if (cells.Contains(testCell))
        {
            cellCount++;
        } 
    }
    return cellCount;
}

/// <summary>Gets a List of surviving Cells for next iteration</summary>
/// <param name="cells">List of Cells to find survivors</param>
/// <returns>List of surviving Cells</returns>
List<Cell> GetSurvivingCells(List<Cell> cells)
{
    var newState = new List<Cell>();
    foreach (var cell in cells)
    {
        var neighborCount = GetLivingCellNeighborCount(cells, cell);
        if (neighborCount == 2 || neighborCount == 3)
        {
            newState.Add(cell);
        }
    }
    return newState;
}

/// <summary>Gets a List of neighbor Cells to Cell that are dead</summary>
/// <param name="cells">List of Cells</param>
/// <param name="cell">Cell to find dead neighbors</param>
/// <returns>List of dead neighbor Cells</returns>
/// <remarks>
/// As a possible refinement or optimization, <code>var testCell = new Cell(x,y);</code> could be replaced
/// with one declaration of <code>Cell testCell;</code>, which would move it to the stack instead of heap
/// Another possible refinement could be with a lambda, where the X and Y offsets are passed to
/// something like <code>internal Func{int, int, bool} IsNeighborCellDead; ... IsNeighborCellDead = (offsetX, offsetY) => { };</code>
/// Another possible refinement could be to combine all the Min/Max checks and For loop through all 8 coords
/// but I ran out of time and we are probably approaching diminishing returns vs. effort
/// </remarks>
List<Cell> GetNeighborDeadCells(List<Cell> cells, Cell cell)
{
    var deadNeighborCells = new List<Cell>();
    // x-1,y-1
    if (cell.X > long.MinValue && cell.Y > long.MinValue)
    {
        var testCell = new Cell(cell.X - 1, cell.Y - 1);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }
    }
    //x,y-1
    if (cell.Y > long.MinValue)
    {
        var testCell = new Cell(cell.X, cell.Y - 1);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }
    }
    //x+1,y-1
    if (cell.X < long.MaxValue && cell.Y > long.MinValue)
    {
        var testCell = new Cell(cell.X + 1, cell.Y - 1);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }
    }
    //x+1,y
    if (cell.X < long.MaxValue)
    {
        var testCell = new Cell(cell.X + 1, cell.Y);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }    
    }
    //x+1,y+1
    if (cell.X < long.MaxValue && cell.Y < long.MaxValue)
    {
        var testCell = new Cell(cell.X + 1, cell.Y + 1);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }
    }
    //x,y+1
    if (cell.Y < long.MaxValue)
    {
        var testCell = new Cell(cell.X, cell.Y + 1);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }
    }
    //x-1,y+1
    if (cell.X > long.MinValue && cell.Y < long.MaxValue)
    {
        var testCell = new Cell(cell.X - 1, cell.Y + 1);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }
    }
    //x-1,y
    if (cell.X > long.MinValue)
    {
        var testCell = new Cell(cell.X - 1, cell.Y);
        if (!cells.Contains(testCell))
        {
            deadNeighborCells.Add(testCell);
        }
    }
    return deadNeighborCells;
}

/// <summary>Gets a List of all dead Cells that are neighbors to input Cells</summary>
/// <param name="cells">List of Cells</param>
/// <returns>List of Cells that are dead</returns>
List<Cell> GetAllNeighborDeadCells(List<Cell> cells)
{
    var allNeighborDeadCells = new List<Cell>();
    foreach (var cell in cells)
    {
        var neighborDeadCells = GetNeighborDeadCells(cells, cell);
        allNeighborDeadCells.AddRange(neighborDeadCells);
    }
    var distinctAllNeighborNamedCells = allNeighborDeadCells.Distinct().ToList();
    return distinctAllNeighborNamedCells;
}

/// <summary>Gets a List of spawning Cells for next iteration</summary>
/// <param name="cells">List of Cells to find spawners</param>
/// <returns>List of spawning Cells</returns>
List<Cell> GetSpawningCells(List<Cell> cells)
{
    var neighborDeadCells = GetAllNeighborDeadCells(cells);
    Debug.WriteLine("Neighbor dead cells");
    DebugCells(neighborDeadCells);
    var spawningCells = new List<Cell>();
    foreach (var cell in neighborDeadCells)
    {
        var livingCellNeighborCount = GetLivingCellNeighborCount(cells, cell);
        if (livingCellNeighborCount == 3)
        {
            spawningCells.Add(cell);
        }
    }
    return spawningCells;
}

/// <summary>Iterate through one generation of life</summary>
/// <param name="cells">List of Cells to start iteration</param>
/// <returns>List of Cells representing next iteration</returns>
List<Cell> IterateCells(List<Cell> cells)
{
    var survivingCells = GetSurvivingCells(cells);
    Debug.WriteLine("Survivors");
    DebugCells(survivingCells);
    var spawningCells = GetSpawningCells(cells);
    Debug.WriteLine("Spwaning cells");
    DebugCells(spawningCells);
    var mergedState = new List<Cell>();
    mergedState.AddRange(survivingCells);
    mergedState.AddRange(spawningCells);
    Debug.WriteLine("Merged state");
    DebugCells(mergedState);
    return mergedState;
}

// Modern C# entry point
// Check if we have a valid input file, if not, read from standard input
var filename = @".\data.life";
var lines = new List<string>();
if (File.Exists(filename))
{
    var fileLines = File.ReadAllLines(@".\data.life");
    lines = fileLines.ToList();
}
else
{
    string? input;
    while (true)
    {
        input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input))
        {
            lines.Add(input);
        }
        else
        {
            break;
        }
    }
}

// Initialize some cells from input
var cells = Initialize(lines);

// Iterate 10 times
for (var iteration = 1; iteration <= 10; iteration++)
{
    Debug.WriteLine($"Iteration: {iteration}");
    Debug.Indent();
    Debug.WriteLine("Initial cells");
    DebugCells(cells);
    cells = IterateCells(cells);
    Debug.Unindent();
}
Debug.WriteLine("End of iterations");

// Output 10th iteration
StandardOutputCells(cells);

/// <summary>Represents a cell in the Game of Life</summary>
struct Cell
{
    /// <summary>Initializes the Cell</summary>
    /// <param name="x">Sets X value</param>
    /// <param name="y">Sets Y value</param>
    public Cell(long x, long y)
    {
        X = x;
        Y = y;
    }
    /// <value>Long of Cell's X value</value
    public long X { get; init; }
    /// <value>Long of Cell's Y value</value>
    public long Y { get; init; }
    /// <summary>Returns a string representation of the Cell</summary>
    public override string ToString() => $"{X}, {Y}";
}

/// <summary>Class for holding extension methods</summary>
static class Extensions
{
    /// <summary>Tests if a string is a number</summary>
    /// <returns>True if the string is a number, otherwise, returns false</returns> 
    public static bool IsNumeric(this string input)
    {
        return long.TryParse(input, out long result);
    }
}
