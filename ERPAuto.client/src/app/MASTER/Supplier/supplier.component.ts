import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ApiService, Supplier } from '../../services/api.service';

@Component({
  selector: 'app-supplier',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.css']
})
export class SupplierComponent implements OnInit {

  supplier: Supplier = this.getEmptySupplier();

  suppliers: Supplier[] = [];
  filteredSuppliers: Supplier[] = [];

  isEditing = false;
  searchText = '';
  originalSupplierName = '';
  arabicManuallyEdited = false;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  // 🔹 Empty model
  getEmptySupplier(): Supplier {
    return {
      supplierCode: '',
      supplierName: '',
      arabicName: '',
      phone: '',
      email: '',
      address: '',
      vatNo: ''
    };
  }

  // 🔹 Load suppliers
  loadSuppliers(): void {
    this.apiService.getSuppliers().subscribe({
      next: (data) => {
        this.suppliers = data || [];
        this.applyFilter();
      },
      error: (err) => {
        alert(err.error?.message || 'Error fetching suppliers');
      }
    });
  }

  // 🔹 Create / Update
  onSubmit(): void {
    if (!this.supplier.supplierCode?.trim() || !this.supplier.supplierName?.trim()) {
      alert('Supplier Code and Name are required!');
      return;
    }

    const apiCall$ = this.isEditing
      ? this.apiService.updateSupplier(this.supplier.supplierCode, this.supplier)
      : this.apiService.createSupplier(this.supplier);

    apiCall$.subscribe({
      next: () => {
        alert(`Supplier ${this.isEditing ? 'updated' : 'created'} successfully`);
        this.resetForm();
        this.loadSuppliers();
      },
      error: (err) => {
        alert(err.error?.message || 'Error saving supplier');
      }
    });
  }

  // 🔹 Edit
  onEdit(v: Supplier): void {
    this.supplier = { ...v };
    this.originalSupplierName = v.supplierName;
    this.arabicManuallyEdited = false;
    this.isEditing = true;
  }

  // 🔹 Delete
  onDelete(supplierCode: string): void {
    if (!supplierCode) return;

    if (confirm('Are you sure you want to delete this supplier?')) {
      this.apiService.deleteSupplier(supplierCode).subscribe({
        next: () => {
          this.loadSuppliers();
          this.resetForm();
        },
        error: (err) => {
          alert(err.error?.message || 'Error deleting supplier');
        }
      });
    }
  }

  // 🔹 Reset
  resetForm(): void {
    this.supplier = this.getEmptySupplier();
    this.isEditing = false;
  }

  // 🔹 Search
  applyFilter(): void {
    const search = this.searchText.toLowerCase().trim();

    this.filteredSuppliers = this.suppliers.filter(v =>
      v.supplierCode.toLowerCase().includes(search) ||
      v.supplierName.toLowerCase().includes(search)
    );
  }

  onClear(): void {
    this.originalSupplierName = '';
    this.arabicManuallyEdited = false;
    this.resetForm();
  }

  // 🔹 Auto Arabic conversion
  onSupplierNameChange(value: string): void {
    if (!value) return;

    const nameChanged = value !== this.originalSupplierName;

    if ((!this.isEditing || nameChanged) && !this.arabicManuallyEdited) {
      this.supplier.arabicName = this.toArabic(value);
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
