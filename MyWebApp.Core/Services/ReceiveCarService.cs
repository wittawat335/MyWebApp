using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static MyWebApp.Core.Model.ViewModels.ReceiveCar.ReceiveCarViewModel;

namespace MyWebApp.Core.Services
{
    public class ReceiveCarService : IReceiveCarService
    {
        private readonly IReceiveCarRepository _repositorySp;
        private readonly IGenericRepository<T_JOB_REPO> _repository;
        private readonly IGenericRepository<T_REPO_NOTE> _noteRepository;
        Common common = new Common();

        public ReceiveCarService(IReceiveCarRepository repositorySP, IGenericRepository<T_JOB_REPO> repository,
            IGenericRepository<T_REPO_NOTE> noteRepository)
        {
            _repositorySp = repositorySP;
            _repository = repository;
            _noteRepository = noteRepository;
        }

        public async Task<List<SP_SEARCH_RC_Result>> GetAllbySp(SearchModel model)
        {
            try
            {
                var list = await _repositorySp.GetAllBySp(model);
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<T_JOB_REPO>> GetAll()
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

        public async Task<bool> SaveFile(List<FileUploadModel> model)
        {  
            var listNote = new List<T_REPO_NOTE>();
            try
            {
                if (model.Count() > 0)
                {
                    foreach (var item in model)
                    {
                        var findId = await _repository
                            .Get(x => x.JOB_CONTRACT_NO == item.ContractNo);
                        if(findId != null)
                        {
                            var note = new T_REPO_NOTE();
                            note.NOTE_JOB_ID = findId.JOB_ID;
                            note.NOTE_CONTRACT_NO = item.ContractNo;
                            note.NOTE_REMARK = item.Remark;
                            note.NOTE_STATUS = Constants.Status.Active;
                            note.NOTE_CREATE_BY = item.TrackingBy;
                            note.NOTE_CREATE_DATE = common
                                .ConvertStringToDateTime(item.TrackingDate);
                            note.NOTE_UPDATE_BY = item.TrackingBy;
                            note.NOTE_UPDATE_DATE = common
                                .ConvertStringToDateTime(item.TrackingDate);
                            listNote.Add(note);
                        }                     
                    }                   
                }
                var query = await _noteRepository.AddList(listNote);
                return query;
            }
            catch
            {
                throw;
            }
        }

        public string GenerateAndDownLoadExcel()
        {
            try
            {
                byte[] fileContents = null;
                ExcelPackage.LicenseContext =
                    LicenseContext.NonCommercial;
                using (ExcelPackage pk = new ExcelPackage())
                {
                    ExcelWorksheet ws =
                        pk.Workbook.Worksheets.Add("test");

                    ws.Cells["A1"].Value = "ContractNo";
                    ws.Cells["B1"].Value = "TrackingBy";
                    ws.Cells["C1"].Value = "TrackingDate";
                    ws.Cells["D1"].Value = "Remark";
                    ws.Cells["A1:D1"].Style.Font.Bold = true;
                    ws.Cells["A1:D1"].Style.Font.Size = 12;
                    ws.Cells["A1:D1"].Style.Fill.PatternType =
                        ExcelFillStyle.Solid;
                    ws.Cells["A1:D1"].Style.Fill.BackgroundColor
                        .SetColor(System.Drawing.Color.SkyBlue);
                    ws.Cells["A1:D1"].Style.VerticalAlignment =
                        ExcelVerticalAlignment.Center;
                    ws.Cells["A1:D1"].Style.HorizontalAlignment =
                        ExcelHorizontalAlignment.Center;

                    pk.Save();
                    fileContents = pk.GetAsByteArray();
                }
                return Convert.ToBase64String(fileContents);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
