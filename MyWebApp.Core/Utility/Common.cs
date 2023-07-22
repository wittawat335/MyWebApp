using Microsoft.AspNetCore.Http;
using MyWebApp.Core.Model.ViewModels;
using MyWebApp.Core.Model.ViewModels.TodoList;
using MyWebApp.Core.Services;
using MyWebApp.Core.Services.Contract;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static MyWebApp.Core.Model.ViewModels.User.UserViewModel;

namespace MyWebApp.Core.Utility
{
    public class Common
    {
        IHttpContextAccessor _contxt = new HttpContextAccessor();
        private string GetipAddress()
        {
            string strHostName = "127.0.0.1";
            try
            {
                strHostName = _contxt.HttpContext.Connection.RemoteIpAddress.ToString();

                return strHostName;
            }
            catch (Exception)
            {
                return strHostName;
            }

        }
        public void SetLoginSession(long id, string userLogin,
           string userName,
           string role,
           string roleName,
           int dataLevel)
        {
            var loginInfo = new LoginInfo
            {
                clID = id,
                UserLogin = userLogin,
                FullName = userName,
                Role = role,
                RoleName = roleName,
                DataLevel = dataLevel.ToString()
            };

            string sessionString = JsonConvert.SerializeObject(loginInfo);
            _contxt.HttpContext.Session.SetString(Constants.SessionKey.LoginInfo, sessionString);
        }
        public List<Role> GetRoleList()
        {
            try
            {
                var list = new List<Role>();
                string listString = _contxt.HttpContext.Session.GetString("RoleList");
                if (!string.IsNullOrEmpty(listString))
                    list = JsonConvert.DeserializeObject<List<Role>>(listString);

                if (list.Count() > 0)
                    list = list.Where(x => x.RoleFlag).ToList();

                return list;
            }
            catch
            {
                throw;
            }
        }
        public class LoginInfo
        {
            public long clID { get; set; }
            public string? UserLogin { get; set; }
            public string? FullName { get; set; }
            public string? Role { get; set; }
            public string? RoleName { get; set; }
            public string? DataLevel { get; set; }
        }
        public DateTime ConvertStringToDateTime(string value)
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                if (value != null)
                    dateTime = DateTime
                        .ParseExact(value, "dd/MM/yyyy", null);

