import { Component, OnInit } from '@angular/core';
import { Customer } from './customer.model';
import { CustomerService } from './customer.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {

  customer: Customer = this.getEmptyCustomer();

  customers: Customer[] = [];
  filteredCustomers: Customer[] = [];

  isEditing = false;
  searchText = '';
  originalCustomerName = '';
  arabicManuallyEdited = false;

  constructor(private customerService: CustomerService) { }

  ngOnInit(): void {
    this.loadCustomers();
  }

  // 🔹 Centralized empty model
  getEmptyCustomer(): Customer {
    return {
      customerCode: '',
      name: '',
      nameAr: '',
      phone: '',
      email: '',
      address: '',
      vatNo: ''
    };
  }

  // 🔹 Load customers
  loadCustomers(): void {
    this.customerService.getCustomers().subscribe({
      next: (data) => {
        this.customers = data || [];
        this.applyFilter();
      },
      error: (err) => {
        alert(err.error?.message || 'Error fetching customers');
      }
    });
  }

  // 🔹 Save / Update
  onSubmit(): void {
    if (!this.customer.customerCode?.trim() || !this.customer.name?.trim()) {
      alert('Customer Code and Name are required!');
      return;
    }

    console.log(this.customer);
    const apiCall$ = this.isEditing
      ? this.customerService.updateCustomer(this.customer.customerCode, this.customer)
      : this.customerService.createCustomer(this.customer);

    apiCall$.subscribe({
      next: () => {
        alert(`Customer ${this.isEditing ? 'updated' : 'created'} successfully`);
        this.resetForm();
        this.loadCustomers();
      },
      error: (err) => {
        alert(err.error?.message || 'Error saving customer');
      }
    });
  }

  // 🔹 Edit
  onEdit(v: Customer): void {
    this.customer = { ...v };
    this.originalCustomerName = v.name;
    this.arabicManuallyEdited = false;
    this.isEditing = true;
  }

  // 🔹 Delete
  onDelete(customerCode: string): void {
    if (!customerCode) return;

    if (confirm('Are you sure you want to delete this customer?')) {
      this.customerService.deleteCustomer(customerCode).subscribe({
        next: () => {
          this.loadCustomers();
          this.resetForm();
        },
        error: (err) => {
          alert(err.error?.message || 'Error deleting customer');
        }
      });
    }
  }

  // 🔹 Reset form
  resetForm(): void {
    this.customer = this.getEmptyCustomer();
    this.isEditing = false;
  }

  // 🔹 Search
  applyFilter(): void {
    const search = this.searchText.toLowerCase().trim();

    this.filteredCustomers = this.customers.filter(v =>
      v.customerCode.toLowerCase().includes(search) ||
      v.name.toLowerCase().includes(search)
    );
  }

  onClear(): void {
    this.originalCustomerName = '';
    this.arabicManuallyEdited = false;
    this.resetForm();
  }

  // 🔹 Arabic conversion
  onCustomerNameChange(value: string): void {
    if (!value) return;

    const nameChanged = value !== this.originalCustomerName;

    if ((!this.isEditing || nameChanged) && !this.arabicManuallyEdited) {
      this.customer.nameAr = this.toArabic(value);
    }
  }

  toArabic(text: string): string {
    const map: { [key: string]: string } = {
      a: 'ا', b: 'ب', t: 'ت', j: 'ج', h: 'ح',
      d: 'د', r: 'ر', s: 'س', f: 'ف',
      k: 'ك', l: 'ل', m: 'م', n: 'ن',
      w: 'و', y: 'ي'
    };

    return text
      .toLowerCase()
      .split('')
      .map(c => map[c] || c)
      .join('');
  }
}
