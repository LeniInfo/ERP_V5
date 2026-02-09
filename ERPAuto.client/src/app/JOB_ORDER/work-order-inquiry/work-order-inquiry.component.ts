import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-work-order-inquiry',
  imports: [CommonModule, FormsModule],
  templateUrl: './work-order-inquiry.component.html',
  styleUrls: ['./work-order-inquiry.component.css'], // fix typo: styleUrl -> styleUrls
})
export class WorkOrderInquiryComponent implements OnInit {
  repairList: any[] = [];
  filteredList: any[] = [];
  searchText: string = '';
  fromDate: string = '';
  toDate: string = '';

  constructor(private apiService: ApiService, private router: Router) { }

  ngOnInit(): void {
    this.loadRepairOrders();
  }

  loadRepairOrders() {
    this.apiService.getAllWorkOrder().subscribe((data: any[]) => {
      console.log(data, "HDR DETAILS");
      this.repairList = data;
      this.filteredList = [...this.repairList];
    });
  }

  filterData() {
    this.filteredList = this.repairList.filter(ro => {
      const searchLower = this.searchText.toLowerCase();
      const repairDate = new Date(ro.repairdt);
      const from = this.fromDate ? new Date(this.fromDate) : null;
      const to = this.toDate ? new Date(this.toDate) : null;

      const matchesSearch =
        ro.repairNo.toLowerCase().includes(searchLower) ||
        ro.customer?.toLowerCase().includes(searchLower) ||
        ro.phone?.includes(searchLower);

      const matchesFrom = from ? repairDate >= from : true;
      const matchesTo = to ? repairDate <= to : true;

      return matchesSearch && matchesFrom && matchesTo;
    });
  }

  clearFilters() {
    this.searchText = '';
    this.fromDate = '';
    this.toDate = '';
    this.filteredList = [...this.repairList];
  }

  loadRepairOrder(row: any) {
    this.router.navigate(['/work-order'], {
      queryParams: {
        fran: row.fran,
        brch: row.brch,
        workshop: row.workshop,
        repairType: row.repairType,
        repairNo: row.repairNo,
        customer: row.customer,
        mode: 'edit'
      }
    });
  }


  deleteRepairOrder(repairOrder: any) {
    if (confirm(`Delete Repair Order ${repairOrder.repairNo}?`)) {
      this.apiService
        .deleteWorkOrder(
          repairOrder.fran,
          repairOrder.customer,
          repairOrder.repairNo
        )
        .subscribe(() => {
          this.loadRepairOrders();
        });
    }
  }

  goToRepairCreation() {
    this.router.navigate(
      ['/work-order'],
      { queryParams: { mode: 'create' } }
    );
  }
}
