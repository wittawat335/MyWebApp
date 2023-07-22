using System;
using System.Collections.Generic;

namespace MyWebApp.Core.Domain.Entities;

public partial class T_LEGAL_ATTACHMENT
{
    public string ATTACH_JOB_ID { get; set; } = null!;

    public int ATTACH_SEQUENCE { get; set; }

    public string? ATTACH_FILE_NAME { get; set; }

    public string? ATTACH_DESCRIPTION { get; set; }

    public DateTime? ATTACH_DATE { get; set; }

    public string? ATTACH_BY { get; set; }
}
