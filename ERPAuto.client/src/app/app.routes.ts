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
import { WorkmasterComponent } from './MASTER/Work-Master/workmaster.component';
import { ChartOfAccountComponent } from './ACCOUNTS/Chart_of_Account/chart_of_account.component';
import { WorkOrderComponent } from './MASTER/Work_Order/work_order.component';
import { LoginComponent } from './MASTER/Login/login.component';
import { SupplierComponent } from './MASTER/Supplier/supplier.component';
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
      { path: 'workmaster', component: WorkmasterComponent },
      { path: 'chartofaccount', component: ChartOfAccountComponent },
      { path: 'supplier', component: SupplierComponent },
      { path: 'purchase-order', component: PurchaseOrderComponent },
      { path: 'po-inquiry', component: PoInquiryComponent },
      { path: 'customer-order-list', component: CustomerOrderListComponent },
      { path: 'customer-order-form', component: CustomerOrderComponent },
      { path: 'customer-order', component: CustomerOrderListComponent },

      //customer web portal


    ]
  }
];
