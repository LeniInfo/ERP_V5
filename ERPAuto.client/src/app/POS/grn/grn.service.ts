import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SupplierInvoiceHeader, SupplierInvoiceDetail, GrnSavePayload, PendingReceipt, ShipmentforGRN } from './grn.model';

@Injectable({
  providedIn: 'root'
})
export class GrnService {

  private baseUrl = 'https://localhost:7231/api/v1/grn';

  constructor(private http: HttpClient) { }

  // 🔹 1. Get pending invoices
  getPendingInvoices(): Observable<PendingReceipt[]> {
    return this.http.get<PendingReceipt[]>(`https://localhost:7231/api/v1/orders/receipts/pending-invoices`);
  }

  getHeaderandDetails(shipmentNumber: string): Observable<ShipmentforGRN> {
    return this.http.get<ShipmentforGRN>(
      `https://localhost:7231/api/v1/orders/shipments/header-and-details/${shipmentNumber}`
    );
  }

  // 🔹 3. Save GRN (called from grn-card)
  saveGrn(payload: GrnSavePayload): Observable<any> {
    return this.http.post(`${this.baseUrl}/save`, payload);
  }
}
