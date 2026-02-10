import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from './customer.model';

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private baseUrl = 'https://localhost:7231/api/v1/master/Customers';

  constructor(private http: HttpClient) { }

  getNextCustomerCode(): Observable<string> {
    return this.http.get(
      `${this.baseUrl}/next-code`,
      { responseType: 'text' }
    );
  }

  // Get all customers
  getCustomers(): Observable<Customer[]> {
    return this.http.get<Customer[]>(this.baseUrl);
  }

  // Get customer by code
  getCustomerByCode(customerCode: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/${customerCode}`);
  }

  // Create a new customer
  createCustomer(customer: Customer): Observable<Customer> {
    return this.http.post<Customer>(this.baseUrl, customer);
  }

  // Update existing customer
  updateCustomer(customerCode: string, customer: Customer): Observable<Customer> {
    return this.http.put<Customer>(`${this.baseUrl}/${customerCode}`, customer);
  }

  // Delete customer
  deleteCustomer(customerCode: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${customerCode}`);
  }
}

export { Customer };
