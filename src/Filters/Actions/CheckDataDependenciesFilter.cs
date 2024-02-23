using ClothingStore.API.Controllers;
using ClothingStore.API.Data.Repositories;
using ClothingStore.API.Helpers;
using ClothingStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClothingStore.API.Filters.Actions;

public class CheckDataDependenciesFilter : ActionFilterAttribute
{
    private readonly IClothesRepository _clothesRepository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IUsersRepository _usersRepository;

    public CheckDataDependenciesFilter(
            IClothesRepository clothesRepository,
            IOrdersRepository ordersRepository,
            IUsersRepository usersRepository
            )
    {
        _clothesRepository = clothesRepository;
        _ordersRepository = ordersRepository;
        _usersRepository = usersRepository;
    }


    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var model = context.ActionArguments["orderClothes"] as OrderClothes;

        if (HttpMethods.IsDelete(context.HttpContext.Request.Method))
        {
            await CheckForeignTableBeforeDeletion(context, model);
        }
        else
        {
            await OrderClothesPostPutForeignCheck(context, model);
        }
        if (context.Result is null) await next();
    }

    private async Task OrderClothesPostPutForeignCheck(ActionExecutingContext context, OrderClothes model)
    {
        var clothesId = model.Clothes_Id;
        var orderId = model.Orders_Id;

        var clothing = await _clothesRepository.GetClothesById(clothesId);
        if (clothing is null)
        {
            var response = CustomValidationResponse.CreateResponse(
                    "Roupa não encontrada",
                    "Id da roupa informada não retornou resultado.",
                    StatusCodes.Status400BadRequest,
                    context.ModelState
                    );
            context.Result = new BadRequestObjectResult(response);
            return;
        }
        context.ActionArguments.Add("clothing", clothing);
        
        var order = await _ordersRepository.GetOrderById(orderId);
        if (order is null)
        {
            var response = CustomValidationResponse.CreateResponse(
                    "Venda não encontrada",
                    "Id da venda informada não retornou resultado.",
                    StatusCodes.Status400BadRequest,
                    context.ModelState
                    );
            context.Result = new BadRequestObjectResult(response);
            return;
        }
    }

    private async Task CheckForeignTableBeforeDeletion(ActionExecutingContext context, OrderClothes model)
    {
        var controllerId = model.Id;

        var currentController = context.Controller;
        if (currentController.GetType() == typeof(UsersController))
        {
            var hasEntity = await _usersRepository.UserHasOrder(controllerId);
            if (hasEntity) return;

            var response = CustomValidationResponse.CreateResponse(
                    "Remoção impedida",
                    "Usuário possui pedidos registrados",
                    StatusCodes.Status400BadRequest,
                    context.ModelState
                    );

            context.Result = new BadRequestObjectResult(response);
        }
        else if (currentController.GetType() == typeof(OrdersController))
        {
            var hasEntity = await _ordersRepository.OrderHasClothesItems(controllerId);
            if (hasEntity) return;

            var response = CustomValidationResponse.CreateResponse(
                    "Remoção impedida",
                    "Pedido possui itens para venda.",
                    StatusCodes.Status400BadRequest,
                    context.ModelState
                    );

            context.Result = new BadRequestObjectResult(response);
        }

        else if (currentController.GetType() == typeof(ClothesController))
        {
            var hasEntity = await _clothesRepository.ClothesHasOrderItems(controllerId);
            if (hasEntity) return;

            var response = CustomValidationResponse.CreateResponse(
                    "Remoção impedida",
                    "Peça está incluído em pedido para venda.",
                    StatusCodes.Status400BadRequest,
                    context.ModelState
                    );

            context.Result = new BadRequestObjectResult(response);
        }
    }
}
