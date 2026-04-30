using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo = new EmployeeRepository(
            ConfigurationManager.ConnectionStrings["EmployeeDb"].ConnectionString);
        //http://localhost:64014/EmployeeService.svc/GetEmployeeById?id=1
        public async Task<Employee> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _repo.GetById(id);

                if (employee == null)
                    throw new WebFaultException<string>($"Employee with ID {id} not found.", HttpStatusCode.NotFound);

                return employee;
            }
            catch (WebFaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new WebFaultException<string>("An unexpected error occurred.", HttpStatusCode.InternalServerError);
            }
        }
        //http://localhost:64014/EmployeeService.svc/EnableEmployee?id=1&enable=true
        public async Task<string> EnableEmployee(int id, bool enable)
        {
            try
            {
                var updated = await _repo.UpdateEnable(id, enable);

                if (!updated)
                    throw new WebFaultException<string>($"Employee with ID {id} not found.", HttpStatusCode.NotFound);

                return $"Employee with ID {id} was updated successfully.";
            }
            catch (WebFaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new WebFaultException<string>("An unexpected error occurred.", HttpStatusCode.InternalServerError);
            }
        }
    }
}
