<<<<<<< HEAD
import { Routes } from '@angular/router';
import { FranComponent } from './MASTER/Fran/fran.component';
import { PartComponent } from './MASTER/part-master.component/part-master.component';
import { MasterComponent } from './MASTER/Master_Page/master.component';
import { InquiryComponent } from './MASTER/Inquiry/inquiry.component';
import { AccountReceivable } from './ACCOUNTS/AccountReceivable/accountreceivable.component';
import { AccountPayable } from './ACCOUNTS/AccountPayable/accountpayable.component';
//import { InvoiceAP } from './ACCOUNTS/Invoice-AP/invoice-ap.component';
//import { InvoiceAR } from './ACCOUNTS/Invoice-AR/invoice-ar.component';
import { Payment } from './ACCOUNTS/Payment/payment.component';
import { PickSlipComponent } from './MASTER/pickslip/pickslip.component';
import { PickslipInquiryComponent } from './MASTER/pickslip-inquiry/pickslip-inquiry';
import { JournalEntryComponent } from './ACCOUNTS/Journal-Entry/journal-entry.component';
import { JournalComponent } from './ACCOUNTS/Journal/journal.component';
import { SaleInvoiceComponent } from './SALES/Saleinvoice/sale-invoice.component';
import { WorkshopComponent } from './MASTER/Workshop/workshop.component';
import { CustomerComponent } from './MASTER/Customer/customer.component';
import { MasterTypeComponent } from './MASTER/mastertype/mastertype.component';
import { WorkMasterComponent } from './MASTER/Work-Master/workmaster.component';
import { ChartOfAccountComponent } from './ACCOUNTS/Chart_of_Account/chart_of_account.component';
import { WorkOrderComponent } from './MASTER/Work_Order/work_order.component';
import { LoginComponent } from './MASTER/Login/login.component';
import { SupplierComponent } from './MASTER/Supplier/supplier.component';
import { SaasCustomerComponent } from './MASTER/saas-customer/saas-customer.component';
import { PurchaseOrderComponent } from './POS/purchase-order/purchase-order.component';
import { PoInquiryComponent } from './POS/po-inquiry/po-inquiry.component';
import { CustomerOrderComponent } from './MASTER/CustomerOrder/customer-order.component';
import { CustomerOrderListComponent } from './MASTER/CustomerOrder/customer-order-list.component';
// customer web portal 
//import { Registration } from './registration/registration';
//import { ViscardComponent } from './viscard/viscard';

export const routes: Routes = [
  /* 🔹 Standalone page (NO master layout) */
  //{ path: 'register', component: Registration },
  { path: 'login', component: LoginComponent },
  { path: 'workorder', component: WorkOrderComponent },
  { path: 'master', component: MasterComponent },
  //{ path: 'viscard', component: ViscardComponent },


  /* 🔹 Pages that use Master layout */
  {
    path: '', component: MasterComponent,

    children: [
      { path: 'fran', component: FranComponent },
      { path: 'part', component: PartComponent },
      { path: 'inquiry', component: InquiryComponent },
      { path: 'accountreceivable', component: AccountReceivable },
      { path: 'accountpayable', component: AccountPayable },
      //{ path: 'invoiceap', component: InvoiceAP },
      //{ path: 'invoicear', component: InvoiceAR },
      { path: 'payment', component: Payment },
      { path: 'pickslip-entry', component: PickSlipComponent },
      { path: 'pickslip-inquiry', component: PickslipInquiryComponent },
      { path: 'journal-entry', component: JournalEntryComponent },
      { path: 'journal', component: JournalComponent },
      { path: 'saleinvoice', component: SaleInvoiceComponent },
      { path: 'workshop', component: WorkshopComponent },
      { path: 'customer', component: CustomerComponent },
      { path: 'mastertype', component: MasterTypeComponent },
      { path: 'workmaster', component: WorkMasterComponent },
      { path: 'chartofaccount', component: ChartOfAccountComponent },
      { path: 'supplier', component: SupplierComponent },
      { path: 'saas-customer', component: SaasCustomerComponent },
      { path: 'purchase-order', component: PurchaseOrderComponent },
      { path: 'po-inquiry', component: PoInquiryComponent },
      { path: 'customer-order-list', component: CustomerOrderListComponent },
      { path: 'customer-order-form', component: CustomerOrderComponent },
      { path: 'customer-order', component: CustomerOrderListComponent }
      //customer web portal


    ]
  }
];

=======
import { Routes } from '@angular/router';

/* 🔹 MASTER */
import { MasterComponent } from './MASTER/Master_Page/master.component';
import { LoginComponent } from './MASTER/Login/login.component';
import { FranComponent } from './MASTER/Fran/fran.component';
import { PartComponent } from './MASTER/part-master.component/part-master.component';
import { InquiryComponent } from './MASTER/Inquiry/inquiry.component';
import { PickSlipComponent } from './MASTER/pickslip/pickslip.component';
import { PickslipInquiryComponent } from './MASTER/pickslip-inquiry/pickslip-inquiry';
import { WorkshopComponent } from './MASTER/Workshop/workshop.component';
import { CustomerComponent } from './MASTER/Customer/customer.component';
import { CustomerFormComponent } from './MASTER/Customer/customer-form.component';
import { WorkmasterComponent } from './MASTER/Work-Master/workmaster.component';
import { SupplierComponent } from './MASTER/Supplier/supplier.component';
import { WorkOrderComponent } from './MASTER/Work_Order/work_order.component';

