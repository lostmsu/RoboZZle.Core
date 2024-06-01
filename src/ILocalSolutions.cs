namespace RoboZZle.Core;

using System.Collections.Generic;

public interface ILocalSolutions {
    /// <summary>
    /// Adds solution
    /// </summary>
    void Add(LocalSolution solution);

    /// <summary>
    /// All registered solutions
    /// </summary>
    IEnumerable<LocalSolution> Solutions { get; }
}