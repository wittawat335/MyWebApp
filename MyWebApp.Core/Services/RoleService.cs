using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
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
        private readonly IGenericRepository<M_PERMISSION> _perRepository;
        private readonly IRoleRepository _roleRepository;
        Common common = new Common();

        public RoleService(IGenericRepository<M_ROLE> repository, IGenericRepository<M_PERMISSION> perRepository ,IRoleRepository roleRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _perRepository = perRepository;
        }

        public async Task<bool> DeletePermissionListByCode(string code)
        {
            try
            {
                var query = await _perRepository.GetAll(x => x.PERM_ROLE_CODE == code);               
                if(query.ToList().Count() > 0)
                    await _perRepository.DeleteList(query.ToList());
               
                return true;
            }
            catch
            {
                return false;
            }
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

        public async Task<bool> Childrenlist(string roleCode, List<DataPermissionJsonInsertList> model)
        {
            string progCode = "";
            bool result = false;
            
            try
            {
                foreach (DataPermissionJsonInsertList list in model)
                {
                    if (list.children.Count() > 0)
                    {
                        progCode = list.id;
                        await Childrenlist(roleCode, list.children);
                    }
                    else 
                    {
                        if (list.state.selected)
                        {
                            M_PERMISSION perm = new M_PERMISSION();
                            perm.PERM_ROLE_CODE = roleCode;
                            perm.PERM_PROG_CODE = progCode;
                            perm.PERM_ACT_CODE = list.id;
                            perm.PERM_USE_FLAG = "1";
                            perm.PERM_CREATE_BY = common.UserLogin;
                            perm.PERM_CREATE_DATE = common.SystemDate;
                            perm.PERM_UPDATE_BY = common.UserLogin;
                            perm.PERM_UPDATE_DATE = common.SystemDate;
                            perm.PERM_STATUS = Constants.Status.Active;
                            _perRepository.AddOnly(perm);
                        }
                    }
                }
                await _perRepository.SaveChangesAsync();
                result = true;
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
