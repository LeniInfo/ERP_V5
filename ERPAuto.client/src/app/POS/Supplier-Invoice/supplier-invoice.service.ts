import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SupplierInvoiceService {

  private baseUrl = 'https://localhost:7231/api/v1/orders/Shipments';

  constructor(private http: HttpClient) { }

  // 🔹 Get shipment (header + details)
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

  // 🔹 CREATE (Header + Details)
  createShipment(payload: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, payload);
  }

  // 🔹 UPDATE (Header + Details)
  updateShipment(
    fran: string,
    branch: string,
    warehouseCode: string,
    shipmentType: string,
    shipmentNumber: string,
    payload: any
  ): Observable<any> {
    return this.http.put<any>(
      `${this.baseUrl}/${fran}/${branch}/${warehouseCode}/${shipmentType}/${shipmentNumber}`,
      payload
    );
  }

  // 🔹 DELETE shipment (optional)
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
