using static MyWebApp.Core.Model.ViewModels.ReceiveCar.ReceiveCarViewModel;

namespace MyWebApp.Core.Domain.RepositoryContracts
{
    public interface IReceiveCarRepository
    {
        Task<List<SP_SEARCH_RC_Result>> GetAllBySp(SearchModel model);
    }
}
