using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClothingStore.API.Filters.Actions;

public class LoggingActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Log("Ação executada.", context.RouteData);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Log("Ação em execução", context.RouteData);
    }

    public override void OnResultExecuted(ResultExecutedContext context)
    {
        Log("Resultado executado", context.RouteData);
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        Log("Resultado em execução", context.RouteData);
    }

    private void Log(string pipelineStage, RouteData route)
    {
        var (controller, action) = (route.Values["controller"], route.Values["action"]);
        var message = $"{pipelineStage}. Controller: {controller}. Action: {action}";
        Debug.WriteLine(message, "Action log");
    }
}
