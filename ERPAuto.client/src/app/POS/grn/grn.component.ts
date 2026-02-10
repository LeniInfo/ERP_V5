import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GrnService } from '../grn/grn.service';
import { PendingReceipt, SupplierInvoiceHeader, ShipmentforGRN } from './grn.model';
import { GrnCardComponent } from './grn-card/grn-card.component';

@Component({
  selector: 'app-grn',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, GrnCardComponent],
  templateUrl: './grn.component.html',
  styleUrl: './grn.component.css',
})
export class GrnComponent implements OnInit {

  invoices: PendingReceipt[] = [];
  selectedInvoice: ShipmentforGRN | null = null;

  constructor(private grnService: GrnService) { }

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.grnService.getPendingInvoices().subscribe({
      next: (res: PendingReceipt[]) => {
        console.log(res);
        this.invoices = res;
      },
      error: (err: any) => {
        console.error('Failed to load invoices', err);
      }
    });
  }
  

  openInvoice(inv: PendingReceipt): void {
    this.selectedInvoice = null; // reset previous
    this.grnService.getHeaderandDetails(inv.sinvno).subscribe({
      next: (res: ShipmentforGRN) => {
        this.selectedInvoice = res; // full header + details
      },
      error: (err) => {
        console.error('Failed to load invoice', err);
      }
    });
    document.body.style.overflow = 'hidden';
  }

  onClose(): void {
    this.selectedInvoice = null;
    document.body.style.overflow = 'auto';
  }

  onSaved(): void {
    this.selectedInvoice = null;
    this.loadInvoices();
  }
}
