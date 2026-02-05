import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { SaasCustomer } from './saas-customer.model';
import { SaasCustomerService } from './saas-customer.service';

@Component({
  selector: 'app-saas-customer',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './saas-customer.component.html',
  styleUrls: ['./saas-customer.component.css']
})
export class SaasCustomerComponent implements OnInit {

  customer: SaasCustomer = this.getEmptyCustomer();

  customers: SaasCustomer[] = [];
  filteredCustomers: SaasCustomer[] = [];

  isEditing = false;
  searchText = '';

  constructor(private customerService: SaasCustomerService) { }

  ngOnInit(): void {
    this.loadNextCustomerId();
    this.loadCustomers();
  }

  // 🔹 Empty Model
  getEmptyCustomer(): SaasCustomer {
    return {
      saasCustomerId: '',
      saasCustomerName: '',
      phone1:'',
      phone2:'',
      email: '',
      address: ''
    };
  }

  loadNextCustomerId(): void {
    this.customerService.getNextCustomerId().subscribe({
      next: (id) => {
        this.customer.saasCustomerId = id;
      },
      error: () => {
        alert('Failed to generate Customer ID');
      }
    });
  }
  // 🔹 Load Customers
  loadCustomers(): void {
    this.customerService.getCustomers().subscribe({
      next: (data) => {
        this.customers = data ?? [];
        this.applyFilter();
      },
      error: (err) => {
        alert(err.error?.message || 'Error fetching SaaS Customers');
      }
    });
  }

  // 🔹 Save / Update
  onSubmit(): void {

    if (!this.customer.saasCustomerName?.trim()) {
      alert('Customer Name is required!');
      return;
    }

    if (this.isEditing) {

      this.customerService.updateCustomer(this.customer).subscribe({
        next: () => {
          alert('Customer updated successfully');
          this.resetForm();
          this.loadCustomers();
        },
        error: (err) => {
          alert(err.error?.message || 'Error updating customer');
        }
      });

    } else {

      this.customerService.createCustomer(this.customer).subscribe({
        next: () => {
          alert('Customer created successfully');
          this.resetForm();
          this.loadCustomers();
        },
        error: (err) => {
          alert(err.error?.message || 'Error creating customer');
        }
      });

    }
  }

  // 🔹 Edit
  onEdit(c: SaasCustomer): void {
    this.customer = { ...c };
    this.isEditing = true;
  }

  // 🔹 Delete
  onDelete(c: SaasCustomer): void {

    if (!c?.saasCustomerId) return;

    this.customerService
      .deleteCustomer(c.saasCustomerId)
      .subscribe(() => {
        this.loadCustomers();
        this.resetForm();
      });
  }

  // 🔹 Reset Form
  resetForm(): void {
    this.customer = this.getEmptyCustomer();
    this.isEditing = false;
    this.loadNextCustomerId();

  }

  // 🔹 Search Filter
  applyFilter(): void {

    const search = this.searchText.toLowerCase().trim();

    this.filteredCustomers = this.customers.filter(c =>
      (c.saasCustomerId ?? '').toLowerCase().includes(search) ||
      (c.saasCustomerName ?? '').toLowerCase().includes(search) ||
      (c.email ?? '').toLowerCase().includes(search)
    );
  }

  // 🔹 Clear Button
  onClear(): void {
    this.resetForm();
  }
}
