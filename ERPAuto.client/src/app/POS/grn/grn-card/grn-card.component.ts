import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GrnService } from '../grn.service';
import { GrnCardService } from './grn-card.service';
import { CreateReceiptHeaderRequest, CreateReceiptDetailRequest } from './grn-card.model';
import { SupplierInvoiceHeader, SupplierInvoiceDetail, PendingReceipt, ShipmentforGRN, ShipmentforGRNDetails } from '../grn.model';

@Component({
  selector: 'app-grn-card',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './grn-card.component.html',
  styleUrl: './grn-card.component.css'
})
export class GrnCardComponent implements OnChanges {

  @Input() invoice!: ShipmentforGRN;

  /* 🔹 Output events */
  @Output() saved = new EventEmitter<void>();
  @Output() closed = new EventEmitter<void>();

  /* 🔹 Details list */
  details: Array<ShipmentforGRNDetails & {
    selected?: boolean;   
    newQty?: number;  
  }> = [];


  constructor(private grnService: GrnService,private grncardService: GrnCardService ) { }

  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['invoice'] && this.invoice) {
      // Clone details to local state
      this.details = this.invoice.details.map(d => ({
        ...d,
        newQty: 0,
        selected: false
      }));
      console.log('GRN Card loaded details:', this.details);
    }
  }

  /* 🔹 Close overlay */
  close(): void {
    this.closed.emit();
  }

  updateBalance(row: any) {
    if (row.receivedQty > row.balanceQty) {
      row.receivedQty = row.balanceQty; // prevent over-receiving
    }
    row.balanceQty = row.ordQty - row.receivedQty;
  }
  
  // Checkbox toggled
  onRowSelect(row: any) {
    if (row.selected) {
      // Fill receivedQty with ordered qty
      row.newQty = row.balanceQty;
    } else {
      // Uncheck → clear receivedQty
      row.newQty = 0;
    }
  }

  // Received qty input changed
  onReceivedQtyChange(row: any) {
    row.newQty = Number(row.newQty) || 0;

    // Automatically check the row if value > 0
    row.selected = row.balanceQty > 0;
  }

  saveGrn(): void {
    const selectedRows = this.details.filter(
      d => d.selected && d.newQty && d.newQty > 0
    );

    if (selectedRows.length === 0) {
      alert('Please select at least one row with Received Qty');
      return;
    }

    const netValue = selectedRows.reduce(
      (sum, d) => sum + (d.newQty! * d.unitPrice),
      0
    );

    const lines = selectedRows.map((row, index) => ({
      fran: this.invoice.fran,
      branch: this.invoice.branch,
      warehouse: this.invoice.warehouseCode,
      receiptType: 'GRN',
      receiptNo: this.invoice.shipmentNumber,
      receiptSerial: index + 1,           // sequential
      make: row.make,
      part: row.part,
      qty: row.newQty!,
      unitPrice: row.unitPrice,
      netValue: row.receivedQty! * row.unitPrice,
      currency: this.invoice.currency,
      storeId: '',
      supplier: this.invoice.supplierCode,
      poType: row.poType,
      poNo: this.invoice.shipmentNumber,
      poSrl: row.shipmentSerial,
      remarks: '',
      status: 'A'
    }));

    const header: CreateReceiptHeaderRequest = {
      fran: this.invoice.fran,
      branch: this.invoice.branch,
      warehouse: this.invoice.warehouseCode,
      receiptType: 'GRN',
      receiptNo: this.invoice.shipmentNumber,
      receiptDate: new Date().toISOString().substring(0, 10),
      noOfItems: selectedRows.length,
      netValue: netValue,
      currency: this.invoice.currency,
      supplier: this.invoice.supplierCode,
      seqPrefix: 'GRN',
      seqNo: 1,
      remarks: '',
      status: 'A',
      lines: lines // 🔹 include all detail lines here
    };

    this.grncardService.createReceipt(header).subscribe({
      next: () => {
        alert('GRN saved successfully');
        this.saved.emit();
        this.close();
      },
      error: err => {
        console.error(err);
        alert('Failed to save GRN');
      }
    });
  }

}
