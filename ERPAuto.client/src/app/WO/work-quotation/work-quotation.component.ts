import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CustomerService, Customer } from '../../MASTER/Customer/customer.service';
import Swal from 'sweetalert2';

interface WorkMaster {
  fran?: string;
  workId: number;
  workType?: string;
  name: string;
  remarks?: string;
  unitPrice?: number;
  estimated?: number;
  [key: string]: any;
}

interface QuotationItem {
  id: number;
  workId?: number; // Store WorkId
  workName: string; // Display name
  price: number;
  quantity: number;
  discount: number; // Discount per item
  total: number; // (Price * Quantity) - Discount
}

@Component({
  selector: 'app-work-quotation',
  standalone: true,
  imports: [CommonModule, FormsModule, DecimalPipe],
  templateUrl: './work-quotation.component.html',
  styleUrls: ['./work-quotation.component.css']
})
export class WorkQuotationComponent implements OnInit, OnDestroy {

  status: string = 'OPEN';
  quotationDate: string = new Date().toISOString().split('T')[0];
  
  // Customer
  customers: Customer[] = [];
  customerCode: string = '';
  customerSearchTerm: string = '';
  showCustomerDropdown: boolean = false;
  isLoadingCustomers: boolean = false;
  showCustomerModal: boolean = false;
  
  // Customer Form
  customerForm: Customer = this.getEmptyCustomer();
  emailError: string = '';
  phoneError: string = '';
  nameArManuallyEdited: boolean = false;
  
  // Quotation Items
  quotationItems: QuotationItem[] = [];
  nextItemId: number = 1;
  
  // Overall Discount
  overallSubtotal: number = 0;
  overallDiscount: number = 0;
  overallTotal: number = 0;
  
  // WorkMaster
  workMasters: WorkMaster[] = [];
  isLoadingWorkMasters: boolean = false;
  
  // API URLs
  readonly customerApiUrl = 'http://localhost:5220/api/v1/master/Customers';
  readonly workMasterApiUrl = 'https://localhost:7231/api/v1/WorkMaster';
  readonly quotationApiUrl = 'http://localhost:5220/api/v1/QuotationHeader';
  readonly quotationDetailApiUrl = 'http://localhost:5220/api/v1/QuotationDetail';
  
  // Default values
  readonly defaultFran: string = 'MAIN';
  readonly defaultBranch: string = 'MAIN';
  readonly defaultWarehouse: string = 'MAIN';
  readonly defaultQuotType: string = 'enquiry';
  readonly defaultQuotationSource: string = 'work'; // 'work' or 'marketing'
  readonly defaultCurrency: string = 'USD';
  
