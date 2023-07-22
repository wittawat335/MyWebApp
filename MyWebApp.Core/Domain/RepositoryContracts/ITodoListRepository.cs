using MyWebApp.Core.Model.ViewModels.TodoList;

namespace MyWebApp.Core.Domain.RepositoryContracts
{
    public interface ITodoListRepository 
    {
        Task<List<RefNo>?> GetRefNoAsync(string text);
        Task<List<SP_SEARCH_CUSTOMER_Result>?> GetCustomer(string text);
    }
}
