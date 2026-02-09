import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule, DecimalPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomerService, Customer } from '../../MASTER/Customer/customer.service';

import Swal from 'sweetalert2';
import * as XLSX from 'xlsx';

interface OrderRow {
  id: number;
  partId: string; // Store selected part ID/stockkey for reference
  partName: string; // Display name
  make: string; // Part make
  price: number;
  quantity: number;
  discount: number;
  total: number;
  partSearchTerm?: string; // Search term for part dropdown
  showPartDropdown?: boolean; // Show/hide part dropdown
}

interface Part {
  partCode?: string; // Part code from API (camelCase)
  PartCode?: string; // Part code from API (PascalCase)
  PART?: string; // Part number/name (legacy)
  description?: string; // Description from API (camelCase) - USE THIS FOR DISPLAY
  Description?: string; // Description from API (PascalCase) - USE THIS FOR DISPLAY
  make?: string; // Make from API (camelCase)
  Make?: string; // Make from API (PascalCase)
  fob?: number; // Price (FOB) from API (camelCase)
  Fob?: number; // Price (FOB) from API (PascalCase)
  FOB?: number; // Price (FOB) (legacy)
  active?: boolean; // Active from API (camelCase)
  Active?: boolean; // Active from API (PascalCase)
  id?: number;
  stockKey?: string; // StockKey (should NOT be used for display - use Description instead)
  StockKey?: string; // StockKey (should NOT be used for display - use Description instead)
  [key: string]: any;
}

interface Vehicle {
  id?: number;
  vehicleCode?: string;
  plateNumber: string;
  make?: string;
  model?: string;
  year?: number;
  color?: string;
  chassisNumber?: string;
  customer?: number; // Customer ID
  customerCode?: string;
  [key: string]: any;
}

export interface CustomerOrder {
  id?: number;
  orderNumber?: string;
  customerCode?: string;
  customerName?: string;
  customerPhone?: string;
  vehicleId?: string;
  vehiclePlate?: string;
  orderDate?: string;
  status?: string;
  totalAmount?: number;
  items?: OrderRow[];
  createdAt?: string;
  updatedAt?: string;
  // Header fields from API
  grossValue?: number;
  netValue?: number;
  totalValue?: number;
  discountValue?: number;
  vatValue?: number;
  noOfItems?: number;
  fran?: string;
  branch?: string;
  warehouse?: string;
  cordType?: string;
  cordNo?: string;
  cordDate?: string;
  // Audit fields
  createDt?: string;
  createTm?: string;
  createBy?: string;
  updateDt?: string;
  updateTm?: string;
  updateBy?: string;
  [key: string]: any;
}

interface StatusParam {
  PARAMDESC?: string;
  paramDesc?: string;
  [key: string]: any;
}

@Component({
  selector: 'app-customer-order',
  standalone: true,
  imports: [CommonModule, FormsModule, DecimalPipe, DatePipe],
  templateUrl: './customer-order.component.html',
  styleUrls: ['./customer-order.component.css']
})
export class CustomerOrderComponent implements OnInit, OnDestroy {
  orderRows: OrderRow[] = [];
  allParts: Part[] = [];
  allMakesList: string[] = []; // Store all unique makes for dropdown
  isLoading: boolean = false;
  nextId: number = 1;

  // Status options from API
  statusOptions: StatusParam[] = [];
  isLoadingStatus: boolean = false;

  // Customer and Vehicle
  customers: Customer[] = [];
  vehicles: Vehicle[] = [];
  allVehicles: Vehicle[] = []; // Store all vehicles
  selectedCustomerId: number | null = null; // Track selected customer ID
  customerOrder: CustomerOrder = {
    customerCode: '',
    vehicleId: '',
    orderDate: new Date().toISOString().split('T')[0],
    status: 'Registered', // Default status when creating
    items: []
  };

  // Search filters
  customerSearchTerm: string = '';
  vehicleSearchTerm: string = '';
  showCustomerDropdown: boolean = false;

  // Modals
  showCustomerModal: boolean = false;
  showVehicleModal: boolean = false;
  isEditMode: boolean = false;

  // Customer Form
  customerForm: Customer = this.getEmptyCustomer();
  emailError: string = '';
  phoneError: string = '';
  nameArManuallyEdited: boolean = false; // Track if user manually edited Arabic name

  // Vehicle Form
  vehicleForm: Vehicle = this.getEmptyVehicle();

  // Loading states
  isLoadingCustomers: boolean = false;
  isLoadingVehicles: boolean = false;
  isSearchingParts: boolean = false;

  // Search debounce
  private searchTimeout: any = null;
  private currentSearchRow: OrderRow | null = null;

  readonly apiUrl = 'http://localhost:5220/api/v1/master/Parts';
  readonly customerOrderApiUrl = 'http://localhost:5220/api/v1/orders/CustomerOrders';
  readonly customerApiUrl = 'http://localhost:5220/api/v1/master/Customers';
  readonly vehicleApiUrl = 'http://localhost:5220/api/v1/VehicleMaster';
  readonly statusApiUrl = 'http://localhost:5220/api/v1/finance/Params';
  
  // Default values for order header (can be made configurable)
  defaultFran: string = 'MAIN';
  defaultBranch: string = 'MAIN';
  defaultWarehouse: string = 'MAIN';
  defaultCordType: string = 'CO'; // Customer Order
  defaultCurrency: string = 'USD';

  constructor(
    private http: HttpClient,
    private customerService: CustomerService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // No longer loading all parts upfront - will search on demand
    this.loadCustomers();
    // Vehicles not needed in customer order page - removed loadVehicles()
    this.loadStatusOptions();
    // Always add initial row
    this.addNewRow();
    
    // Check if editing an existing order from query params
    this.route.queryParams.subscribe(params => {
      if (params['orderNo']) {
        this.loadOrderFromParams(params);
      }
    });
  }

  // Load status options from API
  loadStatusOptions(): void {
    this.isLoadingStatus = true;
    this.http.get<StatusParam[]>(this.statusApiUrl).subscribe({
      next: (data) => {
        this.statusOptions = data || [];
        this.isLoadingStatus = false;
        console.log('Status options loaded:', this.statusOptions.length);
        // Set default status to "Registered" when creating (not editing)
        if (!this.isEditMode && !this.customerOrder.status) {
          // Find "Registered" status or use first available
          const registeredStatus = this.statusOptions.find(s => 
            this.getStatusValue(s).toUpperCase() === 'REGISTERED'
          );
          this.customerOrder.status = registeredStatus 
            ? this.getStatusValue(registeredStatus) 
            : (this.statusOptions.length > 0 ? this.getStatusValue(this.statusOptions[0]) : 'Registered');
        }
      },
      error: (err) => {
        this.isLoadingStatus = false;
        console.error('Error loading status options:', err);
        // Fallback to default statuses if API fails
        this.statusOptions = [
          { PARAMDESC: 'Registered' },
          { PARAMDESC: 'Hold' },
          { PARAMDESC: 'Under Review' },
          { PARAMDESC: 'Confirmed' },
          { PARAMDESC: 'Cancelled' },
          { PARAMDESC: 'Closed' }
        ];
        if (!this.isEditMode && !this.customerOrder.status) {
          this.customerOrder.status = 'Registered';
        }
      }
    });
  }

  // Get status display value
  getStatusValue(status: StatusParam): string {
    return status.PARAMDESC || status.paramDesc || '';
  }

  // Get filtered status options - exclude "Registered" when editing
  getFilteredStatusOptions(): StatusParam[] {
    if (this.isEditMode) {
      // When editing, exclude "Registered" status
      return this.statusOptions.filter(status => {
        const statusValue = this.getStatusValue(status);
        return statusValue !== 'Registered';
      });
    } else {
      // When creating, show all statuses including "Registered"
      return this.statusOptions;
    }
  }

