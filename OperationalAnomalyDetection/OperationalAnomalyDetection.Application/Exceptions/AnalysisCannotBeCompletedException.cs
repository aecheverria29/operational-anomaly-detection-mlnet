namespace OperationalAnomalyDetection.Application.Exceptions;

public sealed class AnalysisCannotBeCompletedException : Exception
{
    public AnalysisCannotBeCompletedException(string message)
        : base(message)
    {
    }
}
