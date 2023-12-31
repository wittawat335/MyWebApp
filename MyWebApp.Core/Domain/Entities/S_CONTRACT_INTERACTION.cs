﻿using System;
using System.Collections.Generic;

namespace MyWebApp.Core.Domain.Entities;

public partial class S_CONTRACT_INTERACTION
{
    public string CONTRACT_NO { get; set; } = null!;

    public int INTERACTION_ID { get; set; }

    public DateTime? CREATE_DATE { get; set; }

    public string? INTERACTION_NOTES { get; set; }

    public string? INTERACTION_TYPE { get; set; }

    public string? INTERACTION_SUB_TYPE { get; set; }

    public string? CREATE_USER { get; set; }

    public DateTime? DATA_IMPORT_DATE { get; set; }
}
