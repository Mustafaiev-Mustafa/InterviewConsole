using System.Threading.Tasks;

namespace EmployeeService
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetById(int id);
        Task<bool> UpdateEnable(int id, bool enable);
    }
}
