<<<<<<< HEAD
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
=======
import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ApiService, Customer as ApiCustomer } from '../../services/api.service';

// Re-export Customer interface for backward compatibility
export type Customer = ApiCustomer;

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  constructor(private apiService: ApiService) { }

  // Get all customers
  getAll(): Observable<Customer[]> {
    return this.apiService.getAllCustomers().pipe(
      map((response: any) => {
        // Handle different response formats
        if (Array.isArray(response)) {
          return response;
        } else if (response && Array.isArray(response.data)) {
          return response.data;
        } else {
          return [];
        }
      }),
      catchError(this.handleError)
    );
  }

  // Get customer by code
  getByCode(customerCode: string): Observable<Customer> {
    return this.apiService.getCustomerByCode(customerCode).pipe(
      catchError(this.handleError)
    );
  }

  // Create new customer
  create(customer: Customer): Observable<any> {
    // Prepare request matching backend CreateCustomerRequest
    // CustomerCode is optional - backend will auto-generate if not provided
    const createRequest: any = {
      name: customer.name,
      nameAr: customer.nameAr,
      phone: customer.phone,
      email: customer.email,
      address: customer.address,
      vatNo: customer.vatNo
    };
    
    // Only include customerCode if it's provided and not empty
    if (customer.customerCode && customer.customerCode.trim() !== '') {
      createRequest.customerCode = customer.customerCode;
    }

    return this.apiService.addCustomer(createRequest).pipe(
      catchError(this.handleError)
    );
  }

  // Update customer
  update(customer: Customer): Observable<any> {
    if (!customer.customerCode) {
      return throwError(() => ({ message: 'Customer code is required for update' }));
    }

    return this.apiService.updateCustomer(customer.customerCode, customer).pipe(
      catchError(this.handleError)
    );
  }

  // Delete customer
  delete(customerCode: string): Observable<any> {
    return this.apiService.deleteCustomer(customerCode).pipe(
      catchError(this.handleError)
    );
  }

  // Search customers (client-side filtering)
  search(term: string, customers: Customer[]): Customer[] {
    if (!term.trim()) {
      return customers;
    }
    const searchTerm = term.toLowerCase();
    return customers.filter(c =>
      c.name?.toLowerCase().includes(searchTerm) ||
      c.nameAr?.toLowerCase().includes(searchTerm) ||
      c.email?.toLowerCase().includes(searchTerm) ||
      c.phone?.includes(term) ||
      c.address?.toLowerCase().includes(searchTerm) ||
      c.vatNo?.toLowerCase().includes(searchTerm) ||
      c.customerCode?.toLowerCase().includes(searchTerm)
    );
  }

  // Error handler
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error (network error, CORS, etc.)
      errorMessage = `Network Error: ${error.error.message}`;
      console.error('Client-side error:', error.error);
    } else {
      // Server-side error
      console.error('Server error:', {
        status: error.status,
        statusText: error.statusText,
        error: error.error,
        url: error.url
      });

      if (error.status === 0) {
        errorMessage = 'Cannot connect to server. Please ensure the backend API is running.';
      } else if (error.status === 404) {
        errorMessage = 'API endpoint not found. Please check the backend URL.';
      } else if (error.status === 500) {
        errorMessage = error.error?.message || error.error?.details || 'Server error occurred.';
      } else if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.error?.details) {
        errorMessage = error.error.details;
      } else {
        errorMessage = `Error ${error.status}: ${error.statusText || error.message}`;
      }
    }

    console.error('API Error Details:', {
      message: errorMessage,
      status: error.status,
      url: error.url,
      error: error.error
    });

    return throwError(() => ({
      message: errorMessage,
      status: error.status,
      error: error.error
    }));
  }
}
>>>>>>> bf4536ad25e42144e3814695c42fad2bb6622520
