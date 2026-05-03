using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace EmployeeService
{
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetEmployeeById?id={id}",
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Employee> GetEmployeeById(int id);

        [OperationContract]
        [WebGet(UriTemplate = "GetEnabledEmployeeById?id={id}",
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Employee> GetEnabledEmployeeById(int id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "EnableEmployee?id={id}&enable={enable}",
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<string> EnableEmployee(int id, bool enable);
    }
}
