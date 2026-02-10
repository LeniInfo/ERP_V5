import { Component, OnInit } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-work-enquiry-list',
  standalone: true,
  imports: [CommonModule, DecimalPipe],
  templateUrl: './work-enquiry-list.component.html'
})
export class WorkEnquiryListComponent implements OnInit {

  workEnquiries: any[] = [];
  enquiryDetails: { [key: string]: any[] } = {}; // Store details by requestNo
  customers: any[] = []; // Store customers for lookup
  workMasters: any[] = []; // Store work masters for part name lookup
  isLoadingWorkEnquiries = false;
  showDetailModal = false;
  selectedWorkEnquiryForDetail: any;
  expandedEnquiries: Set<string> = new Set(); // Track expanded enquiries

  readonly defaultFran = 'MAIN';
  readonly defaultBranch = 'MAIN';
  readonly defaultWarehouse = 'MAIN';
  readonly requestType = 'work order inquiry';

  constructor(
    private router: Router,
    private apiService: ApiService
  ) { }

  ngOnInit(): void {
    this.loadCustomers();
    this.loadWorkMasters();
    this.loadWorkEnquiries();
  }

  // Navigate to add new work enquiry form
  goToAddWorkEnquiry(): void {
    this.router.navigate(['/work-enquiry']);
  }

  loadCustomers(): void {
    this.apiService.getAllCustomers().subscribe({
      next: (data) => {
        this.customers = data || [];
      },
      error: (err) => {
        console.error('Error loading customers:', err);
      }
    });
  }

  loadWorkMasters(): void {
    this.apiService.getAllWorkMasters().subscribe({
      next: (data) => {
        this.workMasters = data || [];
      },
      error: (err) => {
        console.error('Error loading work masters:', err);
      }
    });
  }

