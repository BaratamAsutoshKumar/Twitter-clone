using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TwitterClone.Infrastructure.Repositories;

public class VulnerableRepository
{
    private readonly DbContext _context;
    
    public VulnerableRepository(DbContext context)
    {
        _context = context;
    }

    public object GetUserBadly(string username)
    {
        // VULNERABILITY: EF Core SQL Injection via raw string concatenation (CWE-89)
        string query = $"SELECT * FROM AspNetUsers WHERE UserName = '{username}'";
        
        // Snyk will detect this because it uses FromSqlRaw instead of parameterized inputs
        return _context.Database.SqlQueryRaw<int>(query).ToList();
    }
}