  private customerSearchTimeout: any = null;
  isEditMode: boolean = false;
  editQuotationNo: string = '';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient,
    private customerService: CustomerService
  ) { }

  ngOnInit(): void {
    this.loadCustomers();
    
    // Check if editing existing quotation
    this.route.queryParams.subscribe(params => {
      if (params['quotationNo']) {
        this.isEditMode = true;
        this.editQuotationNo = params['quotationNo'];
        // Load work masters first, then load quotation for edit
        this.isLoadingWorkMasters = true;
        this.http.get<WorkMaster[]>(this.workMasterApiUrl).subscribe({
          next: (data) => {
            this.workMasters = data || [];
            this.isLoadingWorkMasters = false;
            console.log('WorkMasters loaded for edit:', this.workMasters.length);
            // Now load quotation after work masters are loaded
            this.loadQuotationForEdit(
              params['fran'] || this.defaultFran,
              params['branch'] || this.defaultBranch,
              params['warehouse'] || this.defaultWarehouse,
              params['quotType'] || this.defaultQuotType,
              params['quotationNo']
            );
          },
          error: (err) => {
            this.isLoadingWorkMasters = false;
            console.error('Error loading WorkMasters:', err);
            // Still try to load quotation even if work masters fail
            this.loadQuotationForEdit(
              params['fran'] || this.defaultFran,
              params['branch'] || this.defaultBranch,
              params['warehouse'] || this.defaultWarehouse,
              params['quotType'] || this.defaultQuotType,
              params['quotationNo']
            );
          }
        });
      } else {
        this.loadWorkMasters();
        this.addNewItem();
      }
    });
  }

  ngOnDestroy(): void {
    if (this.customerSearchTimeout) {
      clearTimeout(this.customerSearchTimeout);
    }
  }

  // ========== CUSTOMER FUNCTIONS ==========
  
  loadCustomers(): void {
    this.isLoadingCustomers = true;
    this.http.get<Customer[]>(this.customerApiUrl).subscribe({
      next: (data) => {
        this.customers = data || [];
        this.isLoadingCustomers = false;
        console.log('Customers loaded:', this.customers.length);
      },
      error: (err) => {
        this.isLoadingCustomers = false;
        console.error('Error loading customers:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load customers',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  searchCustomers(searchTerm: string): void {
    if (!searchTerm || searchTerm.trim().length < 1) {
      this.loadCustomers();
      return;
    }

    this.isLoadingCustomers = true;
    const searchUrl = `${this.customerApiUrl}/search?name=${encodeURIComponent(searchTerm.trim())}`;
    this.http.get<Customer[]>(searchUrl).subscribe({
      next: (data) => {
        this.customers = data || [];
        this.isLoadingCustomers = false;
      },
      error: (err) => {
        this.isLoadingCustomers = false;
        this.customers = [];
      }
    });
  }

  onCustomerSearch(event: any): void {
    const searchTerm = event.target.value;
    this.customerSearchTerm = searchTerm;
    this.showCustomerDropdown = searchTerm.trim().length >= 1;

    if (this.customerSearchTimeout) {
      clearTimeout(this.customerSearchTimeout);
    }

    if (searchTerm.trim().length >= 1) {
      this.customerSearchTimeout = setTimeout(() => {
        this.searchCustomers(searchTerm);
      }, 300);
    } else {
      this.loadCustomers();
    }
  }

  onCustomerInputFocus(): void {
    if (this.customerSearchTerm.trim().length >= 1 || this.customers.length > 0) {
      this.showCustomerDropdown = true;
    }
  }

  onCustomerInputBlur(): void {
    setTimeout(() => {
      this.showCustomerDropdown = false;
    }, 200);
  }

  selectCustomer(customer: Customer): void {
    this.customerCode = customer.customerCode;
    this.customerSearchTerm = this.getCustomerDisplayName(customer);
    this.showCustomerDropdown = false;
  }

  getCustomerDisplayName(customer: Customer): string {
    return customer.name || customer.customerCode || '';
  }

  getFilteredCustomers(): Customer[] {
    if (!this.customerSearchTerm || this.customerSearchTerm.trim().length < 1) {
      return this.customers.slice(0, 10);
    }
    const search = this.customerSearchTerm.toLowerCase().trim();
    return this.customers.filter(c => 
      c.customerCode?.toLowerCase().includes(search) ||
      c.name?.toLowerCase().includes(search) ||
      c.nameAr?.toLowerCase().includes(search) ||
      c.phone?.includes(search) ||
      c.email?.toLowerCase().includes(search)
    ).slice(0, 20);
  }

  getSelectedCustomerName(): string {
    const selectedCustomer = this.customers.find(c => c.customerCode === this.customerCode);
    return selectedCustomer ? this.getCustomerDisplayName(selectedCustomer) : '';
  }

  clearCustomerSelection(): void {
    this.customerCode = '';
    this.customerSearchTerm = '';
  }

  // ========== CUSTOMER MODAL FUNCTIONS ==========

  openCustomerModal(): void {
    this.customerForm = this.getEmptyCustomer();
    this.emailError = '';
    this.phoneError = '';
    this.nameArManuallyEdited = false;
    this.showCustomerModal = true;
  }

  closeCustomerModal(): void {
    this.showCustomerModal = false;
    this.customerForm = this.getEmptyCustomer();
    this.emailError = '';
    this.phoneError = '';
    this.nameArManuallyEdited = false;
  }

  saveCustomer(): void {
    if (!this.validateCustomerForm()) {
      return;
    }

    const customerName = this.customerForm.name;
    const customerEmail = this.customerForm.email;

    this.customerService.create(this.customerForm).subscribe({
      next: (response) => {
        Swal.fire({
          icon: 'success',
          title: 'Success!',
          text: response.message || 'Customer added successfully',
          confirmButtonColor: '#3085d6',
          timer: 1500
        });
        
        // Reset customer form
        this.customerForm = this.getEmptyCustomer();
        this.emailError = '';
        this.phoneError = '';
        this.nameArManuallyEdited = false;
        this.closeCustomerModal();
        
        // Reload customers and auto-select the new one
        this.loadCustomers();
        setTimeout(() => {
          // Try to find the customer by name and email (since code is auto-generated)
          const newCustomer = this.customers.find(c => 
            c.name === customerName && 
            (customerEmail ? c.email === customerEmail : true)
          );
          if (newCustomer) {
            this.customerCode = newCustomer.customerCode;
            this.customerSearchTerm = this.getCustomerDisplayName(newCustomer);
            this.showCustomerDropdown = false;
          } else if (response.customerCode) {
            // Fallback: use customerCode from response if available
            this.customerCode = response.customerCode;
            const foundCustomer = this.customers.find(c => c.customerCode === response.customerCode);
            if (foundCustomer) {
              this.customerSearchTerm = this.getCustomerDisplayName(foundCustomer);
            }
            this.showCustomerDropdown = false;
          }
        }, 500);
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: err.message || 'Failed to add customer',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  getEmptyCustomer(): Customer {
    return {
      customerCode: '',
      name: '',
      nameAr: '',
      phone: '',
      email: '',
      address: '',
      vatNo: ''
    };
  }

  validateCustomerForm(): boolean {
    this.emailError = '';
    this.phoneError = '';

    if (!this.customerForm.name || this.customerForm.name.trim() === '') {
      Swal.fire({
        icon: 'error',
        title: 'Validation Error',
        text: 'Customer name is required',
        confirmButtonColor: '#d33'
      });
      return false;
    }

    if (this.customerForm.email && this.customerForm.email.trim() !== '') {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(this.customerForm.email)) {
        this.emailError = 'Invalid email format';
        return false;
      }
    }

    if (this.customerForm.phone && this.customerForm.phone.trim() !== '') {
      const phoneRegex = /^[0-9+\-\s()]+$/;
      if (!phoneRegex.test(this.customerForm.phone)) {
        this.phoneError = 'Invalid phone format';
        return false;
      }
    }

    return true;
  }

  onNameChange(): void {
    if (!this.nameArManuallyEdited && this.customerForm.name) {
      this.customerForm.nameAr = this.toArabic(this.customerForm.name);
    }
  }

  // Convert English text to Arabic (simple transliteration)
  toArabic(text: string): string {
    const map: { [key: string]: string } = {
      'a': 'ا', 'b': 'ب', 'c': 'ك', 'd': 'د', 'e': 'ي',
      'f': 'ف', 'g': 'ج', 'h': 'ه', 'i': 'ي', 'j': 'ج',
      'k': 'ك', 'l': 'ل', 'm': 'م', 'n': 'ن', 'o': 'و',
      'p': 'ب', 'q': 'ق', 'r': 'ر', 's': 'س', 't': 'ت',
      'u': 'و', 'v': 'ف', 'w': 'و', 'x': 'كس', 'y': 'ي', 'z': 'ز',
      ' ': ' '
    };

    return text
      .toLowerCase()
      .split('')
      .map(c => map[c] || c)
      .join('');
  }

  onNameArFocus(): void {
    this.nameArManuallyEdited = true;
  }

  // ========== WORKMASTER FUNCTIONS ==========

  loadWorkMasters(): void {
    this.isLoadingWorkMasters = true;
    this.http.get<WorkMaster[]>(this.workMasterApiUrl).subscribe({
      next: (data) => {
        this.workMasters = data || [];
        this.isLoadingWorkMasters = false;
        console.log('WorkMasters loaded:', this.workMasters.length);
      },
      error: (err) => {
        this.isLoadingWorkMasters = false;
        console.error('Error loading WorkMasters:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load Work Masters',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  onWorkSelected(item: QuotationItem, workId: number): void {
    const selectedWork = this.workMasters.find(w => w.workId === workId);
    if (selectedWork) {
      item.workId = selectedWork.workId;
      item.workName = selectedWork.name;
      // Auto-fill price from UnitPrice if available
      if (selectedWork.unitPrice !== undefined && selectedWork.unitPrice !== null) {
        item.price = Number(selectedWork.unitPrice);
      }
      // Recalculate when work is selected
      this.calculateItemTotal(item);
    }
  }

  getWorkDisplayName(work: WorkMaster): string {
    return work.name || `Work ID: ${work.workId}`;
  }

  // ========== QUOTATION ITEMS FUNCTIONS ==========

  addNewItem(): void {
    this.quotationItems.push({
      id: this.nextItemId++,
      workId: undefined,
      workName: '',
      price: 0,
      quantity: 0,
      discount: 0,
      total: 0
    });
  }

  removeItem(item: QuotationItem): void {
    const index = this.quotationItems.indexOf(item);
    if (index > -1) {
      this.quotationItems.splice(index, 1);
      this.calculateOverallTotals();
    }
  }

  calculateItemTotal(item: QuotationItem): void {
    const price = item.price || 0;
    const quantity = item.quantity || 0;
    const discount = item.discount || 0;
    
    // Calculate total (Price * Quantity) - Discount
    const subtotal = price * quantity;
    item.total = subtotal - discount;
    
    this.calculateOverallTotals();
  }

  calculateOverallTotals(): void {
    // Calculate overall subtotal (sum of all item totals)
    this.overallSubtotal = this.quotationItems.reduce((sum, item) => sum + (item.total || 0), 0);
    
    // Calculate overall total (Subtotal - Overall Discount)
    this.overallTotal = this.overallSubtotal - (this.overallDiscount || 0);
  }

  onOverallDiscountChange(): void {
    this.calculateOverallTotals();
  }

  calculateGrandTotal(): number {
    return this.overallTotal;
  }

  // Navigate back to quotation list
  goBackToList(): void {
    this.router.navigate(['./work-quotation-list']);
  }

  // Save Quotation
  saveQuotation(): void {
    // Validate required fields
    if (!this.customerCode || this.customerCode.trim() === '') {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please select a customer',
        confirmButtonColor: '#3085d6'
      });
      return;
    }

    if (!this.quotationDate) {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please select a quotation date',
        confirmButtonColor: '#3085d6'
      });
      return;
    }

    // Validate items
    const validItems = this.quotationItems.filter(item => 
      item.workId && item.workName && item.quantity > 0
    );

    if (validItems.length === 0) {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please add at least one quotation item',
        confirmButtonColor: '#3085d6'
      });
      return;
    }

    // Get next quotation number
    this.http.get<string>(`${this.quotationApiUrl}/next-quotation-number`, { responseType: 'text' as 'json' }).subscribe({
      next: (response) => {
        let quotationNo: string;
        if (typeof response === 'string') {
          quotationNo = response;
        } else if (response && typeof response === 'object') {
          quotationNo = (response as any).value || (response as any).data || (response as any).result || String(response);
        } else {
          quotationNo = String(response);
        }
        quotationNo = quotationNo.replace(/^["']|["']$/g, '').trim(); // Remove any quotes
        
        if (!quotationNo || quotationNo.length === 0) {
          throw new Error('Empty quotation number received from server');
        }
        
        console.log('Generated quotation number:', quotationNo);
        this.proceedWithSave(quotationNo, validItems);
      },
      error: (err) => {
        console.error('Error getting next quotation number:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: `Failed to generate quotation number: ${err.error?.text || err.message}. Please check the console for details.`,
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  private proceedWithSave(quotationNo: string, validItems: QuotationItem[]): void {
    const quotationDate = this.quotationDate || new Date().toISOString().split('T')[0];
    const noOfItems = validItems.length;
    // Overall discount is direct value (not percentage)
    const discount = this.overallDiscount || 0;
    // Total value = Overall subtotal - Overall discount
    const totalValue = this.overallTotal;

    const headerData: any = {
      fran: this.defaultFran,
      branch: this.defaultBranch,
      warehouse: this.defaultWarehouse,
      quotType: this.defaultQuotType,
      quotationNo: quotationNo,
      quotationDate: quotationDate,
      customer: this.customerCode,
      quotationSource: this.defaultQuotationSource, // 'work' for work quotation
      refNo: '', // Reference number (can be empty string)
      refDate: quotationDate, // Use quotation date as reference date
      seqNo: 0,
      seqPrefix: '',
      currency: this.defaultCurrency,
      noOfItems: noOfItems,
      discount: discount,
      vatValue: 0, // VAT value (set to 0 for now)
      totalValue: totalValue,
      createDt: new Date().toISOString().split('T')[0],
      createTm: new Date().toISOString(),
      createBy: 'SYSTEM',
      createRemarks: 'Created via Work Quotation Form',
      updateDt: new Date('1900-01-01').toISOString().split('T')[0],
      updateTm: new Date('1900-01-01').toISOString(),
      updateBy: '',
      updateRemarks: ''
    };

    if (this.isEditMode && this.editQuotationNo) {
      // Update existing quotation - use editQuotationNo instead of newly generated one
      headerData.quotationNo = this.editQuotationNo;
      const updateUrl = `${this.quotationApiUrl}/${this.defaultFran}/${this.defaultBranch}/${this.defaultWarehouse}/${this.defaultQuotType}/${this.editQuotationNo}`;
      this.http.put(updateUrl, headerData).subscribe({
        next: (response: any) => {
          console.log('Quotation header updated:', response);
          // Delete old details and save new ones
          this.deleteOldDetailsAndSaveNew(this.editQuotationNo, quotationDate, validItems);
        },
        error: (err) => {
          console.error('Error updating quotation header:', err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: err.error?.message || 'Failed to update quotation header',
            confirmButtonColor: '#d33'
          });
        }
      });
    } else {
      // Create new quotation
      this.http.post(`${this.quotationApiUrl}`, headerData).subscribe({
        next: (response: any) => {
          console.log('Quotation header created:', response);
          // After header is saved, save all detail items
          this.saveQuotationDetails(quotationNo, quotationDate, validItems);
        },
        error: (err) => {
          console.error('Error creating quotation header:', err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: err.error?.message || 'Failed to create quotation header',
            confirmButtonColor: '#d33'
          });
        }
      });
    }
  }

  private saveQuotationDetails(quotationNo: string, quotationDate: string, validItems: QuotationItem[]): void {
    const detailPromises: Promise<any>[] = [];
    const now = new Date();
    const createDt = now.toISOString().split('T')[0];
    const createTm = now.toISOString();

    // Create detail items - save sequentially to ensure proper serial numbers
    let savedCount = 0;
    const totalItems = validItems.length;

    validItems.forEach((item, index) => {
      // Discount is stored as direct value (not percentage)
      // Total = (Price * Quantity) - Discount
      const discountValue = item.discount || 0; // Direct discount value
      const totalValue = (item.price * item.quantity) - discountValue;

      // QUOTSRL: Use workId as string (as per user requirement: "store quotation id from workmaster")
      // If workId is not available, use sequential number
      const quotSrl = item.workId ? String(item.workId) : String(index + 1);

      const detailData: any = {
        fran: this.defaultFran,
        branch: this.defaultBranch,
        warehouse: this.defaultWarehouse,
        quotType: this.defaultQuotType, // 'enquiry' for work quotation
        quotationNo: quotationNo,
        quotSrl: quotSrl, // Store workId as serial number
        quotationDate: quotationDate,
        workId: item.workId || 0, // Store workId in WORKID field
        make: '', // Empty string as per requirement
        part: '', // Empty string as per requirement
        qty: item.quantity,
        unitPrice: item.price, // Use unitPrice instead of price
        discount: item.discount, // Discount direct value (not percentage)
        vatPercentage: 0,
        vatValue: 0,
        discountValue: discountValue, // Calculated discount value
        totalValue: totalValue, // Calculated total value
        createDt: createDt,
        createTm: createTm,
        createBy: 'SYSTEM',
        createRemarks: 'Created via Work Quotation Form',
        updateDt: new Date('1900-01-01').toISOString().split('T')[0],
        updateTm: new Date('1900-01-01').toISOString(),
        updateBy: '',
        updateRemarks: ''
      };

      // Save each detail item
      this.http.post(`${this.quotationDetailApiUrl}`, detailData).subscribe({
        next: () => {
          savedCount++;
          console.log(`Detail item ${savedCount}/${totalItems} saved`);
          
          // When all items are saved, show success message
          if (savedCount === totalItems) {
            const actionText = this.isEditMode ? 'updated' : 'created';
            Swal.fire({
              icon: 'success',
              title: 'Success!',
              text: `Quotation ${quotationNo} ${actionText} successfully with ${totalItems} items`,
              confirmButtonColor: '#3085d6',
              timer: 2000
            }).then(() => {
              this.goBackToList();
            });
          }
        },
        error: (err) => {
          console.error(`Error saving detail item ${index + 1}:`, err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: `Failed to save item ${index + 1}. ${err.error?.message || 'Please check and retry.'}`,
            confirmButtonColor: '#d33'
          });
        }
      });
    });
  }

  // Delete old details and save new ones (for edit mode)
  deleteOldDetailsAndSaveNew(quotationNo: string, quotationDate: string, validItems: QuotationItem[]): void {
    // First, load existing details to delete them
    const detailUrl = `${this.quotationDetailApiUrl}/by-header/${this.defaultFran}/${this.defaultBranch}/${this.defaultWarehouse}/${this.defaultQuotType}/${quotationNo}`;
    this.http.get<any[]>(detailUrl).subscribe({
      next: (existingDetails) => {
        // Delete all existing details
        const deletePromises = existingDetails.map(detail => 
          this.http.delete(`${this.quotationDetailApiUrl}/${detail.fran}/${detail.branch}/${detail.warehouse}/${detail.quotType}/${detail.quotationNo}/${detail.quotSrl}`).toPromise()
        );

        Promise.all(deletePromises).then(() => {
          console.log('Old details deleted, saving new ones...');
          // Now save new details
          this.saveQuotationDetails(quotationNo, quotationDate, validItems);
        }).catch((err) => {
          console.error('Error deleting old details:', err);
          // Still try to save new details
          this.saveQuotationDetails(quotationNo, quotationDate, validItems);
        });
      },
      error: (err) => {
        console.error('Error loading existing details for deletion:', err);
        // Still try to save new details
        this.saveQuotationDetails(quotationNo, quotationDate, validItems);
      }
    });
  }

  // Load quotation for editing
  loadQuotationForEdit(fran: string, branch: string, warehouse: string, quotType: string, quotationNo: string): void {
    // Load header
    const headerUrl = `${this.quotationApiUrl}/${fran}/${branch}/${warehouse}/${quotType}/${quotationNo}`;
    this.http.get<any>(headerUrl).subscribe({
      next: (header) => {
        if (header) {
          this.quotationDate = header.quotationDate ? new Date(header.quotationDate).toISOString().split('T')[0] : this.quotationDate;
          this.customerCode = header.customer || '';
          this.overallDiscount = header.discount || 0;
          
          // Load customer name for display
          if (this.customerCode) {
            const customer = this.customers.find(c => c.customerCode === this.customerCode);
            if (customer) {
              this.customerSearchTerm = this.getCustomerDisplayName(customer);
            }
          }

          // Load details
          this.loadQuotationDetailsForEdit(fran, branch, warehouse, quotType, quotationNo);
        }
      },
      error: (err) => {
        console.error('Error loading quotation header:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load quotation',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  loadQuotationDetailsForEdit(fran: string, branch: string, warehouse: string, quotType: string, quotationNo: string): void {
    const detailUrl = `${this.quotationDetailApiUrl}/by-header/${fran}/${branch}/${warehouse}/${quotType}/${quotationNo}`;
    this.http.get<any[]>(detailUrl).subscribe({
      next: (details) => {
        this.quotationItems = [];
        this.nextItemId = 1;

        details.forEach((detail) => {
          // WorkId is stored in QUOTATIONSRL (quotSrl) as a string
          // Parse it to get the workId number
          let workId: number | undefined = undefined;
          
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
              workId = !isNaN(parsed) ? parsed : undefined;
            }
          }
          
          // Find work master to get name
          const workMaster = workId ? this.workMasters.find(w => w.workId === workId) : null;
          
          const item: QuotationItem = {
            id: this.nextItemId++,
            workId: workId,
            workName: workMaster ? workMaster.name : (workId ? `Work ID: ${workId}` : ''),
            price: detail.unitPrice || detail.price || 0, // Support both unitPrice and price
            quantity: detail.qty,
            discount: detail.discount,
            total: detail.totalValue
          };
          
          console.log(`Loaded item: workId=${workId}, quotSrl=${detail.quotSrl}, workName=${item.workName}`);
          this.quotationItems.push(item);
        });

        // Recalculate totals
        this.calculateOverallTotals();

        if (this.quotationItems.length === 0) {
          this.addNewItem();
        }
      },
      error: (err) => {
        console.error('Error loading quotation details:', err);
        this.addNewItem(); // Add empty item if details fail to load
      }
    });
  }
}
