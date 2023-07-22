﻿using System;
using System.Collections.Generic;

namespace MyWebApp.Core.Domain.Entities;

public partial class S_CONTRACT_OVERDUE
{
    public string CONTRACT_NO { get; set; } = null!;

    public DateTime? OVERDUE_SINCE { get; set; }

    public DateTime? DELINQUENT_SINCE { get; set; }

    public int? OVERDUE_DAYS { get; set; }

    public int? DELINQUENCY_DAYS { get; set; }

    public DateTime? UPDATE_DATE { get; set; }
}
