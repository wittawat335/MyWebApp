using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.DTO
{
    public class UserDTO
    {
        [Display(Name = "Username")]
        public string USER_LOGIN { get; set; } = null!;
        [Display(Name = "Password")]
        public string? USER_PASSWORD { get; set; }

        /// <summary>
        /// ชื่อ
        /// </summary>
        [Display(Name = "First Name")]
        public string? USER_FIRST_NAME { get; set; }

        /// <summary>
        /// นามสกุล
        /// </summary>
        [Display(Name = "Last Name")]
        public string? USER_LAST_NAME { get; set; }
        [Display(Name = "Type")]
        public string? USER_AD_FLAG { get; set; }

        /// <summary>
        /// ผู้สร้าง
        /// </summary>
        public string? USER_CREATE_BY { get; set; }

        /// <summary>
        /// วันที่สร้าง
        /// </summary>
        public DateTime? USER_CREATE_DATE { get; set; }

        /// <summary>
        /// ผู้แก้ไข
        /// </summary>
        public string? USER_UPDATE_BY { get; set; }

        /// <summary>
        /// วันที่แก้ไข
        /// </summary>
        public DateTime? USER_UPDATE_DATE { get; set; }

        /// <summary>
        /// สถานะข้อมูล
        /// </summary>
        [Display(Name = "Status")]
        public string? USER_STATUS { get; set; }
    }
}
