// added this service page
// added by - Vaishnavi
// added date - 08-12-2025

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


// ----------- FRAN INTERFACE -----------
// Added by: Nishanth`
// Added on: 12-12-2025
export interface Fran {
  fran: string;
  name: string;
  nameAr?: string;
  saasCustomerId?: string;
  customerCurrency?: string;
  paramValue?: string;   // nature of business
  paramDesc?: string;
  natureOfBusiness?: string;
  vatEnabled: boolean;

}

interface LoadParam {
  paramValue: string;
  paramDesc: string;
}



@Injectable({
  providedIn: 'root'
})
export class ApiService {

  // 🔹 Base API URL
  private baseUrl = 'https://localhost:7231/api/v1';

  constructor(private http: HttpClient) { }

  // ----------- LOGIN ----------
  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/master/Users/login`,
      data
    );
  }

  // ================= FRANCHISE (FRAN) =================

  // ----------- GET ALL FRAN -----------
  getAllFrans(): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/master/Franchises`
    );
  }
  // Added by: Nishanth
  // Added on: 04-02-2026
  getFranDropdown(): Observable<FranDropdown[]> {
    return this.http.get<FranDropdown[]>(
      `${this.baseUrl}/master/Franchises`
    );
  }
  // Added by: Nishanth
  // Added on: 05-02-2026
  

  getFranchises() {
    return this.http.get<Fran[]>(
      `${this.baseUrl}/master/franchises`
    );
  }

  getCurrencies() {
    return this.http.get<Currency[]>(
      `${this.baseUrl}/master/Currencies`
    );
  }

  // ----------- CREATE FRAN -----------
  createFran(data: Fran): Observable<any> {

    const payload = {
      fran: data.fran,
      name: data.name,
      nameAr: data.nameAr,
      customerCurrency: data.customerCurrency, // 🔥 map correctly
      natureOfBusiness: data.natureOfBusiness,
      vatEnabled: data.vatEnabled
    };

    return this.http.post(
      `${this.baseUrl}/master/Franchises`,
      payload
    );
  }

  // ----------- UPDATE FRAN -----------
  updateFran(franCode: string, data: Fran): Observable<any> {

    const payload = {
      fran: franCode,
      name: data.name,
      nameAr: data.nameAr,
      customerCurrency: data.customerCurrency, // 🔥 map correctly
      natureOfBusiness: data.natureOfBusiness,
      vatEnabled: data.vatEnabled
    };

    return this.http.put(
      `${this.baseUrl}/master/Franchises/${franCode}`,
      payload
    );
  }

  // ----------- DELETE FRAN -----------
  deleteFran(franCode: string): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/master/Franchises/${franCode}`
    );
  }



  // ===================== MASTER =====================

  // ----------- MAKE LIST -----------
  getMakeList(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/master/Makes/GetMake`);
  }


  // ----------- GET ALL PARTS -----------
  getAllParts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/master/Parts/Getparts`);
  }

  // ----------- ADD PART -----------
  addPart(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/master/Parts/add-part`, data);
  }

  // ----------- CUSTOMER MASTER -----------

  getAllCustomers(): Observable<Customer[]> {
    return this.http.get<Customer[]>(`${this.baseUrl}/master/customers`);
  }

  getCustomerByCode(customerCode: string): Observable<Customer> {
    return this.http.get<Customer>(
      `${this.baseUrl}/master/customers/${customerCode}`
    );
  }

  addCustomer(customer: Customer): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/master/customers`,
      customer
    );
  }

  updateCustomer(customerCode: string, customer: Customer): Observable<any> {
    if (!customer.updateBy) {
      customer.updateBy = 'api';
    }
    if (!customer.updateRemarks) {
      customer.updateRemarks = '';
    }

    return this.http.put(
      `${this.baseUrl}/master/customers/${customerCode}`,
      customer,
      { observe: 'response' }
    );
  }

  deleteCustomer(customerCode: string): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/master/customers/${customerCode}`,
      { observe: 'response' }
    );
  }

  // ===================== FINANCE =====================

  // ----------- DROPDOWN PARAMS -----------
  getParams(fran: string, type: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/finance/params/load/${fran}/${type}`
    );
  }

  // ----------- ACCOUNTS RECEIVABLE ----------
  getReceivables(fran: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/orders/sales/receivable`,
      { params: { fran } }
    );
  }

  // ----------- ACCOUNTS PAYABLE ----------
  getPayables(fran: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/orders/sales/payable`,
      { params: { fran } }
    );
  }

  // ----------- INVOICE AP ----------
  getInvoiceAP(fran: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/SALEHDRs/InvoiceAP/${fran}`
    );
  }

  // ----------- INVOICE AR ----------
  getInvoiceAR(fran: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/SALEHDRs/InvoiceAR/${fran}`
    );
  }

  // ----------- PAYMENT / JOURNAL ENTRY ----------
  addJournalEntry(data: {
    customer: string;
    saleNo: string;
    billAmount: number;
    paymentMethod: string;
    cardNumber?: string;
    remarks?: string;
  }): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/finance/JournalEntries/InsertJournal`,
      data
    );
  }

  // ===================== EXPENSE =====================

  // ----------- ADD EXPENSE ----------
  addjournalEntry(data: any): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/EXPENSE/Add`,
      data
    );
  }

  // ----------- GET ALL EXPENSES ----------
  getAllJournal(): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/EXPENSE/GetAll`
    );
  }

  // ===================== SALES =====================

  // ----------- SAVE SALE INVOICE ----------
  saveSaleInvoice(data: any): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/SaveSaleInvoice`,
      data
    );
  }

  // ===================== SUPPLIER MASTER =====================

  // ----------- GET ALL SUPPLIERS -----------
  getSuppliers(): Observable<Supplier[]> {
    return this.http.get<Supplier[]>(
      `${this.baseUrl}/master/Suppliers`
    );
  }

  // ----------- GET SUPPLIER BY CODE -----------
  getSupplierByCode(supplierCode: string): Observable<Supplier> {
    return this.http.get<Supplier>(
      `${this.baseUrl}/master/Suppliers/${supplierCode}`
    );
  }

  // ----------- CREATE SUPPLIER -----------
  createSupplier(data: Supplier): Observable<Supplier> {
    return this.http.post<Supplier>(
      `${this.baseUrl}/master/Suppliers`,
      data
    );
  }

  // ----------- UPDATE SUPPLIER -----------
  updateSupplier(
    supplierCode: string,
    data: Supplier
  ): Observable<Supplier> {
    return this.http.put<Supplier>(
      `${this.baseUrl}/master/Suppliers/${supplierCode}`,
      data
    );
  }

  // ----------- DELETE SUPPLIER -----------
  deleteSupplier(supplierCode: string): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/master/Suppliers/${supplierCode}`,
      { observe: 'response' }
    );
  }

  // ===================== PURCHASE ORDER / PO INQUIRY =====================

  // ----------- GET ALL PURCHASE ORDER HEADERS -----------
  getAllPurchaseOrders(): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/orders/PurchaseOrders`
    );
  }

  // ----------- GET PO BY COMPOSITE KEY -----------
  getPurchaseOrderByKey(
    fran: string,
    branch: string,
    warehouse: string,
    poType: string,
    poNumber: string
  ): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/orders/PurchaseOrders/${fran}/${branch}/${warehouse}/${poType}/${poNumber}`
    );
  }

  // ----------- DELETE PURCHASE ORDER -----------
  deletePurchaseOrder(
    fran: string,
    poNumber: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/orders/PurchaseOrders/${fran}/${poNumber}`
    );
  }

  // ===================== PURCHASE ORDER =====================

  // ----------- CREATE PURCHASE ORDER -----------
  createPurchaseOrder(dto: any): Observable<any> {
    return this.http.post<any>(
      `${this.baseUrl}/orders/PurchaseOrders`,
      dto
    );
  }

  // ----------- UPDATE PURCHASE ORDER -----------
  updatePurchaseOrder(
    fran: string,
    poNumber: string,
    supplier: string,
    dto: any
  ): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/orders/PurchaseOrders/${fran}/${poNumber}/${supplier}`,
      dto,
      { responseType: 'text' }
    );
  }

  // ----------- DELETE PURCHASE ORDER (FULL KEY) -----------
  deletePurchaseOrderByKey(
    fran: string,
    branch: string,
    warehouse: string,
    poType: string,
    poNumber: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/orders/PurchaseOrders/${fran}/${branch}/${warehouse}/${poType}/${poNumber}`
    );
  }

}


// ===================== CUSTOMER MODELS =====================
export interface Customer {
  customerCode: string;
  id?: number;
  name: string;
  nameAr: string;
  phone: string;
  email: string;
  address: string;
  vatNo: string;
  createdDate?: string;
  updateDate?: string;
  updateBy?: string;
  updateRemarks?: string;
}

// ----------- LOGIN MODELS -----------
export interface LoginRequest {
  userId: string;
  password: string;
}

export interface LoginResponse {
  fran: string;
  flag: string;
}

// ----------- SUPPLIER MODELS -----------
export interface Supplier {
  id?: number;
  supplierCode: string;
  supplierName: string;
  arabicName?: string;
  phone?: string;
  email?: string;
  address?: string;
  vatNo?: string;
}

// ----------- PO INQUIRY MODELS -----------
export interface PoInquiry {
  id: number;
  franCode: string;
  branchCode: string;
  whCode: string;
  supplier: string;
  supplierName: string;
  pono: string;
  seqno: string;
  noOfItems: number;
  totalValue: number;
  discount: number;
  vat: number;
  createdt: string;
  phone?: string;
}

// ----------- PURCHASE ORDER MODELS -----------
export interface PurchaseOrderDTO {
  header: HeaderDTO;
  details: DetailDTO[];
}

export interface HeaderDTO {
  franCode: string;
  branchCode: string;
  whCode: string;
  vendorCode: string;

  potype: string;
  pono: string;
  vendorrefno: string;

  currency: string;

  noofitems: number;
  discount: number;
  totalvalue: number;

  createdt: string;
  createby: string;
  createremarks: string;
}

export interface DetailDTO {
  franCode: string;
  branchCode: string;
  whCode: string;
  vendorCode: string;

  potype: string;
  pono: string;
  posrl: string;

  plantype: string;
  planno: number;
  plansrl: number;

  make: string;
  part: number;
  qty: number;
  unitprice: number;

  discount: number;
  vatpercentage: number;
  vatvalue: number;
  discountValue: number;
  totalValue: number;

  createdt: string;
  createby: string;
}
  //added by : Nishanth //added on : 04-02-2026



//added by : Nishanth //added on : 04-02-2026

export interface FranDropdown {
  saasCustomerId: string;
}


export interface Currency {
  currencyCode: string;
  baseCurrency: string;
  factor1: number;
  factor2: number;
  decimalPlace: number;

}


