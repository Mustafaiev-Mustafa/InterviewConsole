using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        private const string SqlAll = @"
            WITH EmployeeTree AS
            (
                SELECT [ID], [Name], [ManagerID], [Enable]
                FROM [Employee]
                WHERE [ID] = @id

                UNION ALL

                SELECT e.[ID], e.[Name], e.[ManagerID], e.[Enable]
                FROM [Employee] e
                INNER JOIN EmployeeTree et ON e.[ManagerID] = et.[ID]
            )
            SELECT * FROM EmployeeTree
            OPTION (MAXRECURSION 100)";

        private const string SqlEnabledOnly = @"
            WITH EmployeeTree AS
            (
                SELECT [ID], [Name], [ManagerID], [Enable]
                FROM [Employee]
                WHERE [ID] = @id AND [Enable] = 1

                UNION ALL

                SELECT e.[ID], e.[Name], e.[ManagerID], e.[Enable]
                FROM [Employee] e
                INNER JOIN EmployeeTree et ON e.[ManagerID] = et.[ID]
                WHERE e.[Enable] = 1
            )
            SELECT * FROM EmployeeTree
            OPTION (MAXRECURSION 100)";

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<Employee> GetById(int id) => ExecuteTreeQuery(SqlAll, id);

        public Task<Employee> GetEnabledById(int id) => ExecuteTreeQuery(SqlEnabledOnly, id);

        public async Task<bool> UpdateEnable(int id, bool enable)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand("UPDATE [Employee] SET [Enable] = @enable WHERE [ID] = @id", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@enable", SqlDbType.Bit).Value = enable;
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        private async Task<Employee> ExecuteTreeQuery(string sql, int id)
        {
            var list = new List<Employee>();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var colId = reader.GetOrdinal("ID");
                        var colName = reader.GetOrdinal("Name");
                        var colManagerId = reader.GetOrdinal("ManagerID");
                        var colEnable = reader.GetOrdinal("Enable");

                        while (await reader.ReadAsync())
                        {
                            list.Add(new Employee
                            {
                                ID = reader.GetInt32(colId),
                                Name = reader.GetString(colName),
                                ManagerID = reader.IsDBNull(colManagerId) ? (int?)null : reader.GetInt32(colManagerId),
                                Enable = reader.GetBoolean(colEnable),
                                Employees = new List<Employee>()
                            });
                        }
                    }
                }
            }

            var dict = list.ToDictionary(e => e.ID);
            foreach (var employee in list)
            {
                if (employee.ManagerID.HasValue && dict.TryGetValue(employee.ManagerID.Value, out var manager))
                    manager.Employees.Add(employee);
            }

            dict.TryGetValue(id, out var root);
            return root;
        }
    }
}
