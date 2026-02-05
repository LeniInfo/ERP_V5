import { Component, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ApiService } from '../../services/api.service';


@Component({
  selector: 'app-work-order',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './work-order.component.html',
  styleUrls: ['./work-order.component.css']
})
export class WorkOrderComponent implements OnInit {

  isEditMode = false;
  isEditing = false;
  editIndex: number | null = null;

  //arabic conversion
  isAutoTranslating = false;

  header = {
    fran: 'A',
    brch: 'B1',
    workshop: 'WS1',
    repairType: 'JOB',
    repairNo: '',
    repairDt: new Date().toISOString().split('T')[0],
    customer: ''
  };

  detail = {
    workId: 0,
    workName: '',
    workDesc: '',
    workDescAR: '',
    unitPrice: 0,
    qty: 1,
    discount: 0,
    totalValue: 0
  };

  details: any[] = [];

  customers: any[] = [];
  filteredCustomers: any[] = [];
  showCustomerDropdown = false;
  customerSearch = '';

  works: any[] = [];
  filteredWorks: any[] = [];
  showWorkDropdown = false;
  workSearch = '';

  constructor(private api: ApiService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadCustomers();
    this.loadWorks();

    this.route.queryParams.subscribe(params => {
      const { fran, brch, workshop, repairType, repairNo, customer, mode } = params;

      if (mode === 'edit' && fran && brch && workshop && repairType && repairNo) {
        this.isEditMode = true;

        this.loadWorkOrder(
          fran,
          brch,
          workshop,
          repairType,
          repairNo,
          customer
        );
      }
    });
  }


  loadWorkOrder(
    fran: string,
    brch: string,
    workshop: string,
    repairType: string,
    repairNo: string,
    customer: string
  ): void {

    this.api.getRepairOrderByKey(
      fran, brch, workshop, repairType, repairNo, customer
    ).subscribe(res => {

      console.log(res);
      this.header = {
        fran: res.header.fran,
        brch: res.header.brch,
        workshop: res.header.workshop,
        repairType: res.header.repairType,
        repairNo: res.header.repairNo,
        repairDt: res.header.repairDt?.split('T')[0],
        customer: res.header.customer
      };

      this.details = (res.details || []).map((d: any) => ({
        workId: d.workId,
        workName: d.workType,
        workDesc: d.workDesc,
        workDescAR: d.workDescAr,
        unitPrice: d.unitPrice,
        qty: d.qty || d.noOfWorks,
        discount: d.discount,
        totalValue: d.totalValue
      }));

      const cust = this.customers.find(c => c.customerCode === res.header.customer);
      if (cust) {
        this.customerSearch = cust.name;
        this.filteredCustomers = [...this.customers];
      }

      /* -------- WORK DROPDOWN BIND -------- */
      this.filteredWorks = [...this.works];
      this.isEditMode = true;
    });
  }


  updateValue() {
    this.detail.totalValue = (this.detail.unitPrice * this.detail.qty) - this.detail.discount;
  }

  addDetail() {
    this.details.push({ ...this.detail });
    this.resetDetail();
  }

  editDetail(i: number) {
    this.detail = { ...this.details[i] };

    const workId = Number(this.detail.workId);

    const work = this.works.find(w => Number(w.workId) === workId);

    if (work) {
      this.workSearch = work.name;   
    } else {
      this.workSearch = this.detail.workName || ''; // fallback
    }

    this.filteredWorks = [...this.works];

    this.showWorkDropdown = false;

    this.isEditing = true;
    this.editIndex = i;
  }



  updateDetail() {
    if (this.editIndex !== null) {
      this.details[this.editIndex] = { ...this.detail };
      this.isEditing = false;
      this.editIndex = null;
      this.resetDetail();
    }
  }

  removeDetail(i: number) {
    this.details.splice(i, 1);
  }

  resetDetail() {
    this.detail = {
      workId: 0,
      workName: '',
      workDesc: '',
      workDescAR: '',
      unitPrice: 0,
      qty: 1,
      discount: 0,
      totalValue: 0
    };
  }

  onSubmit(): void {

    /* -------------------- VALIDATION -------------------- */
    if (!this.header.customer) {
      alert('Customer is required');
      return;
    }

    if (this.details.length === 0) {
      alert('At least one work item is required');
      return;
    }

    /* -------------------- CALCULATIONS -------------------- */
    const totalValue = this.details.reduce((sum, d) => sum + (d.totalValue || 0), 0);
    const noOfParts = this.details.reduce((sum, d) => sum + (d.qty || 0), 0);

    /* -------------------- DTO BUILD -------------------- */
    const dto = {
      header: {
        fran: this.header.fran,
        brch: this.header.brch,
        workshop: this.header.workshop,
        repairType: this.header.repairType,
        repairNo: this.header.repairNo || '',              
        repairDt: this.header.repairDt,
        customer: this.header.customer,
        vehicleId: 0,                                      
        currency: 'INR',
        noOfParts: noOfParts,
        noOfJobs: this.details.length,
        discount: this.details.reduce((s, d) => s + (d.discount || 0), 0),
        totalValue: totalValue,
        createBy: 'User',
        createRemarks: ''
      },

      details: this.details.map((d, i) => ({
        fran: this.header.fran,
        brch: this.header.brch,
        workshop: this.header.workshop,
        repairType: this.header.repairType,
        repairNo: this.header.repairNo || '',              
        repairSrl: (i + 1).toString(),
        workId: d.workId,
        workType: d.workName,                   
        workDt: this.header.repairDt,
        noOfWorks: d.qty,
        unitPrice: d.unitPrice,
        discount: d.discount,
        totalValue: d.totalValue,
        createBy: 'User',
        qty: d.qty,
        createRemarks: '',
        workDesc: d.workDesc,
        workDescAR: d.workDescAR
      }))
    };
     

    console.log('REPAIR ORDER DTO:', dto);

    /* -------------------- API CALL -------------------- */
    if (this.isEditMode) {
      this.api.updateWorkOrder(dto).subscribe({
        next: () => {
          alert('Updated successfully');

          this.resetForm();  
        },
        error: (err) => {
          console.error('FULL ERROR:', err);
          console.error('VALIDATION ERRORS:', err.error?.errors);
          alert('Update failed');
        }
      });
    } else {
      this.api.createWorkOrder(dto).subscribe({
        next: () => {
          alert('Saved successfully');

          this.resetForm();  
        },
        error: (err) => {
          console.error(err);
          alert('Save failed');
        }
      });
    }

  }