/* 🔹 ACCOUNTS */
import { AccountReceivable } from './ACCOUNTS/AccountReceivable/accountreceivable.component';
import { AccountPayable } from './ACCOUNTS/AccountPayable/accountpayable.component';
import { Payment } from './ACCOUNTS/Payment/payment.component';
import { JournalEntryComponent } from './ACCOUNTS/Journal-Entry/journal-entry.component';
import { JournalComponent } from './ACCOUNTS/Journal/journal.component';
import { ChartOfAccountComponent } from './ACCOUNTS/Chart_of_Account/chart_of_account.component';

/* 🔹 SALES */
import { SaleInvoiceComponent } from './SALES/Saleinvoice/sale-invoice.component';
import { CustomerOrderComponent } from './SALES/CustomerOrder/customer-order.component';
import { CustomerOrderListComponent } from './SALES/CustomerOrder/customer-order-list.component';

/* 🔹 POS */
import { PurchaseOrderComponent } from './POS/purchase-order/purchase-order.component';
import { PoInquiryComponent } from './POS/po-inquiry/po-inquiry.component';
import { GrnComponent } from './POS/grn/grn.component';
import { SupplierInvoiceComponent } from './POS/Supplier-Invoice/supplier-invoice.component';
import { SupplierInvoiceInquiryComponent } from './POS/Supplier-Invoice-Inquiry/supplier-invoice-inquiry.component';

/* 🔹 JOB ORDER */
import { WorkOrderInquiryComponent } from './JOB_ORDER/work-order-inquiry/work-order-inquiry.component';
import { WorkOrderInvoiceComponent } from './JOB_ORDER/work-order-invoice/work-order-invoice.component';
import { WorkOrderInvoiceInquiryComponent } from './JOB_ORDER/work-order-invoice-inquiry/work-order-invoice-inquiry.component';

/* 🔹 WORK ORDER (WO) */
import { WorkQuotationListComponent } from './WO/work-quotation/work-quotation-list.component';
import { WorkQuotationComponent } from './WO/work-quotation/work-quotation.component';
import { WorkEnquiryComponent } from './WO/work-enquiry/work-enquiry.component';
import { WorkEnquiryListComponent } from './WO/work-enquiry/work-enquiry-list.component';

export const routes: Routes = [

  /* 🔹 Standalone (No master layout) */
  { path: 'login', component: LoginComponent },
  { path: 'workorder', component: WorkOrderComponent },

  /* 🔹 Pages WITH Master layout */
  {
    path: '',
    component: MasterComponent,
    children: [
      /* Default route - redirect to fran (no customer redirect) */
      { path: '', redirectTo: 'fran', pathMatch: 'full' },

      /* MASTER */
      { path: 'fran', component: FranComponent },
      { path: 'part', component: PartComponent },
      { path: 'inquiry', component: InquiryComponent },
      { path: 'pickslip-entry', component: PickSlipComponent },
      { path: 'pickslip-inquiry', component: PickslipInquiryComponent },
      { path: 'workshop', component: WorkshopComponent },
      { path: 'workmaster', component: WorkmasterComponent },
      { path: 'supplier', component: SupplierComponent },

      /* CUSTOMER */
      { path: 'customer', component: CustomerComponent },
      { path: 'customer/add', component: CustomerFormComponent },
      { path: 'customer/edit/:customerCode', component: CustomerFormComponent },

      /* ACCOUNTS */
      { path: 'accountreceivable', component: AccountReceivable },
      { path: 'accountpayable', component: AccountPayable },
      { path: 'payment', component: Payment },
      { path: 'journal-entry', component: JournalEntryComponent },
      { path: 'journal', component: JournalComponent },
      { path: 'chartofaccount', component: ChartOfAccountComponent },

      /* SALES */
      { path: 'saleinvoice', component: SaleInvoiceComponent },
      { path: 'customer-order-list', component: CustomerOrderListComponent },
      { path: 'customer-order-form', component: CustomerOrderComponent },
      { path: 'customer-order', component: CustomerOrderListComponent },

      /* POS */
      { path: 'purchase-order', component: PurchaseOrderComponent },
      { path: 'po-inquiry', component: PoInquiryComponent },
      { path: 'supplier-invoice', component: SupplierInvoiceComponent },
      { path: 'supplier-invoice-inquiry', component: SupplierInvoiceInquiryComponent },
      { path: 'GRN', component: GrnComponent },

      /* 🔹 WORK ORDER */
      { path: 'work-order', component: WorkOrderComponent },
      { path: 'work-order-inquiry', component: WorkOrderInquiryComponent },
      { path: 'work-order-invoice', component: WorkOrderInvoiceComponent },
      { path: 'work-order-invoice-inquiry', component: WorkOrderInvoiceInquiryComponent },

      /* 🔹 WORK QUOTATION */
      { path: 'work-quotation-list', component: WorkQuotationListComponent },
      { path: 'work-quotation-form', component: WorkQuotationComponent },

      /* 🔹 WORK ENQUIRY */
      { path: 'work-enquiry-list', component: WorkEnquiryListComponent },
      { path: 'work-enquiry', component: WorkEnquiryComponent }
    ]
  },

  /* 🔹 Default */
  { path: '**', redirectTo: 'login' }
];
>>>>>>> bf4536ad25e42144e3814695c42fad2bb6622520
