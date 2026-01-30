using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Contracts.Utilities
{
    public class Constant
    {
    }

    // 19-12-2025 Changes jegan
    public class Constant_General()
    {
        public static string Blank { get; set; } = "";
        public static string Yes { get; set; } = "Y";
        public static string No { get; set; } = "N";
        public static string Default_Date { get; set; } = "01/01/1900";

        public static string Zero_String { get; set; } = "0";

        public static int Zero { get; set; } = 0;
        public static int One { get; set; } = 1;
    }

    public class Constant_Params()
    {
        public static string PARTS_CATEGORY { get; set; } = "PARTS_CATEGORY";
        public static string PARTS_COO { get; set; } = "PARTS_COO";
        public static string PARTS_GROUP { get; set; } = "PARTS_GROUP";
        public static string SALES_CHANNEL { get; set; } = "SALES_CHANNEL";
        public static string Accounts_Type { get; set; } = "ACCOUNTS_TYPE";
        public static string WorkShop_WorkType { get; set; } = "WS_WORKTYPE";

        // DOC TYPES Drop Down
        public static string DocTypes_Customer_Order { get; set; } = "DOCTYPES_CORDHDR";
        public static string DocTypes_SaleInvoice { get; set; } = "DOCTYPES_SALEHDR";
        public static string DocTypes_PO { get; set; } = "DOCTYPES_POHDR";
        public static string DocTypes_SupplierInvoice { get; set; } = "DOCTYPES_SINVHDR";
        public static string DocTypes_RepairInvoice { get; set; } = "DOCTYPES_REPAIRHDR";
        public static string DocTypes_WOrkOrderInvoice { get; set; } = "DOCTYPES_WORKINVHDR";
        public static string DocTypes_OrderPlan { get; set; } = "DOCTYPES_OPLNHDR";
        public static string DocTypes_PackType { get; set; } = "DOCTYPES_PACKHDR";
        public static string DocTypes_CartonType { get; set; } = "DOCTYPES_CRTNHDR";
        public static string DocTypes_PO_B2BType { get; set; } = "DOCTYPES_POB2B";
        public static string DocTypes_Receipt_Type { get; set; } = "DOCTYPES_RECTHDR";

        public static string FLAG_VAT_ENABLED { get; set; } = "VAT_ENABLED";

    }
    public class Constant_DocTypes()
    {
        //// CORDHDR - Customer Order
        public static string CORDHDR_WORD { get; set; } = "WORD";
        public static string CORDHDR_DIRO { get; set; } = "DIRO";
        public static string CORDHDR_RORQ { get; set; } = "RORQ";

        // SALEHDR - Sale Invoice
        public static string SALEHDR_POS { get; set; } = "POS";
        public static string SALEHDR_ROI { get; set; } = "ROI";

        // POHDR - PO Order
        public static string POHDR_STKO { get; set; } = "STKO";
        public static string POHDR_B2BO { get; set; } = "B2BO";

        // SIHDR - Supplier Invoice  
        public static string SIHDR_EDI { get; set; } = "EDI";
        public static string SIHDR_KEY_IN { get; set; } = "KIN";

        // WORK - WorkOrder Invoice  
        public static string WORKINVHDR_SERVICE { get; set; } = "SERV";
    }
    // 19-12-2025 Changes ends jegan

    // 23-12-2025 Changes Jegan
    public class Constant_Sessions()
    {
        public static string LOGIN_FRAN { get; set; } = "LOGIN_FRAN";
        public static string LOGIN_BRCH { get; set; } = "LOGIN_BRCH";
        public static string LOGIN_WHSE { get; set; } = "LOGIN_WHSE";
        public static string LOGIN_WORKSHOP { get; set; } = "LOGIN_WORKSHOP";
        public static string Login_UserId { get; set; } = "LOGIN_USERID";
        public static string Login_Company { get; set; } = "LOGIN_COMPANY";
        public static string User_Email { get; set; } = "USER_EMAIL";
        public static string COMPANY_CURRNCY { get; set; } = "COMPANY_CURRNCY";
    }
    // 23-12-2025 Changes Ends Jegan


}
