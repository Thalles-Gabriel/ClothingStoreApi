using ClothingStore.API.Helpers;
using ClothingStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClothingStore.API.Filters.Actions;

public class CheckClothesQuantityFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var model = context.ActionArguments["orderClothes"] as OrderClothes;
        var clothes = context.ActionArguments["clothing"] as Clothes;
        var orderClothesQuantity = model.TotalClothingQuantity;
        if (orderClothesQuantity > clothes.Quantity)
        {
            var response = CustomValidationResponse.CreateResponse(
                    "Quantidade de roupa na venda é inválida.",
                    "A quantidade de roupa nesta venda é maior que o número em estoque.", 
                    StatusCodes.Status400BadRequest,
                    context.ModelState
                    );
            context.Result = new BadRequestObjectResult(response);
            return;
        }
        await next();
    }
}
