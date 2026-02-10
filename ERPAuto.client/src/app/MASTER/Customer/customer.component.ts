<<<<<<< HEAD
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
    this.loadNextCustomerCode();
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

  loadNextCustomerCode(): void {
    this.customerService.getNextCustomerCode().subscribe({
      next: (code) => {
        this.customer.customerCode = code;
      },
      error: () => {
        alert('Failed to generate Customer Code');
      }
    });
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
        this.loadNextCustomerCode();
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
          this.loadNextCustomerCode();
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
    this.loadNextCustomerCode();
  }

  // 🔹 Arabic conversion
  onCustomerNameChange(value: string): void {

    if (!value) {
      this.customer.nameAr = '';
      this.arabicManuallyEdited = false;
      return;
    }

    const nameChanged = value !== this.originalCustomerName;

    if ((!this.isEditing || nameChanged) && !this.arabicManuallyEdited) {
      this.customer.nameAr = this.toArabic(value);
    }
  }


  toArabic(text: string): string {

    const combos: any = {
      sh: 'ش',
      kh: 'خ',
      th: 'ث',
      dh: 'ذ',
      gh: 'غ'
    };

    const singles: any = {
      a: 'ا',
      b: 'ب',
      c: 'ك',
      d: 'د',
      e: 'ي',
      f: 'ف',
      g: 'ج',
      h: 'ه',
      i: 'ي',
      j: 'ج',
      k: 'ك',
      l: 'ل',
      m: 'م',
      n: 'ن',
      o: 'و',
      p: 'ب',
      q: 'ق',
      r: 'ر',
      s: 'س',
      t: 'ت',
      u: 'و',
      v: 'ف',
      w: 'و',
      x: 'كس',
      y: 'ي',
      z: 'ز'
    };

    text = text.toLowerCase();

    // Replace combos first
    Object.keys(combos).forEach(c => {
      text = text.replaceAll(c, combos[c]);
    });

    // Replace single letters
    return text
      .split('')
      .map(c => singles[c] || c)
      .join('');
  }

}
=======
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import * as XLSX from 'xlsx';

import { CustomerService, Customer } from './customer.service';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {

  // ================= DATA =================
  customers: Customer[] = [];
  filteredCustomers: Customer[] = [];
  paginatedCustomers: Customer[] = [];

  // ================= SEARCH =================
  searchFilters: any = {
    customerName: '',
    nameAr: '',
    phone: '',
    email: '',
    address: '',
    vatNo: ''
  };

  // ================= PAGINATION =================
  currentPage = 1;
  itemsPerPage = 5;
  totalPages = 1;
  totalItems = 0;



  // ================= LOADING =================
  isLoading = false;

  constructor(
    private customerService: CustomerService,
    private router: Router
  ) { }

  // ================= INIT =================
  ngOnInit(): void {
    this.loadCustomers();
  }

  // ================= LOAD =================
  loadCustomers(): void {
    this.isLoading = true;

    this.customerService.getAll().subscribe({
      next: (data) => {
        const customers = Array.isArray(data) ? data : [];
        this.customers = customers;
        this.filteredCustomers = customers;
        this.totalItems = customers.length;
        this.currentPage = 1;
        this.updatePagination();
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        Swal.fire('Error', err.message || 'Failed to load customers', 'error');
      }
    });
  }

  // ================= SEARCH =================
  onColumnSearch(): void {
    this.filteredCustomers = this.customers.filter(c =>
      (!this.searchFilters.customerName || c.name?.toLowerCase().includes(this.searchFilters.customerName.toLowerCase())) &&
      (!this.searchFilters.nameAr || c.nameAr?.toLowerCase().includes(this.searchFilters.nameAr.toLowerCase())) &&
      (!this.searchFilters.phone || c.phone?.includes(this.searchFilters.phone)) &&
      (!this.searchFilters.email || c.email?.toLowerCase().includes(this.searchFilters.email.toLowerCase())) &&
      (!this.searchFilters.address || c.address?.toLowerCase().includes(this.searchFilters.address.toLowerCase())) &&
      (!this.searchFilters.vatNo || c.vatNo?.toLowerCase().includes(this.searchFilters.vatNo.toLowerCase()))
    );

    this.currentPage = 1;
    this.updatePagination();
  }

  clearAllFilters(): void {
    this.searchFilters = {
      customerName: '',
      nameAr: '',
      phone: '',
      email: '',
      address: '',
      vatNo: ''
    };
    this.onColumnSearch();
  }

  // ================= PAGINATION =================
  updatePagination(): void {
    this.totalItems = this.filteredCustomers.length;

    if (this.itemsPerPage === 0) {
      this.paginatedCustomers = this.filteredCustomers;
      this.totalPages = 1;
      return;
    }

    this.totalPages = Math.ceil(this.totalItems / this.itemsPerPage) || 1;
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedCustomers = this.filteredCustomers.slice(start, end);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  onItemsPerPageChange(): void {
    this.currentPage = 1;
    this.updatePagination();
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  // ================= NAVIGATION =================
  openAddModal(): void {
    this.router.navigate(['./customer/add']);
  }

  openEditModal(customer: Customer): void {
    this.router.navigate(['./customer/edit', customer.customerCode]);
  }



  // ================= DELETE =================
  deleteCustomer(customer: Customer): void {
    Swal.fire({
      title: 'Are you sure?',
      text: `Delete ${customer.name}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33'
    }).then(res => {
      if (res.isConfirmed) {
        this.customerService.delete(customer.customerCode).subscribe({
          next: () => {
            Swal.fire('Deleted', 'Customer deleted successfully', 'success');
            this.loadCustomers();
          },
          error: err => Swal.fire('Error', err.message, 'error')
        });
      }
    });
  }

  // ================= EXPORT =================
  exportToExcel(): void {
    const data = this.filteredCustomers.map((c, i) => ({
      'S.No': i + 1,
      'Customer Code': c.customerCode,
      'Name': c.name,
      'Name(AR)': c.nameAr,
      'Phone': c.phone,
      'Email': c.email,
      'Address': c.address,
      'VAT No': c.vatNo
    }));

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Customers');

    XLSX.writeFile(
      workbook,
      `Customers_${new Date().toISOString().split('T')[0]}.xlsx`
    );
  }


  Math = Math;
}
>>>>>>> bf4536ad25e42144e3814695c42fad2bb6622520
