namespace Domain.Exceptions;

public class ReportIdNotFoundException : Exception
{
    public ReportIdNotFoundException()
    {
    }

    public ReportIdNotFoundException(string message)
    : base(message)
    {
    }

    public ReportIdNotFoundException(string message, Exception inner)
    : base(message, inner)
    {
    }
}
