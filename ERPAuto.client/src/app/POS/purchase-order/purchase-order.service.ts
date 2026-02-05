import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PurchaseOrderService {

  private baseUrl = 'https://localhost:7231/api/v1/orders/PurchaseOrders';

  constructor(private http: HttpClient) { }

  // 🔹 GET PO by composite key
  getByKey(
    fran: string,
    branch: string,
    warehouse: string,
    poType: string,
    poNumber: string
  ): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/${fran}/${branch}/${warehouse}/${poType}/${poNumber}`
    );
  }


  // 🔹 CREATE Purchase Order
  createPurchaseOrder(dto: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, dto);
  }

  // 🔹 UPDATE Purchase Order
  updatePurchaseOrder(
    fran: string,
    poNumber: string,
    supplier: string,
    dto: any
  ){
    return this.http.put(
      `${this.baseUrl}/${fran}/${poNumber}/${supplier}`,
      dto, { responseType: 'text' }
    );
  }

  // 🔹 DELETE Purchase Order
  deletePurchaseOrder(
    fran: string,
    branch: string,
    warehouse: string,
    poType: string,
    poNumber: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${fran}/${branch}/${warehouse}/${poType}/${poNumber}`
    );
  }
}
