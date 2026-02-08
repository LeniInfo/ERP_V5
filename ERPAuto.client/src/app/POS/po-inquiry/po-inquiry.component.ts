import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PoInquiryService } from './po-inquiry.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

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

  constructor(private poService: PoInquiryService, private router: Router) { }

  ngOnInit(): void {
    this.loadPOs();
  }

  // 🔹 Load all PO headers from API
  loadPOs(): void {
    this.loading = true;
    this.poService.getAllHeaders().subscribe({
      next: (res: any[]) => {
        this.poList = res.map(po => ({
          fran: po.fran,
          branch: po.branch,
          warehouseCode: po.warehouseCode,
          poType: po.poType,
          pono: po.poNumber,           // matches your table
          supplier: po.supplierCode,   // matches your table
          createdt: po.poDate,         // date
          noofitems: po.noOfItems,     // match exact JSON
          totalvalue: po.totalValue,
          discount: po.discount,
          vatvalue: po.vatValue || 0   // your backend may add this later
        }));

        this.filteredList = [...this.poList];
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading POs:', err);
        this.error = 'Failed to load purchase orders';
        this.loading = false;
      }
    });
  }

  // 🔹 Filter by search text and date
  filterData(): void {
    this.filteredList = this.poList.filter(po => {
      const searchMatch = this.searchText
        ? po.pono.toLowerCase().includes(this.searchText.toLowerCase()) ||
        po.supplier.toLowerCase().includes(this.searchText.toLowerCase())
        : true;

      const fromMatch = this.fromDate ? new Date(po.createdt) >= new Date(this.fromDate) : true;
      const toMatch = this.toDate ? new Date(po.createdt) <= new Date(this.toDate) : true;

      return searchMatch && fromMatch && toMatch;
    });
  }

  clearFilters(): void {
    this.searchText = '';
    this.fromDate = '';
    this.toDate = '';
    this.filteredList = [...this.poList];
  }

  // 🔹 Double click row to open PO for editing
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


  // 🔹 Delete PO
  deletePO(po: any): void {
    if (!confirm(`Are you sure you want to delete PO ${po.pono}?`)) return;

    this.poService.deletePO(po.fran, po.pono).subscribe({
      next: () => {
        alert(`Purchase Order ${po.pono} deleted successfully`);
        this.loadPOs(); // refresh list
      },
      error: (err) => {
        console.error('Error deleting PO:', err);
        alert('Failed to delete PO');
      }
    });
  }

  // 🔹 Navigate to PO creation page
  goToPOCreation(): void {
    this.router.navigate(['/purchase-order'], { queryParams: { mode: 'create' } });
  }
}
