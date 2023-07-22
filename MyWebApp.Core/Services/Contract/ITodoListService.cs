using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model.ViewModels.TodoList;

namespace MyWebApp.Core.Services.Contract
{
    public interface ITodoListService
    {
        Task<List<T_JOB_ALL>> Search(SearchViewModel model);
        #region select2
        Task<List<RefNo>> GetRefNo(string code = "");
        Task<List<SP_SEARCH_CUSTOMER_Result>> GetCustomer(
            string code = "");
        Task<List<M_USER>> GetListUserByRole(string code = "");
        Task<List<M_OA>> GetListOA(string code = "");
        Task<List<M_LEGAL_CASE>> GetListLegalCaseByGroup(
            string group = "");
        #endregion
    }
}
