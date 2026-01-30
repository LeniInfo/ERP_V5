import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-po-inquiry',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './po-inquiry.component.html',
  styleUrls: ['./po-inquiry.component.css']
})
export class PoInquiryComponent implements OnInit {

  poList: any[] = [];
  filteredList: any[] = [];

  searchText = '';
  fromDate = '';
  toDate = '';

  loading = false;
  error = '';

  constructor(
    private apiService: ApiService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadPOs();
  }

  // ================= LOAD PURCHASE ORDERS =================

  loadPOs(): void {
    this.loading = true;
    this.error = '';

    this.apiService.getAllPurchaseOrders().subscribe({
      next: (res: any[]) => {

        this.poList = res.map(po => ({
          fran: po.franCode ?? po.fran,
          branch: po.branchCode ?? po.branch,
          warehouseCode: po.whCode ?? po.warehouseCode,
          poType: po.potype ?? po.poType,
          pono: po.pono ?? po.poNumber,
          supplier: po.supplier ?? po.supplierCode,
          supplierName: po.supplierName,
          createdt: po.createdt ?? po.poDate,
          noofitems: po.noofitems ?? po.noOfItems,
          totalvalue: po.totalvalue ?? po.totalValue,
          discount: po.discount ?? 0,
          vatvalue: po.vat ?? po.vatvalue ?? 0
        }));

        this.filteredList = [...this.poList];
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading purchase orders:', err);
        this.error = 'Failed to load purchase orders';
        this.loading = false;
      }
    });
  }

  // ================= FILTER =================

  filterData(): void {
    this.filteredList = this.poList.filter(po => {

      const searchMatch = this.searchText
        ? po.pono?.toLowerCase().includes(this.searchText.toLowerCase()) ||
        po.supplier?.toLowerCase().includes(this.searchText.toLowerCase()) ||
        po.supplierName?.toLowerCase().includes(this.searchText.toLowerCase())
        : true;

      const fromMatch = this.fromDate
        ? new Date(po.createdt) >= new Date(this.fromDate)
        : true;

      const toMatch = this.toDate
        ? new Date(po.createdt) <= new Date(this.toDate)
        : true;

      return searchMatch && fromMatch && toMatch;
    });
  }

  clearFilters(): void {
    this.searchText = '';
    this.fromDate = '';
    this.toDate = '';
    this.filteredList = [...this.poList];
  }

  // ================= EDIT =================

  editPurchaseOrder(po: any): void {
    this.router.navigate(['/purchase-order'], {
      queryParams: {
        fran: po.fran,
        branch: po.branch,
        warehouseCode: po.warehouseCode,
        poType: po.poType,
        pono: po.pono,
        mode: 'edit'
      }
    });
  }

  // ================= DELETE =================

  deletePO(po: any): void {
    if (!confirm(`Are you sure you want to delete PO ${po.pono}?`)) return;

    this.apiService
      .deletePurchaseOrder(po.fran, po.pono)
      .subscribe({
        next: () => {
          alert(`Purchase Order ${po.pono} deleted successfully`);
          this.loadPOs();
        },
        error: (err) => {
          console.error('Error deleting PO:', err);
          alert('Failed to delete Purchase Order');
        }
      });
  }

  // ================= CREATE =================

  goToPOCreation(): void {
    this.router.navigate(
      ['/purchase-order'],
      { queryParams: { mode: 'create' } }
    );
  }
}
