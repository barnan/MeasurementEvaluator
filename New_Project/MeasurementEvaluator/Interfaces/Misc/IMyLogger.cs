
namespace Interfaces.Misc
{
    internal interface IMyLogger
    {
        string Trace(string input, params object[] parameters);
        string Info(string input, params object[] parameters);
        string Debug(string input, params object[] parameters);
        string Warning(string input, params object[] parameters);
        string Error(string input, params object[] parameters);
    }
}
