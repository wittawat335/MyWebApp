using System;
using System.Collections.Generic;

namespace MyWebApp.Core.Domain.Entities;

public partial class S_CONTRACT_CUST
{
    public string CONTRACT_NO { get; set; } = null!;

    public string CUST_CODE { get; set; } = null!;

    public string CUST_BR_TYPE { get; set; } = null!;

    public int? GUARANTOR_SEQ { get; set; }

    public string? SEND_SMS { get; set; }

    public DateTime? DATA_IMPORT_DATE { get; set; }
}
