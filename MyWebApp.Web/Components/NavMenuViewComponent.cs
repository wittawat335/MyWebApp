using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model.ViewModels;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using Newtonsoft.Json;

namespace MyWebApp.Web.Components
{
    public class NavMenuViewComponent : ViewComponent
    {
        private readonly IProgramService _programService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public NavMenuViewComponent(
            IProgramService programService,
            IHttpContextAccessor contextAccessor,
            IMapper mapper)
        {
            _programService = programService;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = new List<ProgramDTO>();
            var logInInfo = new LoginInfo();
            var code = "";
            try
            {
                string sessionString = HttpContext.Session.GetString(Constants.SessionKey.LoginInfo);
                if (sessionString != null)
                    logInInfo = JsonConvert.DeserializeObject<LoginInfo>(sessionString);

                if (logInInfo.Role != null)
                    code = logInInfo.Role;

                menu = await _programService.GetByRoleAsync(code);

                return View(menu);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
