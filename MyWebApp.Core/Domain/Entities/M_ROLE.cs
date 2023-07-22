using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Core.Domain.Entities;

public partial class M_ROLE
{
    /// <summary>
    /// รหัสสิทธิ์
    /// </summary>
    [Display(Name = "รหัสสิทธิ์")]
    public string ROLE_CODE { get; set; } = null!;

    /// <summary>
    /// ชื่อสิทธิ์
    /// </summary>
    [Display(Name = "ชื่อสิทธิ์")]
    public string ROLE_NAME { get; set; } = null!;

    /// <summary>
    /// ระดับการเข้าถึงข้อมูล
    /// </summary>
    [Display(Name = "ระดับการเข้าถึงข้อมูล")]
    public int ROLE_DATA_LEVEL { get; set; }

    /// <summary>
    /// ผู้สร้าง
    /// </summary>
    public string? ROLE_CREATE_BY { get; set; }

    /// <summary>
    /// วันที่สร้าง
    /// </summary>
    public DateTime? ROLE_CREATE_DATE { get; set; }

    /// <summary>
    /// ผู้แก้ไข
    /// </summary>
    [Display(Name = "ผู้แก้ไข")]
    public string? ROLE_UPDATE_BY { get; set; }

    /// <summary>
    /// วันที่แก้ไข
    /// </summary>
    [Display(Name = "วันที่แก้ไข")]
    public DateTime? ROLE_UPDATE_DATE { get; set; }

    /// <summary>
    /// สถานะข้อมูล A=ใช้งาน,I=ไม่ใช้งาน
    /// </summary>
    [Display(Name = "สถานะข้อมูล")]
    public string? ROLE_STATUS { get; set; }
}
