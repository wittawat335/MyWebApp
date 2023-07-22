using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<M_ROLE> _repository;
        private readonly IRoleRepository _roleRepository;

        public RoleService(IGenericRepository<M_ROLE> repository, IRoleRepository roleRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
        }

        public async Task<M_ROLE> GetByCode(string code)
        {
            try
            {
                return await _repository.Get(x => x.ROLE_CODE == code); 
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<M_ROLE>> GetList()
        {
            try
            {
                var list = await _repository.GetAll();

                return list.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<SP_SEARCH_PERMISSION_BY_ROLE_Result>> GetPermissionData(string code)
        {
            try
            {
                var list = await _roleRepository.GetSP_SEARCH_PERMISSION(code);
                return list.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
