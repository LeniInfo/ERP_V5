import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SupplierInvoiceHeader, SupplierInvoiceDetail, GrnSavePayload } from '../grn.model';


@Injectable({
  providedIn: 'root',
})
export class GrnCardService {

  constructor(private http: HttpClient) { }

  private baseUrl = 'https://localhost:7231/api/v1/orders/Receipts';

  createReceipt(payload: any) {
    return this.http.post(
      `${this.baseUrl}/save`,
      payload
    );
  }

}
