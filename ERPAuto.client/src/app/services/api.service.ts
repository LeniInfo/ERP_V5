// added this service page
// added by - Vaishnavi
// added date - 08-12-2025

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


// ----------- FRAN INTERFACE -----------
// Added by: Nishanth
// Added on: 12-12-2025
export interface Fran {
  fran?: string;
  franCode: string;
  name: string;
  nameAr: string;
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

  // ----------- CREATE FRAN -----------
  createFran(data: Fran): Observable<any> {

    const payload = {
      fran: data.franCode,
      name: data.name,
      nameAr: data.nameAr
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
      nameAr: data.nameAr
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

  // ----------- SEARCH PARTS -----------
  searchParts(name: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/master/Parts/search?name=${encodeURIComponent(name)}`
    );
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

  // Search customers by name
  searchCustomers(name: string): Observable<Customer[]> {
    return this.http.get<Customer[]>(
      `${this.baseUrl}/master/customers/search?name=${encodeURIComponent(name)}`
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


  // ----------- WORK ORDER -----------

  getAllWorkOrder(): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/RepairOrder`
    );
  }


  getRepairOrderByKey(
    fran: string,
    brch: string,
    workshop: string,
    repairType: string,
    repairNo: string,
    customer: string
  ) {
    return this.http.get<any>(
      `${this.baseUrl}/RepairOrder/${fran}/${brch}/${workshop}/${repairType}/${repairNo}/${customer}`
    );
  }

  createWorkOrder(dto: any) {
    return this.http.post<any>(
      `${this.baseUrl}/RepairOrder`,
      dto
    );
  }

  updateWorkOrder(dto: any) {
    return this.http.put(
      `${this.baseUrl}/RepairOrder`,
      dto
    );
  }

  deleteWorkOrder(fran: string, customer: string, repairNo: string) {
    return this.http.delete(
      `${this.baseUrl}/RepairOrder/${fran}/${customer}/${repairNo}`
    );
  }

  // ===================== REQUEST HEADER (Work Enquiry) =====================

  // Get all request headers
  getAllRequestHeaders(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/RequestHeader`);
  }

  // Get request header by key
  getRequestHeaderByKey(
    fran: string,
    branch: string,
    warehouse: string,
    requestType: string,
    requestNo: string
  ): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/RequestHeader/${fran}/${branch}/${warehouse}/${requestType}/${requestNo}`
    );
  }

