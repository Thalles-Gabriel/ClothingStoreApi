using ClothingStore.API.Helpers;
using ClothingStore.API.Models;
using Dapper;

namespace ClothingStore.API.Data.Repositories;

public interface IClothesRepository
{
    Task<IEnumerable<Clothes>> GetClothes();
    Task<Clothes> GetClothesById(Guid Id);
    Task UpdateClothing(Clothes clothing);
    Task AddClothing(Clothes clothing);
    Task DeleteClothing(Guid Id);
    Task<bool> ClothesHasOrderItems(Guid Id);
}

public class ClothesRepository : IClothesRepository
{

    private readonly DapperContext _context;

    public ClothesRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task DeleteClothing(Guid Id)
    {
        var command = """
            DELETE FROM clothes
            WHERE id = @Id;
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, new { Id });
    }

    public async Task<IEnumerable<Clothes>> GetClothes()
    {
        var query = "SELECT * FROM clothes;";
        using var connection = _context.NewConnection();
        connection.Open();
        var clothes = await connection.QueryAsync<Clothes>(query);

        return clothes;
    }

    public async Task<Clothes> GetClothesById(Guid Id)
    {
        var query = "SELECT * FROM clothes WHERE id = @Id;";
        using var connection = _context.NewConnection();
        connection.Open();
        var clothing = await connection.QuerySingleOrDefaultAsync<Clothes>(query, new { Id });
        return clothing;
    }

    public async Task AddClothing(Clothes clothing)
    {
        var command = """
            INSERT INTO clothes
            (id, name, size, color, brand, price, tags, description, gender, type, quantity) 
            VALUES (@Id, @Name, @Size, @Color, @Brand, @Price, @Tags, @Description, @Gender, @Type, @Quantity);
            """;
        using var connection = _context.NewConnection();
        connection.Open();

        await connection.ExecuteAsync(command, clothing);
    }

    public async Task UpdateClothing(Clothes clothing)
    {
        var command = """
            UPDATE clothes
            SET name = @Name,
                size = @Size,
                color = @Color,
                brand = @Brand,
                price = @Price,
                tags = @Tags,
                description = @Description,
                gender = @Gender,
                type = @Type,
                quantity = @quantity
            WHERE id = @Id;
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, clothing);
    }

    public async Task<bool> ClothesHasOrderItems(Guid Id)
    {
        var query = """
            SELECT EXISTS (SELECT id FROM orderclothes WHERE clothes_id = @Id);
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.ExecuteScalarAsync<bool>(query, new { Id });
    }
}
