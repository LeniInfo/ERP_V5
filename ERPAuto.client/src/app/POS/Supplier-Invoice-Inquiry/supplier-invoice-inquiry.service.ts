import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SupplierInvoiceInquiry } from './supplier-invoice-inquiry.model';

@Injectable({
  providedIn: 'root',
})
export class SupplierInvoiceInquiryService {

  private baseUrl = 'https://localhost:7231/api/v1/orders/Shipments'; // <-- Change to your API controller

  constructor(private http: HttpClient) { }

  // 🔹 Get all SINVHDR records
  getAllInvoices(): Observable<SupplierInvoiceInquiry[]> {
    return this.http.get<SupplierInvoiceInquiry[]>(`${this.baseUrl}`);
  }

  // 🔹 Get header + detail by DOCNO
  getShipment(
    fran: string,
    branch: string,
    warehouseCode: string,
    shipmentType: string,
    shipmentNumber: string
  ): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/${fran}/${branch}/${warehouseCode}/${shipmentType}/${shipmentNumber}`
    );
  }

  deleteShipment(
    fran: string,
    branch: string,
    warehouseCode: string,
    shipmentType: string,
    shipmentNumber: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${fran}/${branch}/${warehouseCode}/${shipmentType}/${shipmentNumber}`
    );
  }

}