                return dateTime;
            }
            catch
            {
                throw;
            }
        }
        public DataTable ConvertListDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
                DataTable dt = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    dt.Columns.Add(prop.Name,
                        Nullable.GetUnderlyingType(
                        prop.PropertyType) ?? prop.PropertyType);
                }
                //object[] values = new object[props.Count];
                //foreach (var item in data)
                //{
                //    for (int i = 0; i < values.Length; i++)
                //    {
                //        values[i] = props[i].GetValue(item);
                //    }
                //    dt.Rows.Add(values);
                //}
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<TrafficType> GetListTraffic()
        {
            List<TrafficType> list = new List<TrafficType>();
            TrafficType item = new TrafficType();
            try
            {

                item.CODE = Constants.Color.GreenCode;
                item.TEXT = Constants.Color.Green;
                list.Add(item);

                item = new TrafficType();
                item.CODE = Constants.Color.YellowCode;
                item.TEXT = Constants.Color.Yellow;
                list.Add(item);

                item = new TrafficType();
                item.CODE = Constants.Color.RedCode;
                item.TEXT = Constants.Color.Red;
                list.Add(item);


                return list.ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }
       
        public decimal CalculateMonth(decimal amount, DateTime? dtStartDate, DateTime? dtEndDate, int avg)
        {
            decimal total = 0;
            int year = 0;
            int month = 0;
            int week = 0;
            int days = 0;

            try
            {
                if (!IsNullOrEmpty(dtStartDate) && !IsNullOrEmpty(dtEndDate))
                {
                    GetDifference(dtStartDate,
                        dtEndDate,
                        ref year,
                        ref month,
                        ref week,
                        ref days);
                    month = year * 12 + month;
                    days = week * 7 + days;

                    total = amount * month + days * (amount / avg);
                }

                return total;
            }
            catch
            {
                return 0;
            }
        }
        public void GetDifference(DateTime? date1, DateTime? date2, ref int Years,
             ref int Months, ref int Weeks, ref int Days)
        {
            //assumes date2 is the bigger date for simplicity

            //years
            TimeSpan diff = date2.Value - date1.Value;
            Years = diff.Days / 366;
            DateTime workingDate = date1.Value.AddYears(Years);

            while (workingDate.AddYears(1) <= date2)
            {
                workingDate = workingDate.AddYears(1);
                Years++;
            }

            //months
            diff = date2.Value - workingDate;
            Months = diff.Days / 31;
            workingDate = workingDate.AddMonths(Months);

            while (workingDate.AddMonths(1) <= date2)
            {
                workingDate = workingDate.AddMonths(1);
                Months++;
            }

            //weeks and days
            diff = date2.Value - workingDate;
            Weeks = diff.Days / 7; //weeks always have 7 days
            Days = diff.Days % 7;
        }

        #region Function Convert

        public string UnStringNull(object obj)
        {
            try
            {

                return obj.ToString().Trim();
            }
            catch
            {
                return "";
            }

        }
        public string UnStringNullstring(string obj)
        {
            try
            {
                return obj;
            }
            catch
            {
                return "";
            }

        }
        public string UnDateTimeStringNull(object obj)
        {
            try
            {

                return DateTime.Parse(obj.ToString()).ToString(Constants.Config.DateFormat);
            }
            catch
            {
                return "";
            }

        }
        public double UnDoubleNull(object obj)
        {
            try
            {
                return Convert.ToDouble(obj.ToString().Replace(",", "").Trim());
            }
            catch
            {
                return 0;
            }

        }
        public decimal UnDecimalNull(object obj)
        {
            try
            {
                return Convert.ToDecimal(obj.ToString().Replace(",", "").Trim());
            }
            catch
            {
                return 0;
            }

        }
        public int UnInt32Null(object obj)
        {
            try
            {
                return Convert.ToInt32(obj.ToString().Replace(",", "").Trim());
            }
            catch
            {
                return 0;
            }

        }
        public bool IsNullOrEmpty(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return true;
            else
                return false;
        }
        public bool IsNullOrEmpty(string obj)
        {
            if (obj == null || obj.Trim().Equals(""))
            {
                return true;
            }

            return false;
        }
        protected long UnInt64Null(object obj)
        {
            try
            {
                return Convert.ToInt64(obj.ToString().Replace(",", "").Trim());
            }
            catch
            {
                return 0;
            }

        }






        public void DuplicateTable(DataTable srcTable, DataTable targetTable)
        {
            try
            {
                targetTable.Rows.Clear();
                DataRow dr = null;
                foreach (DataRow dr_loopVariable in srcTable.Rows)
                {
                    foreach (DataColumn dc in targetTable.Columns)
                    {
                        if (dr_loopVariable.IsNull(dc.ColumnName))
                        {
                            switch (targetTable.Columns[dc.ColumnName].DataType.Name.ToLower())
                            {
                                case "dbnull":
                                    dr_loopVariable[dc.ColumnName] = "";
                                    break;

                                case "string":
                                    dr_loopVariable[dc.ColumnName] = "";
                                    break;

                                case "datetime":
                                    // Nick 2015-05-06DateTime dateEmpty = new DateTime();
                                    // dr_loopVariable[dc.ColumnName] = dateEmpty;
                                    dr_loopVariable[dc.ColumnName] = DBNull.Value;
                                    break;

                                case "byte":
                                    dr_loopVariable[dc.ColumnName] = DBNull.Value;
                                    break;

                                case "int16":
                                    dr_loopVariable[dc.ColumnName] = DBNull.Value;
                                    break;

                                case "int32":
                                    dr_loopVariable[dc.ColumnName] = DBNull.Value;
                                    break;

                                case "int64":
                                    dr_loopVariable[dc.ColumnName] = DBNull.Value;
                                    break;

                                case "double":
                                    dr_loopVariable[dc.ColumnName] = DBNull.Value;
                                    break;

                                case "decimal":
                                    dr_loopVariable[dc.ColumnName] = DBNull.Value;
                                    break;

                                case "boolean":
                                    break;
                            }
                        }
                    }
                    dr = dr_loopVariable;
                    targetTable.ImportRow(dr);
                }
                srcTable.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Function Display
        public string DateDisplay(DateTime? objDate)
        {
            DateTime dt = new DateTime();
            try
            {

                return objDate == null || objDate == dt ? "" : DateDisplay((DateTime)objDate);
            }
            catch
            {
                return "";
            }
        }

        public string DateDisplay(DateTime objDate)
        {
            DateTime dt = new DateTime();
            try
            {
                return objDate == null || objDate == dt ? "" : objDate.ToString(Constants.Config.DateFormat);
            }
            catch
            {
                return "";
            }
        }

        public string DateTimeDisplay(DateTime? objDateTime)
        {
            DateTime dt = new DateTime();
            try
            {
                return objDateTime == null || objDateTime == dt ? "" : DateTimeDisplay((DateTime)objDateTime);
            }
            catch
            {
                return "";
            }
        }

        public string DateTimeDisplay(DateTime objDateTime)
        {
            DateTime dt = new DateTime();
            try
            {
                return objDateTime == null || objDateTime == dt ? "" : objDateTime.ToString(Constants.Config.DateTimeFormat);
            }
            catch
            {
                return "";
            }
        }

        public string DecimalDisplay(decimal? objDec, int decPlace = 2)
        {
            try
            {
                return objDec == null ? 0.ToString("n" + decPlace.ToString()) : DecimalDisplay((decimal)objDec, decPlace);
            }
            catch
            {
                return 0.ToString("n" + decPlace.ToString());
            }
        }

        public string DecimalDisplay(decimal objDec, int decPlace = 2)
        {
            try
            {
                return objDec.ToString("n" + decPlace.ToString());
            }
            catch
            {
                return 0.ToString("n" + decPlace.ToString());
            }
        }

        public string DateSort(DateTime? objDate)
        {
            try
            {
                return objDate == null ? "" : DateSort((DateTime)objDate);
            }
            catch
            {
                return "";
            }
        }

        public string DateSort(DateTime objDate)
        {
            try
            {
                return objDate.ToString("yyyyMMddHHmm");
            }
            catch
            {
                return "";
            }
        }

        public string DecimalSort(decimal? objDec, int decPlace = 2)
        {
            try
            {
                return objDec == null ? 0.ToString("n" + decPlace.ToString()) : DecimalSort((decimal)objDec, decPlace);
            }
            catch
            {
                return "0000000000" + 0.ToString("n" + decPlace.ToString()).Split('.').Last();
            }
        }

        public string DecimalSort(decimal objDec, int decPlace = 2)
        {
            try
            {
                var toReturn = "";
                toReturn = Convert.ToInt32(objDec).ToString("0000000000");
                toReturn += objDec.ToString("n" + decPlace.ToString()).Split('.').Last();
                return toReturn;
            }
            catch
            {
                return "0000000000" + 0.ToString("n" + decPlace.ToString()).Split('.').Last();
            }
        }





        #endregion
        public List<ddlValuse> GetListStatus()
        {
            List<ddlValuse> list = new List<ddlValuse>();
            ddlValuse item = new ddlValuse();
            try
            {

                item.CODE = Constants.Status.Active;
                item.TEXT = Constants.Status.ActiveText;
                list.Add(item);

                item = new ddlValuse();
                item.CODE = Constants.Status.Inactive;
                item.TEXT = Constants.Status.InactiveText;
                list.Add(item);

                return list.ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<ddlValuse> GetListTypeUser()
        {
            List<ddlValuse> list = new List<ddlValuse>();
            ddlValuse item = new ddlValuse();
            try
            {

                item.CODE = "0";
                item.TEXT = "Internal";
                list.Add(item);

                item = new ddlValuse();
                item.CODE = "1";
                item.TEXT = "External";
                list.Add(item);

                return list.ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }
        public string UserLogin
        {
            get
            {
                LoginInfo logInInfo = new LoginInfo();
                logInInfo = GetValueBySession();

                return logInInfo.UserLogin;
            }
        }
        public string UserName
        {
            get
            {
                var loginInfo = new LoginInfo();
                loginInfo = GetValueBySession();

                return loginInfo.FullName;
            }
        }

        public string UserRole
        {
            get
            {
                var loginInfo = new LoginInfo();
                loginInfo = GetValueBySession();

                return loginInfo.Role;
            }
        }
        public string UserRoleName
        {
            get
            {
                var loginInfo = new LoginInfo();
                loginInfo = GetValueBySession();

                return loginInfo.RoleName;
            }
        }
        public string UserDataLevel
        {
            get
            {
                var loginInfo = new LoginInfo();
                loginInfo = GetValueBySession();

                return loginInfo.DataLevel;
            }
        }
        public string Ip
        {
            get
            {

                string ip = GetipAddress();

                if (!IsNullOrEmpty(ip))
                {
                    Ip = ip;
                    return ip;
                }
                else
                    return "127.0.0.1";

            }
            set
            {
                _contxt.HttpContext.Session.SetString(Constants.SessionKey.Ip, value);
            }
        }
        public DateTime SystemDate
        {
            get
            {
                //DateTime sysDate = new DateTime(); 
                //string session = _contxt.HttpContext.Session.GetString("SysDate");
                //if (session != null)
                //    sysDate = JsonConvert.DeserializeObject<DateTime>(session);
                //else
                //    sysDate = DateTime.Now;

                return DateTime.Now;
            }
        }
        public LoginInfo GetValueBySession()
        {
            var logInInfo = new LoginInfo();
            string sessionString = _contxt.HttpContext.Session.GetString(Constants.SessionKey.LoginInfo);

            if (sessionString != null)
                logInInfo = JsonConvert.DeserializeObject<LoginInfo>(sessionString);

            return logInInfo;
        }
        public string Encrypt(string text)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());

                }
            }

            return text;
        }
        public string Decrypt(string text)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return text;
        }
        public class ddlValuse
        {
            public string? CODE { get; set; }
            public string? TEXT { get; set; }
        }

        public List<StudentMarkDetails> GetStudentMarkList()
        {
            var studentmarkList = new List<StudentMarkDetails>()
            {
                new StudentMarkDetails() { id = 1, name = "John", Physics = 62,Chemistry=78,Biology=84,Mathematics=96 },
                new StudentMarkDetails() { id = 2, name = "Mary", Physics = 96,Chemistry=78,Biology=69,Mathematics=88 },
                new StudentMarkDetails() { id = 3, name = "Asha", Physics = 49,Chemistry=85,Biology=63,Mathematics=77 },
                new StudentMarkDetails() { id = 4, name = "Emily", Physics = 85,Chemistry=56,Biology=78,Mathematics=55 },
                new StudentMarkDetails() { id = 5, name = "Bonnie", Physics = 74,Chemistry=55,Biology=36,Mathematics=69 },
            };
            return studentmarkList;
        }
        public List<StudentScoreDetails> GetStudentScoreDetails()
        {
            var studentscoreList = new List<StudentScoreDetails>()
            {
                new StudentScoreDetails() { id = 1, name = "John", score = 62},
                new StudentScoreDetails() { id = 2, name = "Mary", score = 96 },
                new StudentScoreDetails() { id = 3, name = "Asha", score = 49 },
                new StudentScoreDetails() { id = 4, name = "Emily", score = 85 },
                new StudentScoreDetails() { id = 5, name = "Bonnie", score = 74},
            };
            return studentscoreList;
        }
    }
}
