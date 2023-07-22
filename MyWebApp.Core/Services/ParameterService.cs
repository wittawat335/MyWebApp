using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Core.Services
{
    public class ParameterService : IParameterService
    {
        private readonly IGenericRepository<M_PARAMETER> _repository;
        private readonly IMapper _mapper;

        public ParameterService(IGenericRepository<M_PARAMETER> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<bool> Save(List<M_PARAMETER> model)
        {
            var result = false;
            try
            {
                foreach (var item in model)
                {
                    var query = await _repository
                        .Get(x => x.PARA_CODE == item.PARA_CODE);
                    query.PARA_VALUE = item.PARA_VALUE;
                    query.PARA_UPDATE_BY = item.PARA_UPDATE_BY;
                    query.PARA_UPDATE_DATE = item.PARA_UPDATE_DATE;

                    var update = await _repository.Update(query);
                    result = update;
                }
                return result;
            }
            catch {
                throw;
            }
        }
        public async Task<List<ParameterDTO>> GetAll()
        {
            try
            {
                var list = await _repository
                    .GetAll(x => x.PARA_STATUS == Constants.Status.Active);
                return _mapper.Map<List<ParameterDTO>>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<ParameterDTO> GetByCode(string code)
        {
            try
            {
                var list = await _repository
                    .Get(x => x.PARA_CODE == code);
                return _mapper.Map<ParameterDTO>(list);
            }
            catch
            {
                throw;
            }
        }
    }
}
