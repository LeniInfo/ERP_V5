import { Component, OnInit } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-work-quotation-list',
  standalone: true,
  imports: [CommonModule, DecimalPipe],
  templateUrl: './work-quotation-list.component.html'
})
export class WorkQuotationListComponent implements OnInit {

  workQuotations: any[] = [];
  quotationDetails: { [key: string]: any[] } = {}; // Store details by quotationNo
  workMasters: any[] = []; // Store work masters for name lookup
  isLoadingQuotations = false;
  showDetailModal = false;
  selectedQuotationForDetail: any;
  expandedQuotations: Set<string> = new Set(); // Track expanded quotations

  readonly quotationApiUrl = 'http://localhost:5220/api/v1/QuotationHeader';
  readonly quotationDetailApiUrl = 'http://localhost:5220/api/v1/QuotationDetail';
  readonly workMasterApiUrl = 'https://localhost:7231/api/v1/WorkMaster';
  readonly defaultFran = 'MAIN';
  readonly defaultBranch = 'MAIN';
  readonly defaultWarehouse = 'MAIN';
  readonly defaultQuotType = 'enquiry';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.loadWorkMasters();
    this.loadQuotations();
  }

  loadWorkMasters(): void {
    this.http.get<any[]>(this.workMasterApiUrl).subscribe({
      next: (data) => {
        this.workMasters = data || [];
        console.log('WorkMasters loaded:', this.workMasters.length);
      },
      error: (err) => {
        console.error('Error loading WorkMasters:', err);
        this.workMasters = [];
      }
    });
  }

  getWorkName(detail: any): string {
    // WorkId is stored in QUOTATIONSRL (quotSrl) as a string
    let workId: number | null = null;
    
    if (detail.quotSrl) {
      const parsed = parseInt(detail.quotSrl, 10);
      if (!isNaN(parsed)) {
        workId = parsed;
      }
    }
    
    // Fallback to workId if quotSrl parsing fails
    if (!workId && detail.workId) {
      if (typeof detail.workId === 'number') {
        workId = detail.workId;
      } else {
        const parsed = parseInt(String(detail.workId), 10);
        workId = !isNaN(parsed) ? parsed : null;
      }
    }
    
    if (workId) {
      const workMaster = this.workMasters.find(w => w.workId === workId);
      return workMaster ? workMaster.name : `Work ID: ${workId}`;
    }
    
    return 'N/A';
  }

  loadQuotations(): void {
    this.isLoadingQuotations = true;
    this.http.get<any[]>(this.quotationApiUrl).subscribe({
      next: (data) => {
        // Filter only work quotations (quotType = 'enquiry' and quotationSource = 'work')
        this.workQuotations = (data || []).filter(q => 
          q.quotType === this.defaultQuotType && 
          q.quotationSource === 'work'
        );
        this.isLoadingQuotations = false;
        console.log('Quotations loaded:', this.workQuotations.length);
      },
      error: (err) => {
        this.isLoadingQuotations = false;
        console.error('Error loading quotations:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load quotations',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  loadQuotationDetails(quotationNo: string): void {
    if (this.quotationDetails[quotationNo]) {
      // Already loaded, just toggle expansion
      this.toggleExpand(quotationNo);
      // If modal is open, show it
      if (this.showDetailModal && this.selectedQuotationForDetail?.quotationNo === quotationNo) {
        this.showDetailModal = true;
      }
      return;
    }

    const url = `${this.quotationDetailApiUrl}/by-header/${this.defaultFran}/${this.defaultBranch}/${this.defaultWarehouse}/${this.defaultQuotType}/${quotationNo}`;
    this.http.get<any[]>(url).subscribe({
      next: (data) => {
        this.quotationDetails[quotationNo] = data || [];
        this.toggleExpand(quotationNo);
        // If modal is open for this quotation, show it
        if (this.showDetailModal && this.selectedQuotationForDetail?.quotationNo === quotationNo) {
          this.showDetailModal = true;
        }
        console.log(`Details loaded for quotation ${quotationNo}:`, this.quotationDetails[quotationNo].length);
      },
      error: (err) => {
        console.error(`Error loading details for quotation ${quotationNo}:`, err);
        this.quotationDetails[quotationNo] = [];
        if (this.showDetailModal && this.selectedQuotationForDetail?.quotationNo === quotationNo) {
          this.showDetailModal = true;
        }
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load quotation details',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  toggleExpand(quotationNo: string): void {
    if (this.expandedQuotations.has(quotationNo)) {
      this.expandedQuotations.delete(quotationNo);
    } else {
      this.expandedQuotations.add(quotationNo);
    }
  }

  isExpanded(quotationNo: string): boolean {
    return this.expandedQuotations.has(quotationNo);
  }

  getQuotationDetails(quotationNo: string): any[] {
    return this.quotationDetails[quotationNo] || [];
  }

  // Navigate to add new quotation form
  goToAddQuotation(): void {
    this.router.navigate(['./work-quotation-form']);
  }

  getStatusBadgeClass(status: string): string {
    switch (status?.toUpperCase()) {
      case 'OPEN': return 'bg-warning';
      case 'COMPLETED': return 'bg-success';
      case 'CANCELLED': return 'bg-danger';
      default: return 'bg-secondary';
    }
  }

  showDetail(q: any): void {
    this.selectedQuotationForDetail = q;
    // Load details if not already loaded
    if (!this.quotationDetails[q.quotationNo]) {
      this.loadQuotationDetails(q.quotationNo);
    } else {
      // If already loaded, just show the modal
      this.showDetailModal = true;
    }
  }

  closeDetailModal(): void {
    this.showDetailModal = false;
  }

  editQuotation(quotation: any): void {
    // Navigate to edit quotation form with quotation data
    this.router.navigate(['./work-quotation-form'], { 
      queryParams: { 
        fran: quotation.fran,
        branch: quotation.branch,
        warehouse: quotation.warehouse,
        quotType: quotation.quotType,
        quotationNo: quotation.quotationNo
      } 
    });
  }

  deleteQuotation(quotation: any): void {
    Swal.fire({
      title: 'Are you sure?',
      text: `Do you want to delete quotation ${quotation.quotationNo}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        const url = `${this.quotationApiUrl}/${quotation.fran}/${quotation.branch}/${quotation.warehouse}/${quotation.quotType}/${quotation.quotationNo}`;
        this.http.delete(url).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Deleted!',
              text: 'Quotation deleted successfully',
              timer: 1500
            });
            this.loadQuotations(); // Reload list
          },
          error: (err) => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: err.error?.message || 'Failed to delete quotation',
              confirmButtonColor: '#d33'
            });
          }
        });
      }
    });
  }

  viewQuotation(quotation: any): void {
    this.showDetail(quotation);
  }

  formatDate(date: string | Date): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('en-GB'); // DD/MM/YYYY format
  }
}
