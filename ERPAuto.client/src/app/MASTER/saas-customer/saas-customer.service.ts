import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SaasCustomer } from './saas-customer.model';

@Injectable({
  providedIn: 'root'
})
export class SaasCustomerService {

  // 🔹 Adjust URL if your controller route is different
  private baseUrl = 'https://localhost:7231/api/v1/master/SaasCustomers';

  constructor(private http: HttpClient) { }

  // 🔹 Get all customers
  getCustomers(): Observable<SaasCustomer[]> {
    return this.http.get<SaasCustomer[]>(this.baseUrl);
  }

  // 🔹 Create new customer
  createCustomer(customer: SaasCustomer): Observable<SaasCustomer> {
    return this.http.post<SaasCustomer>(this.baseUrl, customer);
  }

  // 🔹 Update customer
  updateCustomer(customer: SaasCustomer): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/${customer.saasCustomerId}`,
      customer
    );
  }

  // 🔹 Delete customer
  deleteCustomer(saasCustomerId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${saasCustomerId}`
    );
  }

  getNextCustomerId(): Observable<string> {
    return this.http.get(
      `${this.baseUrl}/next-id`,
      { responseType: 'text' }
    );
  }

  // 🔹 Get customer by ID (optional but recommended)
  getCustomerById(saasCustomerId: string): Observable<SaasCustomer> {
    return this.http.get<SaasCustomer>(
      `${this.baseUrl}/${saasCustomerId}`
    );
  }
}
