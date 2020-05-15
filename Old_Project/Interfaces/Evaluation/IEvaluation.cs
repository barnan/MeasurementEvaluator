using Interfaces.Misc;
using Interfaces.Result;

namespace Interfaces.Evaluation
{

    /// <summary>
    /// Evaluates the obtained specifications wit hthe obtained reference file and measurement datas
    /// </summary>
    public interface IEvaluation : IInitializable, IResultProvider
    {
    }
}
