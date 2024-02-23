using ClothingStore.API.Helpers;
using ClothingStore.API.Models;
using Dapper;

namespace ClothingStore.API.Data.Repositories;

public interface IUsersRepository
{
    Task<IEnumerable<Users>> GetUsers();
    Task<Users> GetUserById(Guid Id);
    Task UpdateUser(Users user);
    Task AddUser(Users user);
    Task DeleteUser(Guid Id);
    Task<bool> UserHasOrder(Guid Id);
}

public class UserRepository : IUsersRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task AddUser(Users user)
    {
        var command = """
            INSERT INTO "users" (id, name, email, password, address)
            VALUES (@Id, @Name, @Email, @Password, @Address);
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, user);
    }

    public async Task DeleteUser(Guid Id)
    {
        var command = """
            DELETE FROM "users" WHERE id = @Id;
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, new { Id });
    }

    public async Task<Users> GetUserById(Guid Id)
    {
        var query = """
            SELECT * FROM "users" WHERE id = @Id;
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.QuerySingleOrDefaultAsync<Users>(query, new { Id });
    }

    public async Task<IEnumerable<Users>> GetUsers()
    {
        var query = """
            SELECT * FROM "users";
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.QueryAsync<Users>(query);
    }

    public async Task UpdateUser(Users user)
    {
        var command = """
            UPDATE "users"
            SET name = @Name,
                email = @Email,
                password = @Password,
                address = @Address
            WHERE id = @Id;
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        await connection.ExecuteAsync(command, user);
    }

    public async Task<bool> UserHasOrder(Guid Id)
    {
        var query = """
            SELECT EXISTS (SELECT id FROM orders WHERE user_id = @Id);
            """;
        using var connection = _context.NewConnection();
        connection.Open();
        return await connection.ExecuteScalarAsync<bool>(query, new { Id });
    }

}
