using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using Newtonsoft.Json;
using static MyWebApp.Core.Model.ViewModels.TreeViewInAspNetCor;

namespace MyWebApp.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly IRoleService _service;
        private readonly IPermissionService _permissionService;
        Common common = new Common();

        public TestController(IRoleService service, IPermissionService permissionService)
        {
            _service = service;
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            //add parent nodes
            nodes.Add(new TreeViewNode { id = "1", parent = "#", text = "Rajasthan" });
            nodes.Add(new TreeViewNode { id = "2", parent = "#", text = "Gujrat" });


            //add child nodes for first parent
            nodes.Add(new TreeViewNode { id = "11", parent = "1", text = "Jaipur" });
            nodes.Add(new TreeViewNode { id = "12", parent = "1", text = "Ajmer" });

            //add child nodes for first parent
            nodes.Add(new TreeViewNode { id = "13", parent = "11", text = "test" });
            nodes.Add(new TreeViewNode { id = "14", parent = "12", text = "test2" });

            //add child nodes for second parent
            nodes.Add(new TreeViewNode { id = "21", parent = "2", text = "Baroda" });
            nodes.Add(new TreeViewNode { id = "22", parent = "2", text = "Surat" });


            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
      
            return View();
        }

        [HttpPost]
        public ActionResult Index(string selectedItems)
        {
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> test()
        {
            string code = "CO";

            List<TreeViewNode> nodes = new List<TreeViewNode>();
            List<SP_SEARCH_PERMISSION_BY_ROLE_Result> objData = await _service.GetPermissionData(code);
            if (objData != null && objData.Count > 0)
            {

                int? iCountTopLevel = objData.Count;

                for (int i = 0; i < iCountTopLevel; i++)
                {

                    bool varSelect = false;
                    if (objData[i].PERM_SELECT == "1")
                        varSelect = true;

                    string strIcon;
                    switch (objData[i].PERM_TEXT)
                    {
                        case Constants.JsTreeConfig.TextAdd:
                            strIcon = Constants.JsTreeConfig.IconAdd;
                            break;
                        case Constants.JsTreeConfig.TextEdit:
                            strIcon = Constants.JsTreeConfig.IconEdit;
                            break;
                        case Constants.JsTreeConfig.TextView:
                            strIcon = Constants.JsTreeConfig.IconView;
                            break;
                        default:
                            {
                                strIcon = Constants.JsTreeConfig.IconDefault;
                                break;
                            }
                    }

                    string strParent = (string.IsNullOrEmpty(objData[i].PERM_PARENT)) ? "#" : objData[i].PERM_PARENT;
                    bool booParentOpen = false;
                    OptState objStates = new OptState { opened = booParentOpen, selected = varSelect };
                    nodes.Add(new TreeViewNode()
                    {
                        id = objData[i].PERM_ID.ToString(),
                        parent = strParent,
                        text = objData[i].PERM_TEXT,
                        icon = strIcon,
                        state = objStates
                    });
                }
            }


            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetPermissionData(string roleCode)
        {
            try
            {
                List<DataPermissionJsonList> objReturn = new List<DataPermissionJsonList>();
                List<SP_SEARCH_PERMISSION_BY_ROLE_Result> objData = await _service.GetPermissionData(roleCode);
                if (objData != null && objData.Count > 0)
                {

                    int? iCountTopLevel = objData.Count;

                    for (int i = 0; i < iCountTopLevel; i++)
                    {

                        bool varSelect = false;
                        if (objData[i].PERM_SELECT == "1")
                            varSelect = true;

                        string strIcon;
                        switch (objData[i].PERM_TEXT)
                        {
                            case Constants.JsTreeConfig.TextAdd:
                                strIcon = Constants.JsTreeConfig.IconAdd;
                                break;
                            case Constants.JsTreeConfig.TextEdit:
                                strIcon = Constants.JsTreeConfig.IconEdit;
                                break;
                            case Constants.JsTreeConfig.TextView:
                                strIcon = Constants.JsTreeConfig.IconView;
                                break;
                            default:
                                {
                                    strIcon = Constants.JsTreeConfig.IconDefault;
                                    break;
                                }
                        }

                        string strParent = (string.IsNullOrEmpty(objData[i].PERM_PARENT)) ? "#" : objData[i].PERM_PARENT;
                        bool booParentOpen = false;
                        OptionState objStates = new OptionState { opened = booParentOpen, selected = varSelect };
                        objReturn.Add(new DataPermissionJsonList()
                        {
                            id = objData[i].PERM_ID.ToString(),
                            parent = strParent,
                            text = objData[i].PERM_TEXT,
                            icon = strIcon,
                            state = objStates
                        });
                    }
                }
                return Json(objReturn);
            }
            catch
            {
                throw;
            }
        }
    }
}
