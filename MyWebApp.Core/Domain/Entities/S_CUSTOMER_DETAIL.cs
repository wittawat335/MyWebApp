﻿using System;
using System.Collections.Generic;

namespace MyWebApp.Core.Domain.Entities;

public partial class S_CUSTOMER_DETAIL
{
    public string CUSTOMER_CODE { get; set; } = null!;

    public string? CUSTOMER_TYPE { get; set; }

    public string? CUSTOMER_SUBTYPE_CD { get; set; }

    public string? TAX_NUMBER { get; set; }

    public string? ID_NUMBER { get; set; }

    public string? COMMUNICATION_METHOD_CD { get; set; }

    public string? C_TYPE_CD { get; set; }

    public bool? CUSTOMER_DEALER_FG { get; set; }

    public DateTime? DATE_OF_BIRTH { get; set; }

    public string? GENDER { get; set; }

    public string? MARITAL_STATUS_CD { get; set; }

    public string? FIRST_NAME { get; set; }

    public string? LAST_NAME { get; set; }

    public string? FIRST_NAME_LCL { get; set; }

    public string? LAST_NAME_LCL { get; set; }

    public string? ALIAS_NAME { get; set; }

    public string? ALIAS_NAME_LCL { get; set; }

    public string? CONTACT_NAME { get; set; }

    public string? OCCUPATION { get; set; }

    public string? BIRTHDAY_TYPE_CD { get; set; }

    public string? ID_TYPE_CD { get; set; }

    public string? EMPLOY_STATUS_CD { get; set; }

    public string? EMPLOYER_NAME { get; set; }

    public string? BUSINESS_TYPE_CD { get; set; }

    public string? BUSINESS_DETAIL { get; set; }

    public string? JOB_TITLE { get; set; }

    public string? EMPLOYER_PHONE { get; set; }

    public string? COMPANY_NAME { get; set; }

    public string? COMPANY_NAME_LCL { get; set; }

    public string? COMPANY_TYPE_CD { get; set; }

    public string? BUSINESS_CAT_CD { get; set; }

    public int? ESTABLISHED_YEAR { get; set; }

    public string? PRESIDENT_FULL_NAME { get; set; }

    public string? PASSPORT_ID { get; set; }

    public string? VOTER_ID { get; set; }

    public string? TITLE_TYPE_CD { get; set; }

    public string? FIRST_NAME_SEARCH { get; set; }

    public string? ID_NUMBER_SEARCH { get; set; }

    public string? NATIONALITY { get; set; }

    public DateTime? INCORPORATION_DATE { get; set; }

    public bool? FLEET_CUSTOMER_FLG { get; set; }

    public string? NOTES { get; set; }

    public string? STATUS_CD { get; set; }

    public DateTime? UPDATE_DATE { get; set; }

    public string? TITLE_DESC { get; set; }

    public string? TITLE_DESC_LCL { get; set; }

    public string? TAX_ID { get; set; }

    public DateTime? CREATE_DATE { get; set; }

    public int? PARTNER_ID { get; set; }

    public bool? DPP_FLG { get; set; }

    public string? COMMENTS { get; set; }

    public string? METHOD_OF_COMMUNICATATION { get; set; }

    public string? CUSTOMER_EMAIL { get; set; }

    public string? SEND_SMS { get; set; }

    public string? COMP_CODE { get; set; }

    public string? BRANCH_CODE { get; set; }
}