  // Load order from query params
  loadOrderFromParams(params: any): void {
    const fran = params['fran'] || this.defaultFran;
    const branch = params['branch'] || this.defaultBranch;
    const warehouse = params['warehouse'] || this.defaultWarehouse;
    const cordType = params['cordType'] || this.defaultCordType;
    const cordNo = params['orderNo'];

    // Load header
    this.http.get<any>(`${this.customerOrderApiUrl}/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
      next: (header) => {
        const customerCode = header.customer || header.Customer || '';
        // Format dates for display
        const createDt = header.createDt || header.CreateDt;
        const createTm = header.createTm || header.CreateTm;
        const updateDt = header.updateDt || header.UpdateDt;
        const updateTm = header.updateTm || header.UpdateTm;
        
        this.customerOrder = {
          customerCode: customerCode,
          orderDate: header.cordDate || header.CordDate || new Date().toISOString().split('T')[0],
          status: header.status || 'Registered', // Load status from API (saved at creation time)
          fran: fran,
          branch: branch,
          warehouse: warehouse,
          cordType: cordType,
          cordNo: cordNo,
          // Audit fields
          createDt: createDt ? (typeof createDt === 'string' ? createDt : createDt.toString()) : undefined,
          createTm: createTm ? (typeof createTm === 'string' ? createTm : new Date(createTm).toISOString()) : undefined,
          createBy: header.createBy || header.CreateBy || '',
          updateDt: updateDt ? (typeof updateDt === 'string' ? updateDt : updateDt.toString()) : undefined,
          updateTm: updateTm ? (typeof updateTm === 'string' ? updateTm : new Date(updateTm).toISOString()) : undefined,
          updateBy: header.updateBy || header.UpdateBy || ''
        };
        this.isEditMode = true;

        // Load line items
        this.http.get<any[]>(`${this.customerOrderApiUrl}/lines/by-header/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
          next: (lineItems) => {
            if (lineItems && lineItems.length > 0) {
              this.orderRows = lineItems.map((item: any, index: number) => ({
                id: this.nextId++,
                partId: item.part?.toString() || item.Part?.toString() || '',
                partName: '',
                make: item.make || item.Make || '',
                price: item.price || item.Price || 0,
                quantity: item.qty || item.Qty || 0,
                discount: item.discount || item.Discount || 0,
                total: item.totalValue || item.TotalValue || 0,
                partSearchTerm: '',
                showPartDropdown: false
              }));
            } else {
              this.orderRows = [];
              this.addNewRow();
            }
          },
          error: (err) => {
            console.error('Error loading line items:', err);
            this.orderRows = [];
            this.addNewRow();
          }
        });
      },
      error: (err) => {
        console.error('Error loading order header:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Failed to load order for editing',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  ngOnDestroy(): void {
    // Clean up search timeouts
    if (this.customerSearchTimeout) {
      clearTimeout(this.customerSearchTimeout);
    }
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }
  }

  // Search parts by name from API with debouncing
  searchParts(searchTerm: string, row: OrderRow): void {
    // Clear previous timeout
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }

    // If search term is too short, clear results
    if (!searchTerm || searchTerm.trim().length < 2) {
      row.showPartDropdown = false;
      this.allParts = [];
      this.isSearchingParts = false;
      return;
    }

    // Store current row for the search
    this.currentSearchRow = row;

    // Debounce: wait 300ms after user stops typing before searching
    this.searchTimeout = setTimeout(() => {
      this.performSearch(searchTerm.trim(), row);
    }, 300);
  }

  // Perform the actual search API call
  private performSearch(searchTerm: string, row: OrderRow): void {
    if (!searchTerm || searchTerm.trim().length < 2) {
      row.showPartDropdown = false;
      this.allParts = [];
      this.isSearchingParts = false;
      return;
    }

    this.isSearchingParts = true;
    this.isLoading = true;
    const searchUrl = `${this.apiUrl}/search?name=${encodeURIComponent(searchTerm)}`;
    
    this.http.get<Part[]>(searchUrl).subscribe({
      next: (data) => {
        // Only update if this is still the current search row
        if (this.currentSearchRow === row) {
          this.allParts = data || [];
          row.showPartDropdown = this.allParts.length > 0;
          // Update makes list when parts are loaded
          this.updateMakesList();
          console.log(`Found ${this.allParts.length} parts matching "${searchTerm}"`);
          console.log('Available makes:', this.getAllMakes());
        }
        this.isSearchingParts = false;
        this.isLoading = false;
      },
      error: (err) => {
        if (this.currentSearchRow === row) {
          this.allParts = [];
          row.showPartDropdown = false;
        }
        this.isSearchingParts = false;
        this.isLoading = false;
        if (err.status !== 0 && err.status !== 404) {
          console.error('Error searching parts:', err);
          Swal.fire({
            icon: 'error',
            title: 'Search Error',
            text: 'Failed to search parts. Please try again.',
            confirmButtonColor: '#d33',
            timer: 2000
          });
        }
      }
    });
  }

  // Add new row
  addNewRow(): void {
    const newRow: OrderRow = {
      id: this.nextId++,
      partId: '',
      partName: '',
      make: '',
      price: 0,
      quantity: 0,
      discount: 0,
      total: 0,
      partSearchTerm: '',
      showPartDropdown: false
    };
    this.orderRows.push(newRow);
  }

  // Remove row
  removeRow(rowId: number): void {
    this.orderRows = this.orderRows.filter(row => row.id !== rowId);
  }

  // Calculate total for a row
  calculateTotal(row: OrderRow): void {
    const subtotal = (row.price || 0) * (row.quantity || 0);
    const discountAmount = row.discount || 0;
    row.total = Math.max(0, subtotal - discountAmount);
  }

  // When part is selected, auto-fill price and other details
  onPartSelected(row: OrderRow, selectedPart?: Part): void {
    if (row.partId && selectedPart) {
      // Use the provided part object directly
      const part = selectedPart;
      
      // Set part name for display
      row.partName = this.getPartDisplayName(part);
      // Set search term to display name
      row.partSearchTerm = this.getPartDisplayName(part);
      
      // Always auto-fill make from selected part
      // API returns camelCase: "make", but also check PascalCase for compatibility
      const makeValue = part.make || part.Make || (part as any)['make'] || (part as any)['Make'] || '';
      row.make = makeValue;
      
      // Always auto-fill price (FOB) from selected part
      // API returns camelCase: "fob" (from DTO property "Fob")
      const fobValue = part.fob || (part as any)['fob'] || part.FOB || (part as any)['Fob'] || (part as any)['FOB'] || 0;
      row.price = typeof fobValue === 'number' ? fobValue : parseFloat(fobValue) || 0;
      
      console.log('Auto-filled from part:', {
        partObject: part,
        make: row.make,
        price: row.price,
        partMake: part.make,
        partFob: part.fob
      });
      
      this.calculateTotal(row);
    } else if (row.partId) {
      // Fallback: try to find part in allParts array
      const part = this.allParts.find(p => 
        (p.id && p.id.toString() === row.partId) ||
        (p.PART && p.PART === row.partId) ||
        (p.partCode && p.partCode === row.partId) ||
        (p.PartCode && p.PartCode === row.partId)
      );
      
      if (part) {
        // Set part name for display
        row.partName = this.getPartDisplayName(part);
        // Set search term to display name
        row.partSearchTerm = this.getPartDisplayName(part);
        
        // Always auto-fill make from selected part
        // API returns camelCase: "make"
        const makeValue = part.make || part.Make || (part as any)['make'] || (part as any)['Make'] || '';
        row.make = makeValue;
        
        // Always auto-fill price (FOB) from selected part
        // API returns camelCase: "fob" (from DTO property "Fob")
        const fobValue = part.fob || (part as any)['fob'] || part.FOB || (part as any)['Fob'] || (part as any)['FOB'] || 0;
        row.price = typeof fobValue === 'number' ? fobValue : parseFloat(fobValue) || 0;
        
        console.log('Auto-filled from part (fallback):', {
          partObject: part,
          make: row.make,
          price: row.price,
          partMake: part.make,
          partFob: part.fob
        });
        
        this.calculateTotal(row);
      }
    } else {
      // Clear fields if no part selected
      row.partName = '';
      row.make = '';
      row.price = 0;
      this.calculateTotal(row);
    }
  }

  // When make is selected, filter parts dropdown
  onMakeChange(row: OrderRow): void {
    // If make is cleared, reset part selection
    if (!row.make) {
      row.partId = '';
      row.partName = '';
      row.price = 0;
      this.calculateTotal(row);
    } else {
      // If a part is already selected, check if it matches the make
      if (row.partId) {
        const part = this.allParts.find(p => 
          (p.id && p.id.toString() === row.partId) ||
          (p.PART && p.PART === row.partId)
        );
        // If part doesn't match the selected make, clear part selection
        if (part && part.Make !== row.make) {
          row.partId = '';
          row.partName = '';
          row.price = 0;
          this.calculateTotal(row);
        }
      }
    }
  }

  // Get display name for part (for dropdown) - Show Description, not STOCKKEY
  getPartDisplayName(part: Part): string {
    // Prioritize Description over other fields
    if (part.Description && part.Description.trim() !== '') {
      return part.Description;
    }
    if (part.description && part.description.trim() !== '') {
      return part.description;
    }
    // Fallback to PartCode if Description is not available
    return part.PartCode || part.partCode || part.PART || '';
  }

  // Get part ID/key for selection
  getPartId(part: Part): string {
    return part.PartCode || part.partCode || part.PART || part.id?.toString() || '';
  }

  // Get all unique makes from parts (check both camelCase and PascalCase)
  getAllMakes(): string[] {
    // Get makes from allParts (searched parts)
    const makesFromParts = this.allParts
      .map(p => p.make || p.Make || (p as any)['make'] || (p as any)['Make'] || '')
      .filter((make): make is string => !!make && make.trim() !== '');
    
    // Combine with stored makes list and remove duplicates
    const allMakes = [...new Set([...makesFromParts, ...this.allMakesList])]
      .filter((make): make is string => !!make && make.trim() !== '')
      .sort();
    
    return allMakes;
  }
  
  // Update makes list when parts are loaded
  private updateMakesList(): void {
    const makes = this.allParts
      .map(p => p.make || p.Make || (p as any)['make'] || (p as any)['Make'] || '')
      .filter((make): make is string => !!make && make.trim() !== '');
    
    // Add new makes to the list (avoid duplicates)
    makes.forEach(make => {
      if (!this.allMakesList.includes(make)) {
        this.allMakesList.push(make);
      }
    });
    
    // Sort the list
    this.allMakesList.sort();
  }

  // Get filtered parts by make (check both camelCase and PascalCase)
  getFilteredPartsByMake(make?: string): Part[] {
    if (!make || make.trim() === '') {
      return this.allParts;
    }
    return this.allParts.filter(p => {
      const partMake = p.make || p.Make || (p as any)['make'] || (p as any)['Make'] || '';
      return partMake === make;
    });
  }

  // Get filtered parts by search term and make
  getFilteredPartsForRow(row: OrderRow): Part[] {
    // Return the parts from the search API (already filtered)
    return this.allParts;
  }

  // Handle part search input
  onPartSearch(row: OrderRow, event: any): void {
    const searchTerm = event.target.value;
    row.partSearchTerm = searchTerm;
    this.searchParts(searchTerm, row);
  }

  // Select a part from dropdown
  selectPart(row: OrderRow, part: Part): void {
    row.partId = this.getPartId(part);
    row.partSearchTerm = this.getPartDisplayName(part); // This will show Description, not STOCKKEY
    row.showPartDropdown = false;
    
    // Pass the part object directly to auto-fill make and price
    this.onPartSelected(row, part);
    
    // Update makes list to ensure the selected make is available in dropdown
    this.updateMakesList();
  }

  // Create a new part if it doesn't exist
  createPartIfNotExists(row: OrderRow): void {
    const partName = row.partSearchTerm?.trim();
    if (!partName || partName.length < 2) {
      Swal.fire({
        icon: 'warning',
        title: 'Invalid Part Name',
        text: 'Please enter a valid part name (at least 2 characters)',
        confirmButtonColor: '#3085d6'
      });
      return;
    }

    // Check if part already exists
    const existingPart = this.allParts.find(p => 
      (p.PartCode || p.partCode || p.PART || '').toLowerCase() === partName.toLowerCase() ||
      (p.Description || p.description || '').toLowerCase() === partName.toLowerCase()
    );

    if (existingPart) {
      this.selectPart(row, existingPart);
      return;
    }

    // Prompt user to create the part
    Swal.fire({
      title: 'Part Not Found',
      text: `Part "${partName}" does not exist. Do you want to create it?`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, create it!',
      cancelButtonText: 'Cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        // Generate a part code from the name (first 10 characters, uppercase, no spaces)
        const partCode = partName.substring(0, 10).toUpperCase().replace(/\s+/g, '');
        
        const newPart = {
          code: partCode,
          description: partName
        };

        this.http.post<any>(this.apiUrl, newPart).subscribe({
          next: (response) => {
            Swal.fire({
              icon: 'success',
              title: 'Part Created!',
              text: `Part "${partName}" has been created successfully`,
              confirmButtonColor: '#3085d6',
              timer: 1500
            });
            
            // Add the new part to the list and select it
            const createdPart: Part = {
              partCode: response.partCode || response.PartCode || partCode,
              PartCode: response.partCode || response.PartCode || partCode,
              PART: response.partCode || response.PartCode || partCode,
              description: response.description || response.Description || partName,
              Description: response.description || response.Description || partName,
              make: response.make || response.Make || '',
              Make: response.make || response.Make || '',
              fob: 0,
              FOB: 0,
              active: response.active !== undefined ? response.active : (response.Active !== undefined ? response.Active : true),
              Active: response.active !== undefined ? response.active : (response.Active !== undefined ? response.Active : true),
              id: undefined
            };
            
            this.allParts.push(createdPart);
            this.selectPart(row, createdPart);
          },
          error: (err) => {
            console.error('Error creating part:', err);
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: err.error?.message || 'Failed to create part',
              confirmButtonColor: '#d33'
            });
          }
        });
      }
    });
  }

  // Handle part input blur
  onPartInputBlur(row: OrderRow): void {
    // Delay hiding dropdown to allow click on dropdown items
    setTimeout(() => {
      row.showPartDropdown = false;
    }, 200);
  }

  // Handle part input focus
  onPartInputFocus(row: OrderRow): void {
    row.showPartDropdown = true;
  }

  // Download all parts data to Excel
  downloadAllParts(): void {
    if (this.allParts.length === 0) {
      Swal.fire({
        icon: 'warning',
        title: 'No Data',
        text: 'No parts data available to download',
        confirmButtonColor: '#3085d6'
      });
      return;
    }

    try {
      // Prepare data for export
      const exportData = this.allParts.map((part, index) => ({
        'S.No': index + 1,
        'PART': part.PART || '',
        'Make': part.Make || '',
        'FOB': part.FOB || 0
      }));

      // Create worksheet
      const worksheet = XLSX.utils.json_to_sheet(exportData);

      // Set column widths
      const columnWidths = [
        { wch: 8 },   // S.No
        { wch: 30 },  // PART
        { wch: 20 },  // Make
        { wch: 15 }   // FOB
      ];
      worksheet['!cols'] = columnWidths;

      // Create workbook
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, 'All Parts');

      // Generate filename with current date
      const fileName = `All_Parts_Export_${new Date().toISOString().split('T')[0]}.xlsx`;

      // Write file
      XLSX.writeFile(workbook, fileName);

      Swal.fire({
        icon: 'success',
        title: 'Download Successful!',
        text: `Downloaded ${exportData.length} parts to Excel`,
        confirmButtonColor: '#3085d6',
        timer: 2000
      });
    } catch (error) {
      console.error('Export error:', error);
      Swal.fire({
        icon: 'error',
        title: 'Export Failed',
        text: 'Failed to export data to Excel',
        confirmButtonColor: '#d33'
      });
    }
  }


  // Calculate grand total
  getGrandTotal(): number {
    return this.orderRows.reduce((sum, row) => sum + (row.total || 0), 0);
  }

  // ========== CUSTOMER ORDER LOADING ==========


  // Dummy data functions removed - using real APIs
  /*
  loadDummyData(): void {
    this.customerOrders = [
      {
        id: 1,
        orderNumber: 'CO-2024-001',
        customerCode: 'CUST001',
        customerName: 'Alpha Systems',
        vehicleId: 'V001',
        vehiclePlate: 'ABC-1234',
        orderDate: '2024-01-15',
        status: 'COMPLETED',
        totalAmount: 1250.75,
        items: [
          {
            id: 1,
            partId: 'PART001',
            partName: 'Engine Oil Filter - Premium',
            price: 45.50,
            quantity: 2,
            discount: 5.00,
            total: 86.00
          },
          {
            id: 2,
            partId: 'PART002',
            partName: 'Brake Pads - Front Set',
            price: 125.00,
            quantity: 1,
            discount: 10.00,
            total: 115.00
          },
          {
            id: 3,
            partId: 'PART003',
            partName: 'Air Filter - Standard',
            price: 28.75,
            quantity: 3,
            discount: 0.00,
            total: 86.25
          },
          {
            id: 4,
            partId: 'PART004',
            partName: 'Spark Plugs - Iridium',
            price: 89.50,
            quantity: 4,
            discount: 15.00,
            total: 343.00
          },
          {
            id: 5,
            partId: 'PART005',
            partName: 'Transmission Fluid - Synthetic',
            price: 35.25,
            quantity: 5,
            discount: 8.75,
            total: 167.50
          },
          {
            id: 6,
            partId: 'PART006',
            partName: 'Battery - 12V 60Ah',
            price: 145.00,
            quantity: 1,
            discount: 20.00,
            total: 125.00
          },
          {
            id: 7,
            partId: 'PART007',
            partName: 'Windshield Wipers - Pair',
            price: 32.00,
            quantity: 2,
            discount: 4.00,
            total: 60.00
          },
          {
            id: 8,
            partId: 'PART008',
            partName: 'Coolant - Premium',
            price: 18.50,
            quantity: 6,
            discount: 5.00,
            total: 106.00
          }
        ],
        createdAt: '2024-01-15T10:00:00',
        updatedAt: '2024-01-15T14:30:00'
      },
      {
        id: 2,
        orderNumber: 'CO-2024-002',
        customerCode: 'CUST002',
        customerName: 'Omega Corporation',
        vehicleId: 'V002',
        vehiclePlate: 'XYZ-5678',
        orderDate: '2024-01-16',
        status: 'PENDING',
        totalAmount: 875.50,
        items: [
          {
            id: 1,
            partId: 'PART010',
            partName: 'Timing Belt Kit',
            price: 185.00,
            quantity: 1,
            discount: 25.00,
            total: 160.00
          },
          {
            id: 2,
            partId: 'PART011',
            partName: 'Water Pump - OEM',
            price: 95.50,
            quantity: 1,
            discount: 10.00,
            total: 85.50
          },
          {
            id: 3,
            partId: 'PART012',
            partName: 'Thermostat',
            price: 42.00,
            quantity: 1,
            discount: 5.00,
            total: 37.00
          },
          {
            id: 4,
            partId: 'PART013',
            partName: 'Radiator Hose - Upper',
            price: 28.75,
            quantity: 2,
            discount: 3.00,
            total: 54.50
          },
          {
            id: 5,
            partId: 'PART014',
            partName: 'Radiator Hose - Lower',
            price: 32.25,
            quantity: 2,
            discount: 3.50,
            total: 57.50
          },
          {
            id: 6,
            partId: 'PART015',
            partName: 'Serpentine Belt',
            price: 48.00,
            quantity: 1,
            discount: 5.00,
            total: 43.00
          },
          {
            id: 7,
            partId: 'PART016',
            partName: 'Tensioner Pulley',
            price: 65.00,
            quantity: 1,
            discount: 8.00,
            total: 57.00
          },
          {
            id: 8,
            partId: 'PART017',
            partName: 'Idler Pulley',
            price: 52.00,
            quantity: 1,
            discount: 6.00,
            total: 46.00
          },
          {
            id: 9,
            partId: 'PART018',
            partName: 'Coolant - Premium',
            price: 18.50,
            quantity: 8,
            discount: 10.00,
            total: 138.00
          },
          {
            id: 10,
            partId: 'PART019',
            partName: 'Gasket Set - Complete',
            price: 75.00,
            quantity: 1,
            discount: 10.00,
            total: 65.00
          },
          {
            id: 11,
            partId: 'PART020',
            partName: 'Engine Mount - Front',
            price: 125.00,
            quantity: 1,
            discount: 15.00,
            total: 110.00
          }
        ],
        createdAt: '2024-01-16T09:30:00',
        updatedAt: '2024-01-16T11:45:00'
      },
      {
        id: 3,
        orderNumber: 'CO-2024-003',
        customerCode: 'CUST003',
        customerName: 'Lambda Solutions',
        vehicleId: 'V003',
        vehiclePlate: 'DEF-9012',
        orderDate: '2024-01-17',
        status: 'OPEN',
        totalAmount: 2340.25,
        items: [
          {
            id: 1,
            partId: 'PART025',
            partName: 'Shock Absorber - Front Left',
            price: 185.00,
            quantity: 1,
            discount: 20.00,
            total: 165.00
          },
          {
            id: 2,
            partId: 'PART026',
            partName: 'Shock Absorber - Front Right',
            price: 185.00,
            quantity: 1,
            discount: 20.00,
            total: 165.00
          },
          {
            id: 3,
            partId: 'PART027',
            partName: 'Shock Absorber - Rear Left',
            price: 175.00,
            quantity: 1,
            discount: 18.00,
            total: 157.00
          },
          {
            id: 4,
            partId: 'PART028',
            partName: 'Shock Absorber - Rear Right',
            price: 175.00,
            quantity: 1,
            discount: 18.00,
            total: 157.00
          },
          {
            id: 5,
            partId: 'PART029',
            partName: 'Strut Mount - Front',
            price: 65.00,
            quantity: 2,
            discount: 8.00,
            total: 122.00
          },
          {
            id: 6,
            partId: 'PART030',
            partName: 'Strut Mount - Rear',
            price: 58.00,
            quantity: 2,
            discount: 7.00,
            total: 109.00
          },
          {
            id: 7,
            partId: 'PART031',
            partName: 'Control Arm - Lower Front',
            price: 145.00,
            quantity: 2,
            discount: 15.00,
            total: 275.00
          },
          {
            id: 8,
            partId: 'PART032',
            partName: 'Ball Joint - Outer',
            price: 85.00,
            quantity: 2,
            discount: 10.00,
            total: 160.00
          },
          {
            id: 9,
            partId: 'PART033',
            partName: 'Tie Rod End',
            price: 75.00,
            quantity: 2,
            discount: 8.00,
            total: 142.00
          },
          {
            id: 10,
            partId: 'PART034',
            partName: 'Sway Bar Link',
            price: 42.00,
            quantity: 4,
            discount: 5.00,
            total: 163.00
          },
          {
            id: 11,
            partId: 'PART035',
            partName: 'Wheel Bearing - Front',
            price: 125.00,
            quantity: 2,
            discount: 12.00,
            total: 238.00
          },
          {
            id: 12,
            partId: 'PART036',
            partName: 'Wheel Bearing - Rear',
            price: 115.00,
            quantity: 2,
            discount: 11.00,
            total: 224.00
          },
          {
            id: 13,
            partId: 'PART037',
            partName: 'CV Axle - Left',
            price: 195.00,
            quantity: 1,
            discount: 20.00,
            total: 175.00
          },
          {
            id: 14,
            partId: 'PART038',
            partName: 'CV Axle - Right',
            price: 195.00,
            quantity: 1,
            discount: 20.00,
            total: 175.00
          }
        ],
        createdAt: '2024-01-17T08:00:00',
        updatedAt: '2024-01-17T10:15:00'
      },
      {
        id: 4,
        orderNumber: 'CO-2024-004',
        customerCode: 'CUST001',
        customerName: 'Alpha Systems',
        vehicleId: 'V004',
        vehiclePlate: 'GHI-3456',
        orderDate: '2024-01-18',
        status: 'OPEN',
        totalAmount: 645.80,
        items: [
          {
            id: 1,
            partId: 'PART040',
            partName: 'Headlight Bulb - H4',
            price: 28.50,
            quantity: 2,
            discount: 3.00,
            total: 54.00
          },
          {
            id: 2,
            partId: 'PART041',
            partName: 'Tail Light Bulb - 12V',
            price: 8.75,
            quantity: 4,
            discount: 1.00,
            total: 34.00
          },
          {
            id: 3,
            partId: 'PART042',
            partName: 'Turn Signal Bulb - Amber',
            price: 12.00,
            quantity: 4,
            discount: 1.50,
            total: 42.00
          },
          {
            id: 4,
            partId: 'PART043',
            partName: 'Fog Light Bulb',
            price: 35.00,
            quantity: 2,
            discount: 4.00,
            total: 66.00
          },
          {
            id: 5,
            partId: 'PART044',
            partName: 'Headlight Assembly - Left',
            price: 185.00,
            quantity: 1,
            discount: 20.00,
            total: 165.00
          },
          {
            id: 6,
            partId: 'PART045',
            partName: 'Headlight Assembly - Right',
            price: 185.00,
            quantity: 1,
            discount: 20.00,
            total: 165.00
          },
          {
            id: 7,
            partId: 'PART046',
            partName: 'Fuse Box - Main',
            price: 45.00,
            quantity: 1,
            discount: 5.00,
            total: 40.00
          },
          {
            id: 8,
            partId: 'PART047',
            partName: 'Relay - 12V',
            price: 15.50,
            quantity: 5,
            discount: 2.00,
            total: 75.50
          },
          {
            id: 9,
            partId: 'PART048',
            partName: 'Wiring Harness - Engine',
            price: 125.00,
            quantity: 1,
            discount: 15.00,
            total: 110.00
          }
        ],
        createdAt: '2024-01-18T11:15:00',
        updatedAt: '2024-01-18T13:20:00'
      },
      {
        id: 5,
        orderNumber: 'CO-2024-005',
        customerCode: 'CUST004',
        customerName: 'Delta Enterprises',
        vehicleId: 'V005',
        vehiclePlate: 'JKL-7890',
        orderDate: '2024-01-19',
        status: 'CANCELLED',
        totalAmount: 1895.50,
        items: [
          {
            id: 1,
            partId: 'PART050',
            partName: 'AC Compressor - OEM',
            price: 485.00,
            quantity: 1,
            discount: 50.00,
            total: 435.00
          },
          {
            id: 2,
            partId: 'PART051',
            partName: 'AC Condenser',
            price: 285.00,
            quantity: 1,
            discount: 30.00,
            total: 255.00
          },
          {
            id: 3,
            partId: 'PART052',
            partName: 'AC Evaporator',
            price: 195.00,
            quantity: 1,
            discount: 20.00,
            total: 175.00
          },
          {
            id: 4,
            partId: 'PART053',
            partName: 'AC Receiver Dryer',
            price: 65.00,
            quantity: 1,
            discount: 7.00,
            total: 58.00
          },
          {
            id: 5,
            partId: 'PART054',
            partName: 'AC Expansion Valve',
            price: 85.00,
            quantity: 1,
            discount: 9.00,
            total: 76.00
          },
          {
            id: 6,
            partId: 'PART055',
            partName: 'AC Refrigerant - R134a',
            price: 35.00,
            quantity: 4,
            discount: 4.00,
            total: 136.00
          },
          {
            id: 7,
            partId: 'PART056',
            partName: 'AC Belt - Serpentine',
            price: 42.00,
            quantity: 1,
            discount: 4.50,
            total: 37.50
          },
          {
            id: 8,
            partId: 'PART057',
            partName: 'AC Pressure Switch',
            price: 55.00,
            quantity: 1,
            discount: 6.00,
            total: 49.00
          },
          {
            id: 9,
            partId: 'PART058',
            partName: 'AC Blower Motor',
            price: 185.00,
            quantity: 1,
            discount: 20.00,
            total: 165.00
          },
          {
            id: 10,
            partId: 'PART059',
            partName: 'AC Heater Core',
            price: 225.00,
            quantity: 1,
            discount: 25.00,
            total: 200.00
          },
          {
            id: 11,
            partId: 'PART060',
            partName: 'AC Hose Set - Complete',
            price: 145.00,
            quantity: 1,
            discount: 15.00,
            total: 130.00
          },
          {
            id: 12,
            partId: 'PART061',
            partName: 'AC O-Ring Set',
            price: 28.00,
            quantity: 1,
            discount: 3.00,
            total: 25.00
          },
          {
            id: 13,
            partId: 'PART062',
            partName: 'AC Service Kit',
            price: 95.00,
            quantity: 1,
            discount: 10.00,
            total: 85.00
          }
        ],
        createdAt: '2024-01-19T09:00:00',
        updatedAt: '2024-01-19T12:30:00'
      }
    ];
    console.log('Dummy customer orders loaded:', this.customerOrders.length);
  }
  */

  // Load dummy customers for demo - REMOVED
  /*
  loadDummyCustomers(): void {
    this.customers = [
      {
        id: 1,
        customerCode: 'CUST001',
        name: 'Alpha Systems',
        nameAr: 'أنظمة ألفا',
        phone: '1234567890',
        email: 'alpha@example.com',
        address: '123 Main Street, City',
        vatNo: 'VAT001'
      },
      {
        id: 2,
        customerCode: 'CUST002',
        name: 'Omega Corporation',
        nameAr: 'شركة أوميغا',
        phone: '2345678901',
        email: 'omega@example.com',
        address: '456 Business Ave, City',
        vatNo: 'VAT002'
      },
      {
        id: 3,
        customerCode: 'CUST003',
        name: 'Lambda Solutions',
        nameAr: 'حلول لامدا',
        phone: '3456789012',
        email: 'lambda@example.com',
        address: '789 Tech Park, City',
        vatNo: 'VAT003'
      },
      {
        id: 4,
        customerCode: 'CUST004',
        name: 'Delta Enterprises',
        nameAr: 'مؤسسات دلتا',
        phone: '4567890123',
        email: 'delta@example.com',
        address: '321 Industrial Zone, City',
        vatNo: 'VAT004'
      },
      {
        id: 5,
        customerCode: 'CUST005',
        name: 'Gamma Industries',
        nameAr: 'صناعات جاما',
        phone: '5678901234',
        email: 'gamma@example.com',
        address: '654 Commerce Blvd, City',
        vatNo: 'VAT005'
      }
    ];
    console.log('Dummy customers loaded:', this.customers.length);
  }
  */

  // Load dummy vehicles for demo - REMOVED
  /*
  loadDummyVehicles(): void {
    this.vehicles = [
      {
        id: 1,
        vehicleCode: 'V001',
        plateNumber: 'ABC-1234',
        make: 'Toyota',
        model: 'Camry',
        year: 2020,
        color: 'White',
        chassisNumber: 'CH001',
        customerCode: 'CUST001'
      },
      {
        id: 2,
        vehicleCode: 'V002',
        plateNumber: 'XYZ-5678',
        make: 'Honda',
        model: 'Accord',
        year: 2021,
        color: 'Black',
        chassisNumber: 'CH002',
        customerCode: 'CUST002'
      },
      {
        id: 3,
        vehicleCode: 'V003',
        plateNumber: 'DEF-9012',
        make: 'Ford',
        model: 'F-150',
        year: 2019,
        color: 'Blue',
        chassisNumber: 'CH003',
        customerCode: 'CUST003'
      },
      {
        id: 4,
        vehicleCode: 'V004',
        plateNumber: 'GHI-3456',
        make: 'Nissan',
        model: 'Altima',
        year: 2022,
        color: 'Silver',
        chassisNumber: 'CH004',
        customerCode: 'CUST001'
      },
      {
        id: 5,
        vehicleCode: 'V005',
        plateNumber: 'JKL-7890',
        make: 'Chevrolet',
        model: 'Malibu',
        year: 2020,
        color: 'Red',
        chassisNumber: 'CH005',
        customerCode: 'CUST004'
      },
      {
        id: 6,
        vehicleCode: 'V006',
        plateNumber: 'MNO-2468',
        make: 'BMW',
        model: '3 Series',
        year: 2021,
        color: 'Gray',
        chassisNumber: 'CH006',
        customerCode: 'CUST005'
      }
    ];
    console.log('Dummy vehicles loaded:', this.vehicles.length);
  }
  */

  // Reset form for new order
  resetForm(): void {
    this.isEditMode = false;
    this.customerOrder = {
      customerCode: '',
      vehicleId: '',
      orderDate: new Date().toISOString().split('T')[0],
      status: 'Registered', // Default to "Registered" when creating
      items: []
    };
    this.orderRows = [];
    this.addNewRow();
    this.customerSearchTerm = '';
    this.vehicleSearchTerm = '';
    this.showCustomerDropdown = false;
    this.selectedCustomerId = null;
    this.vehicles = [];
  }

  // Save customer order (create or update)
  saveOrder(): void {
    // Validate required fields
    if (!this.customerOrder.customerCode) {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please select a customer',
        confirmButtonColor: '#3085d6'
      });
      return;
    }

    // Filter out empty rows (rows without part selected)
    const validRows = this.orderRows.filter(row => row.partId && row.partId.trim() !== '');
    
    if (validRows.length === 0) {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please add at least one order item with a part selected',
        confirmButtonColor: '#3085d6'
      });
      return;
    }

    // Get order key - use existing if editing, generate new if creating
    const fran = this.customerOrder.fran || this.defaultFran;
    const branch = this.customerOrder.branch || this.defaultBranch;
    const warehouse = this.customerOrder.warehouse || this.defaultWarehouse;
    const cordType = this.customerOrder.cordType || this.defaultCordType;
    
    // Get CORDNO - use existing if editing, get from backend if creating
    let cordNo: string;
    if (this.isEditMode && this.customerOrder.cordNo) {
      cordNo = this.customerOrder.cordNo;
    } else {
      // Get next order number from backend (4-digit auto-generated)
      // Use responseType: 'text' to handle plain string response
      this.http.get(`${this.customerOrderApiUrl}/next-order-number`, { responseType: 'text' }).subscribe({
        next: (response) => {
          // Response is a plain string like "0001" or JSON string like "\"0001\""
          let orderNo: string = response;
          
          // Remove quotes if JSON-wrapped
          orderNo = orderNo.replace(/^["']|["']$/g, '').trim();
          
          if (!orderNo || orderNo.length === 0) {
            throw new Error('Empty order number received from server');
          }
          
          cordNo = orderNo;
          console.log('Generated order number:', cordNo);
          this.proceedWithSave(cordNo, validRows, fran, branch, warehouse, cordType);
        },
        error: (err) => {
          console.error('Error getting next order number:', err);
          console.error('Error status:', err.status);
          console.error('Error message:', err.message);
          console.error('Error body:', err.error);
          
          const errorMessage = err.error?.message || err.error || err.message || 'Unknown error';
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: `Failed to generate order number: ${errorMessage}. Please check the console for details.`,
            confirmButtonColor: '#d33'
          });
        }
      });
      return; // Exit early, proceedWithSave will be called from the callback
    }

    // If editing, proceed directly
    this.proceedWithSave(cordNo, validRows, fran, branch, warehouse, cordType);
  }

  // Helper method to proceed with save after getting order number
  private proceedWithSave(cordNo: string, validRows: OrderRow[], fran: string, branch: string, warehouse: string, cordType: string): void {
    // Get CORDDT from selected date (format: YYYY-MM-DD)
    const orderDate = this.customerOrder.orderDate || new Date().toISOString().split('T')[0];
    const cordDate = orderDate; // Store selected date in CORDDT

    // Calculate values from order items
    // GROSSVALUE = sum of (price * quantity) for all items
    const grossValue = validRows.reduce((sum, row) => sum + ((row.price || 0) * (row.quantity || 0)), 0);
    
    // DISCOUNTVALUE = sum of discounts for all items
    const discountValue = validRows.reduce((sum, row) => sum + (row.discount || 0), 0);
    
    // NETVALUE = GROSSVALUE - DISCOUNTVALUE
    const netValue = grossValue - discountValue;
    
    // VAT calculation (if needed in future)
    const vatValue = 0; // TODO: Calculate VAT if needed
    
    // TOTALVALUE = NETVALUE + VAT (or just NETVALUE if no VAT)
    const totalValue = netValue + vatValue;
    
    // NOOFITEMS = count of line items
    const noOfItems = validRows.length;

    // Prepare header data for CORDHDR
    const headerData: any = {
      fran: this.defaultFran,
      branch: this.defaultBranch,
      warehouse: this.defaultWarehouse,
      cordType: this.defaultCordType,
      cordNo: cordNo, // Auto-generated 4-digit number
      cordDate: cordDate, // CORDDT - selected date
      customer: this.customerOrder.customerCode, // CUSTOMER - customer number
      seqNo: 0, // SEQNO - can be set from sequence service later
      seqPrefix: '', // SEQPREFIX - can be set from sequence service later
      currency: this.defaultCurrency,
      noOfItems: noOfItems, // NOOFITEMS - count of line items
      discountValue: discountValue, // DISCOUNTVALUE - total discount value
      grossValue: grossValue, // GROSSVALUE - sum of (price * quantity)
      netValue: netValue, // NETVALUE - grossValue - discountValue
      vatValue: vatValue, // VATVALUE
      totalValue: totalValue // TOTALVALUE - netValue + vatValue
    };
    
    // Add status only when creating (not when updating)
    if (!this.isEditMode && this.customerOrder.status) {
      headerData.status = this.customerOrder.status;
    }

    // Prepare line items data
    const lineItems = validRows.map((row, index) => {
      // Convert partId to number if it's numeric, otherwise use 0
      const partNumber = this.parsePartNumber(row.partId);
      
      return {
        fran: this.defaultFran,
        branch: this.defaultBranch,
        warehouse: this.defaultWarehouse,
        cordType: this.defaultCordType,
        cordNo: cordNo,
        cordSrl: (index + 1).toString().padStart(3, '0'), // Sequential number: 001, 002, etc.
        cordDate: cordDate,
        make: row.make || '',
        part: partNumber,
        qty: row.quantity || 0,
        accpQty: 0, // Accepted quantity (default 0)
        notAvlQty: 0, // Not available quantity (default 0)
        price: row.price || 0,
        discount: row.discount || 0,
        vatPercentage: 0, // TODO: Get VAT percentage if needed
        vatValue: 0, // TODO: Calculate VAT value
        discountValue: row.discount || 0,
        totalValue: row.total || 0
      };
    });

    if (this.isEditMode) {
      // Update existing order - include status (can be changed when editing)
      const updateHeaderData: any = {
        cordDate: cordDate, // CORDDT
        customer: this.customerOrder.customerCode, // CUSTOMER
        noOfItems: noOfItems, // NOOFITEMS
        discountValue: discountValue, // DISCOUNTVALUE
        grossValue: grossValue, // GROSSVALUE
        netValue: netValue, // NETVALUE
        vatValue: vatValue, // VATVALUE
        totalValue: totalValue // TOTALVALUE
      };
      
      // Include status if it's set
      if (this.customerOrder.status) {
        updateHeaderData.status = this.customerOrder.status;
      }

      // First, update the header
      this.http.put(`${this.customerOrderApiUrl}/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`, updateHeaderData).subscribe({
        next: (headerResponse: any) => {
          console.log('Header updated:', headerResponse);
          
          // Load existing line items to delete them
          this.http.get<any[]>(`${this.customerOrderApiUrl}/lines/by-header/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
            next: (existingLineItems) => {
              // Delete all existing line items
              const deletePromises = (existingLineItems || []).map((item: any) => {
                const cordSrl = item.cordSrl || item.CordSrl || '';
                return this.http.delete(`${this.customerOrderApiUrl}/lines/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}/${cordSrl}`).toPromise();
              });

              Promise.all(deletePromises).then(() => {
                // Then create new line items
                let completedItems = 0;
                let hasError = false;
                
                lineItems.forEach((lineItem, index) => {
                  this.http.post(`${this.customerOrderApiUrl}/lines`, lineItem).subscribe({
                    next: () => {
                      completedItems++;
                      console.log(`Line item ${index + 1} created`);
                      
                      if (completedItems === lineItems.length && !hasError) {
                        Swal.fire({
                          icon: 'success',
                          title: 'Success!',
                          text: `Order ${cordNo} updated successfully with ${lineItems.length} items`,
                          confirmButtonColor: '#3085d6',
                          timer: 2000
                        });
                        // Navigate back to list
                        this.router.navigate(['./customer-order-list']);
                      }
                    },
                    error: (err) => {
                      hasError = true;
                      console.error(`Error creating line item ${index + 1}:`, err);
                      Swal.fire({
                        icon: 'error',
                        title: 'Partial Success',
                        text: `Order header updated but failed to create line item ${index + 1}. Please check and retry.`,
                        confirmButtonColor: '#d33'
                      });
                    }
                  });
                });
              }).catch((err) => {
                console.error('Error deleting existing line items:', err);
                Swal.fire({
                  icon: 'error',
                  title: 'Error',
                  text: 'Failed to delete existing line items',
                  confirmButtonColor: '#d33'
                });
              });
            },
            error: (err) => {
              console.error('Error loading existing line items:', err);
              // Continue anyway - try to create new line items
              let completedItems = 0;
              let hasError = false;
              
              lineItems.forEach((lineItem, index) => {
                this.http.post(`${this.customerOrderApiUrl}/lines`, lineItem).subscribe({
                  next: () => {
                    completedItems++;
                    if (completedItems === lineItems.length && !hasError) {
                      Swal.fire({
                        icon: 'success',
                        title: 'Success!',
                        text: `Order ${cordNo} updated successfully`,
                        confirmButtonColor: '#3085d6',
                        timer: 2000
                      });
                      // Navigate back to list
                      this.router.navigate(['./customer-order-list']);
                    }
                  },
                  error: (err) => {
                    hasError = true;
                    console.error(`Error creating line item ${index + 1}:`, err);
                  }
                });
              });
            }
          });
        },
        error: (err) => {
          console.error('Error updating order header:', err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: err.error?.message || 'Failed to update order header',
            confirmButtonColor: '#d33'
          });
        }
      });
    } else {
      // Create new order
      this.http.post(`${this.customerOrderApiUrl}/headers`, headerData).subscribe({
        next: (headerResponse: any) => {
          console.log('Header created:', headerResponse);
          
          // Then, create all line items sequentially
          let completedItems = 0;
          let hasError = false;
          
          lineItems.forEach((lineItem, index) => {
            this.http.post(`${this.customerOrderApiUrl}/lines`, lineItem).subscribe({
              next: () => {
                completedItems++;
                console.log(`Line item ${index + 1} created`);
                
                // If all items are created, show success
                if (completedItems === lineItems.length && !hasError) {
                  Swal.fire({
                    icon: 'success',
                    title: 'Success!',
                    text: `Order ${cordNo} created successfully with ${lineItems.length} items`,
                    confirmButtonColor: '#3085d6',
                    timer: 2000
                  });
                  // Navigate back to list
                  this.router.navigate(['./customer-order-list']);
                }
              },
              error: (err) => {
                hasError = true;
                console.error(`Error creating line item ${index + 1}:`, err);
                Swal.fire({
                  icon: 'error',
                  title: 'Partial Success',
                  text: `Order header created but failed to create line item ${index + 1}. Please check and retry.`,
                  confirmButtonColor: '#d33'
                });
              }
            });
          });
        },
        error: (err) => {
          console.error('Error creating order header:', err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: err.error?.message || 'Failed to create order header',
            confirmButtonColor: '#d33'
          });
        }
      });
    }
  }

  // Generate order number (max 10 characters: CO + YYMMDD + 2 digits = 10 chars)
  private generateOrderNumber(): string {
    const date = new Date();
    const year = String(date.getFullYear()).substring(2); // Last 2 digits of year (YY)
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const random = Math.floor(Math.random() * 100).toString().padStart(2, '0'); // 2 random digits
    // Format: CO + YYMMDD + 2 digits = CO25122900 (10 characters)
    return `CO${year}${month}${day}${random}`;
  }

  // Parse part number - try to convert to number, return 0 if not numeric
  private parsePartNumber(partId: string): number {
    if (!partId) return 0;
    // Try to extract numeric part
    const numericPart = partId.replace(/[^0-9]/g, '');
    return numericPart ? parseFloat(numericPart) : 0;
  }

  // Navigate back to list
  goBackToList(): void {
    this.router.navigate(['./customer-order-list']);
  }

  // Load a specific order for editing (kept for backward compatibility if needed)
  loadOrderForEdit(order: CustomerOrder): void {
    this.isEditMode = true;
    this.customerOrder = {
      id: order.id,
      orderNumber: order.orderNumber || order['cordNo'] || '',
      customerCode: order.customerCode || order['customer'] || '',
      customerName: order.customerName,
      vehicleId: order.vehicleId || '',
      vehiclePlate: order.vehiclePlate,
      orderDate: order.orderDate || order['cordDate'] || order.createdAt?.split('T')[0] || new Date().toISOString().split('T')[0],
      status: order.status || 'OPEN',
      totalAmount: order.totalAmount || order.totalValue || order.grossValue,
      // Store header key for API calls
      fran: order.fran || order['fran'] || this.defaultFran,
      branch: order.branch || order['branch'] || this.defaultBranch,
      warehouse: order.warehouse || order['warehouse'] || this.defaultWarehouse,
      cordType: order.cordType || order['cordType'] || this.defaultCordType,
      cordNo: order['cordNo'] || order.orderNumber || ''
    };

    // Load line items from API
    const fran = order.fran || order['fran'] || this.defaultFran;
    const branch = order.branch || order['branch'] || this.defaultBranch;
    const warehouse = order.warehouse || order['warehouse'] || this.defaultWarehouse;
    const cordType = order.cordType || order['cordType'] || this.defaultCordType;
    const cordNo = order['cordNo'] || order.orderNumber || '';

    if (cordNo) {
      this.http.get<any[]>(`${this.customerOrderApiUrl}/lines/by-header/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
        next: (lineItems) => {
          if (lineItems && lineItems.length > 0) {
            this.orderRows = lineItems.map((item: any, index: number) => ({
              id: this.nextId++,
              partId: item.part?.toString() || item.Part?.toString() || '',
              partName: '', // Will need to load part name from parts API if needed
              make: item.make || item.Make || '',
              price: item.price || item.Price || 0,
              quantity: item.qty || item.Qty || 0,
              discount: item.discount || item.Discount || 0,
              total: item.totalValue || item.TotalValue || 0,
              partSearchTerm: '', // Will need to load part name
              showPartDropdown: false
            }));
          } else {
            this.orderRows = [];
            this.addNewRow();
          }
        },
        error: (err) => {
          console.error('Error loading line items:', err);
          this.orderRows = [];
          this.addNewRow();
        }
      });
    } else {
      this.orderRows = [];
      this.addNewRow();
    }

    // Scroll to top to show the form
    window.scrollTo({ top: 0, behavior: 'smooth' });

    Swal.fire({
      icon: 'info',
      title: 'Order Loaded',
      text: `Order ${order.orderNumber || order['cordNo'] || order.id} has been loaded for editing`,
      confirmButtonColor: '#3085d6',
      timer: 2000
    });
  }

  // Delete a customer order (removed - now handled in list component)
  deleteOrder(order: CustomerOrder): void {
    const fran = order.fran || order['fran'] || this.defaultFran;
    const branch = order.branch || order['branch'] || this.defaultBranch;
    const warehouse = order.warehouse || order['warehouse'] || this.defaultWarehouse;
    const cordType = order.cordType || order['cordType'] || this.defaultCordType;
    const cordNo = order['cordNo'] || order.orderNumber || '';

    if (!cordNo) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Cannot delete order without order number',
        confirmButtonColor: '#d33'
      });
      return;
    }

    Swal.fire({
      title: 'Are you sure?',
      text: `Do you want to delete order ${cordNo}? This will also delete all line items.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        // First, delete all line items
        this.http.get<any[]>(`${this.customerOrderApiUrl}/lines/by-header/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
          next: (lineItems) => {
            // Delete all line items first
            const deletePromises = (lineItems || []).map((item: any) => {
              const cordSrl = item.cordSrl || item.CordSrl || '';
              return this.http.delete(`${this.customerOrderApiUrl}/lines/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}/${cordSrl}`).toPromise();
            });

            Promise.all(deletePromises).then(() => {
              // Then delete the header
              this.http.delete(`${this.customerOrderApiUrl}/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
                next: () => {
                  Swal.fire({
                    icon: 'success',
                    title: 'Deleted!',
                    text: 'Order and all line items have been deleted',
                    confirmButtonColor: '#3085d6',
                    timer: 2000
                  });
                  // Navigate back to list
                  this.router.navigate(['./customer-order-list']);
                },
                error: (err) => {
                  console.error('Error deleting order header:', err);
                  Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: err.error?.message || 'Failed to delete order header',
                    confirmButtonColor: '#d33'
                  });
                }
              });
            }).catch((err) => {
              console.error('Error deleting line items:', err);
              Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to delete some line items',
                confirmButtonColor: '#d33'
              });
            });
          },
          error: (err) => {
            // If we can't load line items, try to delete header anyway
            console.warn('Could not load line items, deleting header only:', err);
            this.http.delete(`${this.customerOrderApiUrl}/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
              next: () => {
                Swal.fire({
                  icon: 'success',
                  title: 'Deleted!',
                  text: 'Order header has been deleted',
                  confirmButtonColor: '#3085d6',
                  timer: 2000
                });
                // Navigate back to list
                this.router.navigate(['./customer-order-list']);
              },
              error: (deleteErr) => {
                console.error('Error deleting order header:', deleteErr);
                Swal.fire({
                  icon: 'error',
                  title: 'Error',
                  text: deleteErr.error?.message || 'Failed to delete order',
                  confirmButtonColor: '#d33'
                });
              }
            });
          }
        });
      }
    });
  }

  // ========== CUSTOMER AND VEHICLE FUNCTIONS ==========

  // Load all customers (for initial load or when search is empty)
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
          text: 'Failed to load customers from API',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  // Search customers using API
  searchCustomers(searchTerm: string): void {
    if (!searchTerm || searchTerm.trim().length < 1) {
      // If search term is too short, load all customers
      this.loadCustomers();
      return;
    }

    this.isLoadingCustomers = true;
    const searchUrl = `${this.customerApiUrl}/search?name=${encodeURIComponent(searchTerm.trim())}`;
    this.http.get<Customer[]>(searchUrl).subscribe({
      next: (data) => {
        this.customers = data || [];
        this.isLoadingCustomers = false;
        console.log(`Found ${this.customers.length} customers matching "${searchTerm}"`);
      },
      error: (err) => {
        this.isLoadingCustomers = false;
        console.error('Error searching customers:', err);
        // Fallback to client-side filtering if API fails
        this.customers = [];
      }
    });
  }

  // Load all vehicles (optional - not required for customer orders)
  loadVehicles(customerId?: number): void {
    // Vehicles are not needed in customer order page, so fail silently
    this.isLoadingVehicles = false;
    this.vehicles = [];
    this.allVehicles = [];
    // Silently skip vehicle loading - no error messages
    return;
    
    /* Original code - commented out since vehicles not needed
    this.isLoadingVehicles = true;
    this.http.get<Vehicle[]>(this.vehicleApiUrl).subscribe({
      next: (data) => {
        this.allVehicles = data || [];
        // Filter by customer if customer is selected
        if (customerId) {
          this.vehicles = this.allVehicles.filter(v => v.customer === customerId);
        } else {
          this.vehicles = this.allVehicles;
        }
        this.isLoadingVehicles = false;
        console.log('Vehicles loaded:', this.vehicles.length, customerId ? `(filtered by customer ${customerId})` : '(all vehicles)');
      },
      error: (err) => {
        this.isLoadingVehicles = false;
        // Fail silently - no error message
        this.vehicles = [];
        this.allVehicles = [];
      }
    });
    */
  }

  // Handle customer change - vehicles not needed, so just update selected customer ID
  onCustomerChange(): void {
    const selectedCustomer = this.customers.find(c => c.customerCode === this.customerOrder.customerCode);
    if (selectedCustomer && selectedCustomer.id) {
      this.selectedCustomerId = selectedCustomer.id;
      this.customerOrder.vehicleId = ''; // Reset vehicle selection
      // Vehicles not needed - removed loadVehicles call
    } else {
      this.selectedCustomerId = null;
      this.vehicles = [];
      this.customerOrder.vehicleId = '';
    }
  }

  // Filter customers by search term
  getFilteredCustomers(): Customer[] {
    if (!this.customerSearchTerm || this.customerSearchTerm.trim().length < 1) {
      return this.customers.slice(0, 10); // Show first 10 if no search term
    }
    const search = this.customerSearchTerm.toLowerCase().trim();
    return this.customers.filter(c => 
      c.customerCode?.toLowerCase().includes(search) ||
      c.name?.toLowerCase().includes(search) ||
      c.nameAr?.toLowerCase().includes(search) ||
      c.phone?.includes(search) ||
      c.email?.toLowerCase().includes(search)
    ).slice(0, 20); // Limit to 20 results for performance
  }

  // Handle customer search input with debouncing
  private customerSearchTimeout: any = null;
  
  onCustomerSearch(event: any): void {
    const searchTerm = event.target.value;
    this.customerSearchTerm = searchTerm;
    this.showCustomerDropdown = searchTerm.trim().length >= 1;

    // Debounce API calls - wait 300ms after user stops typing
    if (this.customerSearchTimeout) {
      clearTimeout(this.customerSearchTimeout);
    }

    if (searchTerm.trim().length >= 1) {
      this.customerSearchTimeout = setTimeout(() => {
        this.searchCustomers(searchTerm);
      }, 300);
    } else {
      // Load all customers if search is cleared
      this.loadCustomers();
    }
  }

  // Handle customer input focus
  onCustomerInputFocus(): void {
    if (this.customerSearchTerm.trim().length >= 1 || this.customers.length > 0) {
      this.showCustomerDropdown = true;
    }
  }

  // Handle customer input blur
  onCustomerInputBlur(): void {
    // Delay hiding dropdown to allow click on dropdown items
    setTimeout(() => {
      this.showCustomerDropdown = false;
    }, 200);
  }

  // Select a customer from dropdown
  selectCustomer(customer: Customer): void {
    this.customerOrder.customerCode = customer.customerCode;
    this.customerSearchTerm = this.getCustomerDisplayName(customer);
    this.showCustomerDropdown = false;
    this.onCustomerChange();
  }

  // Handle customer dropdown change
  onCustomerDropdownChange(): void {
    if (this.customerOrder.customerCode) {
      const selectedCustomer = this.customers.find(c => c.customerCode === this.customerOrder.customerCode);
      if (selectedCustomer) {
        this.selectedCustomerId = selectedCustomer.id || null;
        this.onCustomerChange();
      }
    } else {
      this.clearCustomerSelection();
    }
  }

  // Get selected customer name
  getSelectedCustomerName(): string {
    const selectedCustomer = this.customers.find(c => c.customerCode === this.customerOrder.customerCode);
    return selectedCustomer ? this.getCustomerDisplayName(selectedCustomer) : '';
  }

  // Clear customer selection
  clearCustomerSelection(): void {
    this.customerOrder.customerCode = '';
    this.customerSearchTerm = '';
    this.selectedCustomerId = null;
    this.vehicles = [];
    this.customerOrder.vehicleId = '';
  }

  // Filter vehicles by search term (already filtered by customer in loadVehicles)
  getFilteredVehicles(): Vehicle[] {
    if (!this.vehicleSearchTerm) {
      return this.vehicles;
    }
    
    const search = this.vehicleSearchTerm.toLowerCase();
    return this.vehicles.filter(v =>
      v.plateNumber?.toLowerCase().includes(search) ||
      v.make?.toLowerCase().includes(search) ||
      v.model?.toLowerCase().includes(search)
    );
  }

  // Get customer display name
  getCustomerDisplayName(customer: Customer): string {
    return customer.name || customer.customerCode || '';
  }

  // Get vehicle display name
  getVehicleDisplayName(vehicle: Vehicle): string {
    const parts = [];
    if (vehicle.plateNumber) parts.push(vehicle.plateNumber);
    if (vehicle.make) parts.push(vehicle.make);
    if (vehicle.model) parts.push(vehicle.model);
    return parts.length > 0 ? parts.join(' - ') : (vehicle.vehicleCode || '');
  }

  // Get vehicle ID for selection
  getVehicleId(vehicle: Vehicle): string {
    return vehicle.id?.toString() || vehicle.vehicleCode || vehicle.plateNumber || '';
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

    this.customerService.create(this.customerForm).subscribe({
      next: (response) => {
        Swal.fire({
          icon: 'success',
          title: 'Success!',
          text: response.message || 'Customer added successfully',
          confirmButtonColor: '#3085d6',
          timer: 1500
        });
        this.closeCustomerModal();
        
        // Reload all customers to ensure we have the latest data (includes the newly created customer with auto-generated code)
        this.loadCustomers();
        
        // Auto-select the newly created customer after a short delay to ensure list is loaded
        // The customerCode will be in the response or we'll get it from the reloaded list
        setTimeout(() => {
          // Try to find the customer by name and email (since code is auto-generated)
          const newCustomer = this.customers.find(c => 
            c.name === this.customerForm.name && 
            c.email === this.customerForm.email
          );
          if (newCustomer) {
            this.customerOrder.customerCode = newCustomer.customerCode;
            this.onCustomerDropdownChange();
          } else if (response.customerCode) {
            // Fallback: use customerCode from response if available
            this.customerOrder.customerCode = response.customerCode;
            this.onCustomerDropdownChange();
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
      vatNo: '',
      createBy: 'SYSTEM',
      createRemarks: 'Created via Customer Order'
    };
  }

  validateCustomerForm(): boolean {
    // Only Name and NameAr are required
    if (!this.customerForm.name || this.customerForm.name.trim() === '') {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please fill in Customer Name',
        confirmButtonColor: '#3085d6'
      });
      return false;
    }
    if (!this.customerForm.nameAr || this.customerForm.nameAr.trim() === '') {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please fill in Customer Name (AR)',
        confirmButtonColor: '#3085d6'
      });
      return false;
    }
    // Email, Phone, Address, VAT No are optional but validate format if provided
    if (this.customerForm.email && this.customerForm.email.trim() !== '' && !this.validateEmail(this.customerForm.email)) {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please enter a valid email address',
        confirmButtonColor: '#3085d6'
      });
      return false;
    }
    return true;
  }

  validateEmail(email: string): boolean {
    if (!email || email.trim() === '') return true; // Optional field
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailPattern.test(email.trim());
  }

  onEmailBlur(): void {
    if (this.customerForm.email && this.customerForm.email.trim() !== '') {
      if (!this.validateEmail(this.customerForm.email)) {
        this.emailError = 'Please enter a valid email address';
      } else {
        this.emailError = '';
      }
    } else {
      this.emailError = ''; // Optional field, no error if empty
    }
  }

  onPhoneBlur(): void {
    if (this.customerForm.phone && this.customerForm.phone.trim() !== '') {
      if (!/^[0-9]+$/.test(this.customerForm.phone.trim())) {
        this.phoneError = 'Phone number should contain only numbers';
      } else {
        this.phoneError = '';
      }
    } else {
      this.phoneError = ''; // Optional field, no error if empty
    }
  }

  // Auto-fill Arabic name when English name changes
  onCustomerNameChange(name: string): void {
    if (!this.nameArManuallyEdited && name) {
      this.customerForm.nameAr = this.toArabic(name);
    }
  }

  // Convert English text to Arabic transliteration
  toArabic(text: string): string {
    const map: { [key: string]: string } = {
      'a': 'ا', 'b': 'ب', 'c': 'ك', 'd': 'د', 'e': 'ي',
      'f': 'ف', 'g': 'ج', 'h': 'ه', 'i': 'ي', 'j': 'ج',
      'k': 'ك', 'l': 'ل', 'm': 'م', 'n': 'ن', 'o': 'و',
      'p': 'ب', 'q': 'ق', 'r': 'ر', 's': 'س', 't': 'ت',
      'u': 'و', 'v': 'ف', 'w': 'و', 'x': 'كس', 'y': 'ي', 'z': 'ز'
    };

    return text
      .toLowerCase()
      .split('')
      .map(c => map[c] || c)
      .join('');
  }

  // ========== VEHICLE MODAL FUNCTIONS ==========

  openVehicleModal(): void {
    this.vehicleForm = this.getEmptyVehicle();
    // Pre-fill customer ID if customer is selected
    if (this.selectedCustomerId) {
      this.vehicleForm.customer = this.selectedCustomerId;
    }
    this.showVehicleModal = true;
  }

  closeVehicleModal(): void {
    this.showVehicleModal = false;
    this.vehicleForm = this.getEmptyVehicle();
  }

  saveVehicle(): void {
    if (!this.validateVehicleForm()) {
      return;
    }

    // If no customer ID, use the selected customer ID
    if (!this.vehicleForm.customer && this.selectedCustomerId) {
      this.vehicleForm.customer = this.selectedCustomerId;
    }

    this.http.post<Vehicle>(this.vehicleApiUrl, this.vehicleForm).subscribe({
      next: (response) => {
        Swal.fire({
          icon: 'success',
          title: 'Success!',
          text: 'Vehicle added successfully',
          confirmButtonColor: '#3085d6',
          timer: 1500
        });
        this.closeVehicleModal();
        // Reload vehicles for the selected customer
        this.loadVehicles(this.selectedCustomerId || undefined);
        // Auto-select the newly created vehicle
        if (response.id) {
          this.customerOrder.vehicleId = response.id.toString();
        } else if (response.vehicleCode) {
          this.customerOrder.vehicleId = response.vehicleCode;
        }
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: err.error?.message || 'Failed to add vehicle',
          confirmButtonColor: '#d33'
        });
      }
    });
  }

  getEmptyVehicle(): Vehicle {
    return {
      plateNumber: '',
      make: '',
      model: '',
      year: new Date().getFullYear(),
      color: '',
      chassisNumber: '',
      customer: undefined
    };
  }

  validateVehicleForm(): boolean {
    if (!this.vehicleForm.plateNumber || this.vehicleForm.plateNumber.trim() === '') {
      Swal.fire({
        icon: 'warning',
        title: 'Validation Error',
        text: 'Please fill in Plate Number',
        confirmButtonColor: '#3085d6'
      });
      return false;
    }
    return true;
  }

  // ========== DETAIL MODAL FUNCTIONS ==========
  // Removed - now handled in list component

  // Get status badge class
  getStatusBadgeClass(status: string): string {
    switch (status?.toUpperCase()) {
      case 'COMPLETED':
        return 'bg-success';
      case 'PENDING':
      case 'OPEN':
        return 'bg-warning';
      case 'CANCELLED':
        return 'bg-danger';
      default:
        return 'bg-info';
    }
  }
}

