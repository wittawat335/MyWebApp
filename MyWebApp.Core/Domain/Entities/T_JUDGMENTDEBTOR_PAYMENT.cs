﻿using System;
using System.Collections.Generic;

namespace MyWebApp.Core.Domain.Entities;

public partial class T_JUDGMENTDEBTOR_PAYMENT
{
    public string JDP_HID { get; set; } = null!;

    public int JDP_TERM { get; set; }

    public string? JDP_DESCRIPTION { get; set; }

    public DateTime? JDP_DUEDATE { get; set; }

    public decimal? JDP_INSTALLMENT { get; set; }

    public string? JDP_CREATE_BY { get; set; }

    public DateTime? JDP_CREATE_DATE { get; set; }

    public string? JDP_UPDATE_BY { get; set; }

    public DateTime? JDP_UPDATE_DATE { get; set; }

    public string? JDP_STATUS { get; set; }
}
