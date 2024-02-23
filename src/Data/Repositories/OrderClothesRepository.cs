using ClothingStore.API.Helpers;
using ClothingStore.API.Models;
using Dapper;

namespace ClothingStore.API.Data.Repositories;

public interface IOrderClothesRepository
{
    Task AddOrderClothes(OrderClothes orderClothes);
    Task<IEnumerable<OrderClothes>> GetOrderClothes();
    Task<OrderClothes> GetOrderClothesById(Guid Id);
    Task DeleteOrderClothes(Guid Id);
    Task UpdateOrderClothes(OrderClothes orderClothes);
}

public class OrderClothesRepository : IOrderClothesRepository
{
    private readonly DapperContext _context;

    public OrderClothesRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task AddOrderClothes(OrderClothes orderClothes)
    {
        var command = """
            INSERT INTO orderclothes (id, totalclothingquantity, clothes_id, orders_id)
            VALUES (@Id, @TotalClothingQuantity, @Clothes_Id, @Orders_Id)
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, orderClothes);
    }

    public async Task DeleteOrderClothes(Guid Id)
    {
        var command = """
            DELETE FROM orderclothes WHERE id = @Id;
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, new { Id });

    }

    public async Task<IEnumerable<OrderClothes>> GetOrderClothes()
    {
        var query = """
            SELECT * FROM orderclothes;
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.QueryAsync<OrderClothes>(query);
    }

    public async Task<OrderClothes> GetOrderClothesById(Guid Id)
    {
        var query = """
            SELECT * FROM orderclothes WHERE id = @Id;
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.QuerySingleOrDefault(query, new { Id });
    }

    public async Task UpdateOrderClothes(OrderClothes orderClothes)
    {
        var command = """
            UPDATE orderclothes 
            SET totalclothingquantity = @TotalClothingQuantity,
            clothes_id = @Clothes_Id,
            orders_id = @Orders_Id
            WHERE id = @Id
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, orderClothes);
    }
}