  loadWorkEnquiries(): void {
    this.isLoadingWorkEnquiries = true;
    this.apiService.getAllRequestHeaders().subscribe({
      next: (data) => {
        // Filter by requestType if needed, or show all
        this.workEnquiries = (data || []).filter(e => 
          e.requestType === this.requestType || !this.requestType
        );
        this.isLoadingWorkEnquiries = false;
      },
      error: (err) => {
        this.isLoadingWorkEnquiries = false;
        console.error('Error loading work enquiries:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load work enquiries',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  loadEnquiryDetails(requestNo: string): void {
    if (this.enquiryDetails[requestNo]) {
      // Already loaded, just toggle expansion
      this.toggleExpand(requestNo);
      return;
    }

    this.apiService.getRequestDetailsByHeader(this.defaultFran, this.defaultBranch, this.defaultWarehouse, this.requestType, requestNo).subscribe({
      next: (data) => {
        this.enquiryDetails[requestNo] = data || [];
        this.toggleExpand(requestNo);
        console.log(`Details loaded for enquiry ${requestNo}:`, this.enquiryDetails[requestNo].length);
      },
      error: (err) => {
        console.error(`Error loading details for enquiry ${requestNo}:`, err);
        this.enquiryDetails[requestNo] = [];
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load enquiry details',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  toggleExpand(requestNo: string): void {
    if (this.expandedEnquiries.has(requestNo)) {
      this.expandedEnquiries.delete(requestNo);
    } else {
      this.expandedEnquiries.add(requestNo);
    }
  }

  isExpanded(requestNo: string): boolean {
    return this.expandedEnquiries.has(requestNo);
  }

  getCustomerName(customerCode: string): string {
    const customer = this.customers.find(c => c.customerCode === customerCode);
    return customer ? (customer.name || customerCode) : customerCode;
  }

  getPartName(partId: string): string {
    if (!partId) return 'N/A';
    const workId = parseInt(partId, 10);
    if (isNaN(workId)) return partId;
    const workMaster = this.workMasters.find(w => w.workId === workId);
    return workMaster ? workMaster.name : `Work ID: ${workId}`;
  }

  // Get description for a specific detail item by splitting header descriptions
  getItemDescriptionEn(enquiry: any, detailIndex: number, totalItems: number): string {
    if (!enquiry.descEn) return 'N/A';
    const descParts = enquiry.descEn.split(' | ').filter((d: string) => d.trim().length > 0);
    if (descParts.length === 0) return 'N/A';
    
    // If multiple descriptions, get by index; if single, show only for first item
    if (descParts.length > 1 && detailIndex < descParts.length) {
      return descParts[detailIndex];
    } else if (descParts.length === 1 && detailIndex === 0) {
      return descParts[0];
    }
    return 'N/A';
  }

  getItemDescriptionAr(enquiry: any, detailIndex: number, totalItems: number): string {
    if (!enquiry.descArabic) return 'N/A';
    const descParts = enquiry.descArabic.split(' | ').filter((d: string) => d.trim().length > 0);
    if (descParts.length === 0) return 'N/A';
    
    // If multiple descriptions, get by index; if single, show only for first item
    if (descParts.length > 1 && detailIndex < descParts.length) {
      return descParts[detailIndex];
    } else if (descParts.length === 1 && detailIndex === 0) {
      return descParts[0];
    }
    return 'N/A';
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
    // Load details if not already loaded
    if (!this.enquiryDetails[enquiry.requestNo]) {
      this.loadEnquiryDetails(enquiry.requestNo);
    }
    this.showDetailModal = true;
  }

  closeDetailModal(): void {
    this.showDetailModal = false;
  }

  editEnquiry(enquiry: any): void {
    // Navigate to edit page with enquiry data
    this.router.navigate(['/work-enquiry'], {
      queryParams: {
        fran: enquiry.fran,
        branch: enquiry.branch,
        warehouse: enquiry.warehouse,
        requestType: enquiry.requestType,
        requestNo: enquiry.requestNo
      }
    });
  }

  deleteEnquiry(enquiry: any): void {
    Swal.fire({
      title: 'Are you sure?',
      text: `Do you want to delete work enquiry ${enquiry.requestNo}? This will also delete all associated details.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        // First delete all details
        this.deleteEnquiryDetails(enquiry.requestNo, () => {
          // Then delete header
          this.apiService.deleteRequestHeader(enquiry.fran, enquiry.branch, enquiry.warehouse, enquiry.requestType, enquiry.requestNo).subscribe({
            next: () => {
              Swal.fire({
                icon: 'success',
                title: 'Deleted!',
                text: `Work enquiry ${enquiry.requestNo} has been deleted.`,
                confirmButtonColor: '#3085d6',
                timer: 2000
              });
              this.loadWorkEnquiries();
            },
            error: (err) => {
              Swal.fire({
                icon: 'error',
                title: 'Error',
                text: err.error?.message || 'Failed to delete work enquiry',
                confirmButtonColor: '#d33'
              });
            }
          });
        });
      }
    });
  }

  deleteEnquiryDetails(requestNo: string, callback: () => void): void {
    const details = this.enquiryDetails[requestNo] || [];
    if (details.length === 0) {
      callback();
      return;
    }

    let deletedCount = 0;
    const totalDetails = details.length;

    details.forEach((detail: any) => {
      this.apiService.deleteRequestDetail(detail.fran, detail.branch, detail.warehouse, detail.requestType, detail.requestNo, detail.requestSrl).subscribe({
        next: () => {
          deletedCount++;
          if (deletedCount === totalDetails) {
            delete this.enquiryDetails[requestNo];
            callback();
          }
        },
        error: (err) => {
          console.error(`Error deleting detail ${detail.requestSrl}:`, err);
          deletedCount++;
          if (deletedCount === totalDetails) {
            delete this.enquiryDetails[requestNo];
            callback();
          }
        }
      });
    });
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-GB', { day: '2-digit', month: '2-digit', year: 'numeric' });
  }
}