  goToInquiry() {
    this.router.navigate(['/work-order-inquiry']);
  }

  // lookups
  loadCustomers() { this.api.getCustomers().subscribe(r => { this.customers = r; console.log(r); this.filteredCustomers = r; }); }
  loadWorks() { this.api.getWorks().subscribe(r => { this.works = r; this.filteredWorks = r; }); }

  filterCustomers() {
    const v = this.customerSearch.toLowerCase();
    this.filteredCustomers = this.customers.filter((c: any) =>
      c.name.toLowerCase().includes(v)
    );
    this.showCustomerDropdown = true;
  }


  selectCustomer(c: any) {
    this.header.customer = c.customerCode;
    console.log(c, "Customer Dropdown");
    this.customerSearch = `${c.name}`;
    this.showCustomerDropdown = false;
  }

  filterWorks() {
    const v = (this.workSearch || '').toLowerCase().trim();

    this.filteredWorks = this.works.filter((w: any) =>
      w.name.toLowerCase().includes(v)
    );

    this.showWorkDropdown = true;
  }


  selectWork(w: any) {
    console.log(w, "work dropdown");
    this.detail.workId = w.workId;
    this.detail.workName = w.name;
    this.workSearch = w.name;

    if (!this.isEditing) {
      this.detail.unitPrice = w.price;
    }

    this.showWorkDropdown = false;
  }

  isHeaderDisabled(): boolean {
    return this.isEditMode;
  }


  onCustomerFocus() {
    this.filteredCustomers = [...this.customers];
    this.showCustomerDropdown = true;
  }

  onClear() {
    this.isEditing = false;
    this.editIndex = null;
    this.resetDetail();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;


    // Customer dropdown
    if (!target.closest('.customer-dropdown')) {
      this.showCustomerDropdown = false;
    }


    // Work dropdown
    if (!target.closest('.work-dropdown')) {
      this.showWorkDropdown = false;
    }
  }


  resetForm() {

    /* -------- HEADER RESET -------- */
    this.header = {
      fran: 'A',
      brch: 'B1',
      workshop: 'WS1',
      repairType: 'JOB',
      repairNo: '',
      repairDt: new Date().toISOString().split('T')[0],
      customer: ''
    };

    /* -------- SEARCH / DROPDOWNS -------- */
    this.customerSearch = '';
    this.workSearch = '';
    this.showCustomerDropdown = false;
    this.showWorkDropdown = false;

    /* -------- DETAIL INPUT RESET -------- */
    this.detail = {
      workId: 0,
      workName: '',
      workDesc: '',
      workDescAR: '',
      unitPrice: 0,
      qty: 1,
      discount: 0,
      totalValue: 0
    };

    /* -------- TABLE RESET -------- */
    this.details = [];

    /* -------- STATES -------- */
    this.isEditMode = false;
    this.isEditing = false;
    this.editIndex = null;

    /* -------- FILTER LISTS RESET -------- */
    this.filteredCustomers = [...this.customers];
    this.filteredWorks = [...this.works];
  }


  //arabic conversion
  onEnglishDescChange() {
    if (this.isAutoTranslating) return;

    if (!this.detail.workDesc?.trim()) {
      this.detail.workDescAR = '';
      return;
    }

    this.isAutoTranslating = true;

    this.api.translate(this.detail.workDesc, 'en', 'ar')
      .subscribe(res => {
        this.detail.workDescAR = res.translatedText;
        this.isAutoTranslating = false;
      });

    this.api.translate(this.detail.workDesc, "en", "ar")
        .subscribe(res => {
          console.log(res.translatedText);
          this.detail.workDescAR = res.translatedText;
          this.isAutoTranslating = false;
        });
  }

  onArabicDescChange() {
    if (this.isAutoTranslating) return;

    if (!this.detail.workDescAR?.trim()) {
      this.detail.workDesc = '';
      return;
    }

    this.isAutoTranslating = true;

    this.api.translate(this.detail.workDescAR, 'ar', 'en')
      .subscribe(res => {
        console.log(res.translatedText);
        this.detail.workDesc = res.translatedText;
        this.isAutoTranslating = false;
      });
  }
}
