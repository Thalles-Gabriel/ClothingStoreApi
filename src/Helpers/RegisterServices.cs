using ClothingStore.API.Data.Repositories;
using ClothingStore.API.Filters.Actions;

namespace ClothingStore.API.Helpers;

public static class RegisterServices
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClothesRepository, ClothesRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IOrderClothesRepository, OrderClothesRepository>();
        services.AddScoped<IUsersRepository, UserRepository>();
    }

    public static void AddFilters(this IServiceCollection services)
    {
        services.AddScoped<CheckClothesQuantityFilter>();
        services.AddScoped<CheckDataDependenciesFilter>();
    }
}
