import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SupplierInvoiceInquiryService } from './supplier-invoice-inquiry.service'; // Your API service

@Component({
  selector: 'app-shipment-inquiry',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './supplier-invoice-inquiry.component.html',
  styleUrls: ['./supplier-invoice-inquiry.component.css']
})
export class SupplierInvoiceInquiryComponent implements OnInit {

  shipmentList: any[] = [];
  filteredList: any[] = [];

  searchText = '';
  fromDate = '';
  toDate = '';

  loading = false;
  error = '';

  constructor(private shipmentService: SupplierInvoiceInquiryService, private router: Router) { }

  ngOnInit(): void {
    this.loadShipments();
  }

  // 🔹 Load all shipment headers from API
  loadShipments(): void {
    this.loading = true;
    this.shipmentService.getAllInvoices().subscribe({
      next: (res: any[]) => {
        console.log(res);
        // Map only the fields needed for the table
        this.shipmentList = res.map(s => ({
          fran: s.fran,
          branch: s.branch,
          warehouseCode: s.warehouseCode,
          shipmentType: s.shipmentType,
          shipmentNumber: s.shipmentNumber,
          supplier: s.supplierCode,
          shipmentDate: new Date(s.shipmentDate),
          currency: s.currency,
          noOfItems: s.noOfItems,
          netValue: s.totalValue || 0,
          discountValue: s.discountValue || 0,
          vatValue: s.vatValue || 0,
          details: s.details || []
        }));

        this.filteredList = [...this.shipmentList];
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading shipments:', err);
        this.error = 'Failed to load shipments';
        this.loading = false;
      }
    });
  }

  // 🔹 Filter by search text and date
  filterData(): void {
    this.filteredList = this.shipmentList.filter(s => {
      const searchMatch = this.searchText
        ? s.shipmentNumber.toLowerCase().includes(this.searchText.toLowerCase()) ||
        s.supplier.toLowerCase().includes(this.searchText.toLowerCase())
        : true;

      const fromMatch = this.fromDate ? new Date(s.shipmentDate) >= new Date(this.fromDate) : true;
      const toMatch = this.toDate ? new Date(s.shipmentDate) <= new Date(this.toDate) : true;

      return searchMatch && fromMatch && toMatch;
    });
  }

  clearFilters(): void {
    this.searchText = '';
    this.fromDate = '';
    this.toDate = '';
    this.filteredList = [...this.shipmentList];
  }

  // 🔹 Double click row to open shipment for editing
  editInvoice(shipment: any): void {
    this.router.navigate(['/supplier-invoice-create'], {
      queryParams: {
        fran: shipment.fran,
        branch: shipment.branch,
        warehouseCode: shipment.warehouseCode,       // match what create page expects
        sinvType: shipment.shipmentType,
        sinvno: shipment.shipmentNumber,
        mode: 'edit'
      }
    });
  }

  deleteShipment(shipment: any): void {
    if (!confirm(`Are you sure you want to delete this Shipment ${shipment.sinvno}?`)) return;

    this.shipmentService.deleteShipment(shipment.fran, shipment.branch, shipment.warehouseCode, shipment.shipmentType, shipment.shipmentNumber).subscribe({
      next: () => {
        alert(`Shipment ${shipment.sinvno} deleted successfully`);
        this.loadShipments(); // refresh list
      },
      error: (err) => {
        console.error('Error deleting Shipment:', err);
        alert('Failed to delete Shipment');
      }
    });
  }

  // 🔹 Navigate to shipment creation page
  goToInvoiceCreation(): void {
    this.router.navigate(['/supplier-invoice'], { queryParams: { mode: 'create' } });
  }
}
