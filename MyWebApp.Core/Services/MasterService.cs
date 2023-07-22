using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Services.Contract;

namespace MyWebApp.Core.Services
{
    public class MasterService : IMasterService
    {
        private readonly IGenericRepository<M_MASTER> _repository;
        private readonly IMapper _mapper;

        public MasterService(IGenericRepository<M_MASTER> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<MasterDTO>> GetAll()
        {
            try
            {
                var list = await _repository.GetAll();
                return _mapper.Map<List<MasterDTO>>(list);
            }
            catch {
                throw;
            }
        }
        public async Task<List<MasterDTO>> GetListByMasterType(string code)
        {
            try
            {
                var list = await _repository.GetAll(x => x.MASTER_TYPE == code);
                return _mapper.Map<List<MasterDTO>>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<MasterDTO> GetByCode(string code)
        {
            try
            {
                var list = await _repository.Get(x => x.MASTER_CODE == code);
                return _mapper.Map<MasterDTO>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<MasterDTO>> Search(MasterViewModel model)
        {
            try
            {
                var list = await _repository.GetAll();
                if (model.masterType != null)
                    list = list.Where(x => x.MASTER_TYPE.Contains(model.masterType));
                if (model.masterCode != null)
                    list = list.Where(x => x.MASTER_CODE.Contains(model.masterCode));
                if (model.MasterNameTH != null)
                    list = list.Where(x => 
                    x.MASTER_NAME_TH.Contains(model.MasterNameTH));
                if (model.MasterNameEN != null)
                    list = list.Where(x => 
                    x.MASTER_NAME_EN.Contains(model.MasterNameEN));
                if (model.masterStatus != null)
                    list = list.Where(x => 
                    x.MASTER_STATUS.Contains(model.masterStatus));

                return _mapper.Map<List<MasterDTO>>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Add(MasterDTO model)
        {
            try
            {
                var insert = await _repository.Add(_mapper.Map<M_MASTER>(model));        
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Update(MasterDTO model)
        {
            try
            {
                var mapper = _mapper.Map<M_MASTER>(model);
                var dataUpdate = await _repository.Get(x => 
                x.MASTER_CODE == mapper.MASTER_CODE);
                if (dataUpdate == null)
                    throw new TaskCanceledException("No Data");

                dataUpdate.MASTER_TYPE = mapper.MASTER_TYPE;
                dataUpdate.MASTER_NAME_TH = mapper.MASTER_NAME_TH;
                dataUpdate.MASTER_NAME_EN = mapper.MASTER_NAME_EN;
                dataUpdate.MASTER_UPDATE_BY = mapper.MASTER_UPDATE_BY;
                dataUpdate.MASTER_UPDATE_DATE = mapper.MASTER_UPDATE_DATE;
                dataUpdate.MASTER_STATUS = mapper.MASTER_STATUS;

                bool updated = await _repository.Update(dataUpdate);
                if (!updated)
                    throw new TaskCanceledException("Cannot Update Data");

                return updated;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Delete(string code)
        {
            try
            {
                var query = await _repository.Get(x => x.MASTER_CODE == code);
                if (query == null)
                    throw new TaskCanceledException("No Data");

                bool deleted = await _repository.Delete(query);
                if (deleted == null)
                    throw new TaskCanceledException("No Data");

                return deleted;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<MasterDTO>> GetListMasterActiveOnly()
        {
            try
            {
                var list = await _repository.GetAll(x => x.MASTER_STATUS == "A");
                if (list != null)
                    list = list.GroupBy(x => x.MASTER_TYPE)
                        .Select(x => x.FirstOrDefault());
                    
                return _mapper.Map<List<MasterDTO>>(list);
            }
            catch
            {
                throw;
            }
        }
    }
}
