using ClothingStore.API.Helpers;
using ClothingStore.API.Models;
using Dapper;

namespace ClothingStore.API.Data.Repositories;

public interface IOrdersRepository
{
    Task<IEnumerable<Orders>> GetOrders();
    Task<Orders> GetOrderById(Guid Id);
    Task UpdateOrder(Orders order);
    Task AddOrder(Orders order);
    Task DeleteOrder(Guid Id);
    Task<bool> OrderHasClothesItems(Guid Id);
}

public class OrdersRepository : IOrdersRepository
{
    private readonly DapperContext _context;

    public OrdersRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task AddOrder(Orders order)
    {
        var command = """
            INSERT INTO "orders" ( id, timeofsale, paymethod, orderaddress, user_id, lastchanged)
            VALUES (@Id, 
                    @TimeOfSale, 
                    @PayMethod, 
                    @OrderAddress,
                    @User_Id,
                    @LastChanged);
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command,order);
    }

    public async Task DeleteOrder(Guid Id)
    {
        var command = """
            DELETE FROM "orders" WHERE id = @Id;
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, new { Id });
    }

    public async Task<Orders> GetOrderById(Guid Id)
    {
        var query = """
            SELECT * FROM "orders" WHERE id = @Id;
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.QuerySingleAsync<Orders>(query, new { Id });
    }

    public async Task<IEnumerable<Orders>> GetOrders()
    {
        var query = """
            SELECT * FROM "orders";
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.QueryAsync<Orders>(query);

    }

    public async Task UpdateOrder(Orders order)
    {
        var command = """
            UPDATE "orders"
            SET paymethod = @PayMethod,
                orderaddress = @OrderAddress,
                user_id = @User_Id,
                lastchanged = @LastChanged
            WHERE id = @Id;
            """;

        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, order );
    }
    public async Task<bool> OrderHasClothesItems(Guid Id)
    {
        var query = """
            SELECT EXISTS (SELECT id FROM orderclothes WHERE orders_id = @Id);
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.ExecuteScalarAsync<bool>(query, new { Id });
    }
}
