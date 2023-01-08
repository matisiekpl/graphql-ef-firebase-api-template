namespace MyProject.Exceptions;

public class GraphQLErrorFilter:IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception switch
        {
            ForbiddenException => error.WithMessage("Access forbidden").WithCode("ForbiddenException"),
            BadPermissionException => error.WithMessage("Bad permission").WithCode("BadPermissionException"),
            _ => error
        };
    }
}