using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Threading.Tasks;
public class QuoteRepository : BaseRepository, IRepository<Quote>
{

    public QuoteRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<IEnumerable<Quote>> GetAll()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Quote>("SELECT * FROM Quotes;");

    }

    public void Delete(long id)
    {
        using var connection = CreateConnection();
        connection.Execute("DELETE FROM Quotes WHERE Id = @Id;", new { Id = id });
    }

    public async Task<Quote> Get(long id)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleAsync<Quote>("SELECT * FROM Quotes WHERE Id = @Id;", new { Id = id });
    }

    public async Task<Quote> Update(Quote quote)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleAsync<Quote>("UPDATE Quotes SET Wisdom = @Wisdom, Author = @Author WHERE Id = @Id RETURNING *", quote);
    }

    public async Task<Quote> Insert(Quote quote)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleAsync<Quote>("INSERT INTO Quotes (Wisdom, Author) VALUES (@Wisdom, @Author) RETURNING *;", quote);
    }

    public async Task<IEnumerable<Quote>> Search(string query, int limit, int page)
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Quote>("SELECT * FROM Quotes WHERE Wisdom ILIKE @Query OR Author ILIKE @Query LIMIT @Limit OFFSET @Offset;", new { Query = $"%{query}%", Limit = limit, Offset = (page - 1) * limit });
    }

}

