import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-work-enquiry-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './work-enquiry-list.component.html'
})
export class WorkEnquiryListComponent {

  workEnquiries: any[] = [];
  isLoadingWorkEnquiries = false;
  showDetailModal = false;
  selectedWorkEnquiryForDetail: any;

  constructor(private router: Router) { }

  // Navigate to add new work enquiry form
  goToAddWorkEnquiry(): void {
    this.router.navigate(['./work-enquiry-form']);
  }

  getStatusBadgeClass(status: string): string {
    switch (status?.toUpperCase()) {
      case 'OPEN': return 'bg-warning';
      case 'COMPLETED': return 'bg-success';
      case 'CANCELLED': return 'bg-danger';
      default: return 'bg-secondary';
    }
  }

  showDetail(enquiry: any): void {
    this.selectedWorkEnquiryForDetail = enquiry;
    this.showDetailModal = true;
  }

  closeDetailModal(): void {
    this.showDetailModal = false;
  }
}
