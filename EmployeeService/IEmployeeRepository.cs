using System.Threading.Tasks;

namespace EmployeeService
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> GetEnabledByIdAsync(int id);
        Task<bool> UpdateEnableAsync(int id, bool enable);
    }
}
