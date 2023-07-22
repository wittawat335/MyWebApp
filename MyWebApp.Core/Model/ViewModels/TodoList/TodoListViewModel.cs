﻿using MyWebApp.Core.Domain.Entities;

namespace MyWebApp.Core.Model.ViewModels.TodoList
{
    public class TodoListViewModel
    {
        public int popupCancelLegal { get; set; }
        public string DefaultCase { get; set; }
        public string CaseCode { get; set; }
        public bool DocFlag { get; set; }
        public string OACode { get; set; }
        public List<M_OA> OAList { get; set; }
        public List<M_USER> AdminList { get; set; }
        public List<M_LEGAL_CASE> LegalCaseList { get; set; }
        public List<TrafficType> TrafficList { get; set; }
        public List<RefNo> RefNoList { get; set; }
        public TodoListViewModel()
        {
            RefNoList = new List<RefNo>();
            OAList = new List<M_OA>();
            AdminList = new List<M_USER>();
            LegalCaseList = new List<M_LEGAL_CASE>();
            TrafficList = new List<TrafficType>();
        }
    }
    public class RefNo
    {
        public string? JOB_CASE_CODE { get; set; }
        public string? JOB_CASE_NAME { get; set; }
        public string? JOB_CASE_COLOR { get; set; }
        public string? JOB_LEGAL_STATUS { get; set; }
        public string? JOB_LEGAL_STATUS_NAME { get; set; }
        public string? JOB_REPO_STATUS { get; set; }
        public string? JOB_REPO_STATUS_NAME { get; set; }
        public string? JOB_CASE_STATUS_COLOR { get; set; }
        public string? JOB_ID { get; set; }
        public string? JOB_CONTRACT_NO { get; set; }
        public string? JOB_CUST_NAME { get; set; }
        public string? JOB_BRAND { get; set; }
        public string? JOB_MODEL { get; set; }
        public string? JOB_PLATE_NO { get; set; }
        public decimal? OUTSTANDING_BALANCE { get; set; }
        public decimal? OVERDUE_DAY { get; set; }
        public string? JOB_ADMIN_CODE { get; set; }
        public string? JOB_ADMIN_NAME { get; set; }
        public DateTime? JOB_ASSIGN_ADMIN_DATE { get; set; }
        public string? JOB_OA_CODE { get; set; }
        public string? JOB_OA_NAME { get; set; }
        public DateTime? JOB_ASSIGN_OA_DATE { get; set; }
        public DateTime? JOB_STEP_DATE { get; set; }
        public int? JOB_HANDLE_DAY { get; set; }
        public char? JOB_DOC_FLAG { get; set; }
        public decimal? JOB_SHORT_FALL { get; set; }
        public decimal? JOB_AMOUNT_CLAIMED { get; set; }
        public string? CASE_TYPE { get; set; }
    }
    public class SP_SEARCH_CUSTOMER_Result
    {
        public string CUST_CODE { get; set; }
        public string CUST_NAME { get; set; }
    }
    public class TrafficType
    {
        public string CODE { get; set; }
        public string TEXT { get; set; }
    }
}