  // Create request header
  createRequestHeader(data: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/RequestHeader`, data);
  }

  // Update request header
  updateRequestHeader(
    fran: string,
    branch: string,
    warehouse: string,
    requestType: string,
    requestNo: string,
    data: any
  ): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/RequestHeader/${fran}/${branch}/${warehouse}/${requestType}/${requestNo}`,
      data
    );
  }

  // Delete request header
  deleteRequestHeader(
    fran: string,
    branch: string,
    warehouse: string,
    requestType: string,
    requestNo: string
  ): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/RequestHeader/${fran}/${branch}/${warehouse}/${requestType}/${requestNo}`
    );
  }

  // Get next request number
  getNextRequestNumber(): Observable<string> {
    return this.http.get(
      `${this.baseUrl}/RequestHeader/next-request-number`,
      { responseType: 'text' }
    ) as Observable<string>;
  }

  // ===================== REQUEST DETAIL =====================

  // Get all request details
  getAllRequestDetails(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/RequestDetail`);
  }

  // Get request detail by key
  getRequestDetailByKey(
    fran: string,
    branch: string,
    warehouse: string,
    requestType: string,
    requestNo: string,
    requestSrl: string
  ): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/RequestDetail/${fran}/${branch}/${warehouse}/${requestType}/${requestNo}/${requestSrl}`
    );
  }

  // Get request details by header
  getRequestDetailsByHeader(
    fran: string,
    branch: string,
    warehouse: string,
    requestType: string,
    requestNo: string
  ): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/RequestDetail/by-header/${fran}/${branch}/${warehouse}/${requestType}/${requestNo}`
    );
  }

  // Create request detail
  createRequestDetail(data: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/RequestDetail`, data);
  }

  // Update request detail
  updateRequestDetail(
    fran: string,
    branch: string,
    warehouse: string,
    requestType: string,
    requestNo: string,
    requestSrl: string,
    data: any
  ): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/RequestDetail/${fran}/${branch}/${warehouse}/${requestType}/${requestNo}/${requestSrl}`,
      data
    );
  }

  // Delete request detail
  deleteRequestDetail(
    fran: string,
    branch: string,
    warehouse: string,
    requestType: string,
    requestNo: string,
    requestSrl: string
  ): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/RequestDetail/${fran}/${branch}/${warehouse}/${requestType}/${requestNo}/${requestSrl}`
    );
  }

  // ===================== CUSTOMER ORDER =====================

  // Get all customer order headers
  getAllCustomerOrderHeaders(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/orders/CustomerOrders/headers`);
  }

  // Get customer order header by key
  getCustomerOrderHeaderByKey(
    fran: string,
    branch: string,
    warehouse: string,
    cordType: string,
    cordNo: string
  ): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/orders/CustomerOrders/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`
    );
  }

  // Create customer order header
  createCustomerOrderHeader(data: any): Observable<any> {
    return this.http.post<any>(
      `${this.baseUrl}/orders/CustomerOrders/headers`,
      data
    );
  }

  // Update customer order header
  updateCustomerOrderHeader(
    fran: string,
    branch: string,
    warehouse: string,
    cordType: string,
    cordNo: string,
    data: any
  ): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/orders/CustomerOrders/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`,
      data
    );
  }

  // Delete customer order header
  deleteCustomerOrderHeader(
    fran: string,
    branch: string,
    warehouse: string,
    cordType: string,
    cordNo: string
  ): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/orders/CustomerOrders/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`
    );
  }

  // Get next customer order number
  getNextCustomerOrderNumber(): Observable<string> {
    return this.http.get(
      `${this.baseUrl}/orders/CustomerOrders/next-order-number`,
      { responseType: 'text' }
    ) as Observable<string>;
  }

  // Get customer order lines by header
  getCustomerOrderLinesByHeader(
    fran: string,
    branch: string,
    warehouse: string,
    cordType: string,
    cordNo: string
  ): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/orders/CustomerOrders/lines/by-header/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`
    );
  }

  // Create customer order line
  createCustomerOrderLine(data: any): Observable<any> {
    return this.http.post<any>(
      `${this.baseUrl}/orders/CustomerOrders/lines`,
      data
    );
  }

  // Update customer order line
  updateCustomerOrderLine(
    fran: string,
    branch: string,
    warehouse: string,
    cordType: string,
    cordNo: string,
    cordSrl: string,
    data: any
  ): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/orders/CustomerOrders/lines/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}/${cordSrl}`,
      data
    );
  }

  // Delete customer order line
  deleteCustomerOrderLine(
    fran: string,
    branch: string,
    warehouse: string,
    cordType: string,
    cordNo: string,
    cordSrl: string
  ): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/orders/CustomerOrders/lines/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}/${cordSrl}`
    );
  }

  // ===================== VEHICLE MASTER =====================

  // Get all vehicles
  getAllVehicles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/VehicleMaster`);
  }

  // Get vehicle by key
  getVehicleByKey(key: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/VehicleMaster/${key}`);
  }

  createVehicle(data: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/VehicleMaster`, data);
  }

  // ===================== WORK MASTER =====================

  // Get all work masters
  getAllWorkMasters(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/WorkMaster`);
  }

  // lookups (deprecated - use getAllWorkMasters instead)
  getCustomers() { return this.getAllCustomers(); }
  getWorks() { return this.getAllWorkMasters(); }

  //ARABIC CONVERSION API
  translate(text: string, from: string, to: string) {
    return this.http.post<any>(`${this.baseUrl}/RepairOrder/translate`, {
      text,
      from,
      to
    });
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
  createBy?: string;
  createRemarks?: string;
  [key: string]: any;
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

