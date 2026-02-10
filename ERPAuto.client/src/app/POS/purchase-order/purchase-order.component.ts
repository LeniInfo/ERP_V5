import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PurchaseOrderService } from './purchase-order.service';
import { ApiService, Supplier } from '../../services/api.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { filter } from 'rxjs';

interface LookupItem {
  code: string;
  name: string;
}

@Component({
  selector: 'app-purchase-order',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './purchase-order.component.html',
  styleUrls: ['./purchase-order.component.css']
})
export class PurchaseOrderComponent implements OnInit {

  private pendingSupplierCode: string | null = null;

  isEditMode = false;
  supplierReadonly = false;
  doctypeReadonly = false;

  isEditing = false;
  editIndex: number | null = null;

  header = {
    franCode: 'A',
    branchCode: 'B1',
    whCode: 'WH1',
    supplierCode: '',
    potype: '',
    pono: '',
    podt: new Date().toISOString().split('T')[0],
  };

  detail = {
    part: '',
    make: '',
    qty: 0,
    unitprice: 0,
    discount: 0,
    vatpercentage: 0,
    totalvalue: 0
  };

  details: any[] = [];

  suppliers: any[] = [];
  filteredSuppliers: any[] = [];
  showDropdown = false;
  searchText = '';

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,
    private poService: PurchaseOrderService,
    private router: Router,
    private http: HttpClient 
  ) { }

  // ---------- PART ----------
  partSearch = '';
  parts: LookupItem[] = [];
  filteredParts: LookupItem[] = [];
  showPartDropdown = false;

  // ---------- MAKE ----------
  makeSearch = '';
  makes: LookupItem[] = [];
  filteredMakes: LookupItem[] = [];
  showMakeDropdown = false;

  // ---------- PO TYPE ----------
  poTypeSearch = '';
  poTypes: LookupItem[] = [];
  filteredPoTypes: LookupItem[] = [];
  showPoTypeDropdown = false;


  ngOnInit(): void {
    this.loadSuppliers();
    this.loadPartList();
    this.loadMakeList();
    this.loadPoTypeList();

    // Check query params for edit mode
    this.route.queryParams.subscribe(params => {
      const { fran, branch, warehouseCode, poType, pono, mode } = params;

      if (mode === 'edit' && fran && branch && warehouseCode && poType && pono) {
        this.isEditMode = true;
        this.loadPO(fran, branch, warehouseCode, poType, pono);
      }
    });
  }

  loadPO(
    fran: string,
    branch: string,
    warehouseCode: string,
    poType: string,
    pono: string
  ) {
    this.poService.getByKey(fran, branch, warehouseCode, poType, pono).subscribe({
      next: res => {
        console.log('PO loaded:', res);

        // HEADER
        this.header = {
          franCode: res.fran,
          branchCode: res.branch,
          whCode: res.warehouseCode,
          supplierCode: res.supplierCode,
          potype: res.poType,
          pono: res.poNumber,
          podt: res.poDate
        };

        this.pendingSupplierCode = res.supplierCode;

        // DETAILS (mapped below)
        this.details = (res.lines || []).map((l: any) => ({
          part: l.partCode,
          make: l.make,
          qty: l.qty,
          unitprice: l.unitPrice,
          discount: l.discount,
          vatpercentage: l.vatPercentage,
          totalvalue: l.totalValue
        }));

        this.poTypeSearch = res.poType;

        // 🔑 Set supplier display text AFTER vendors are loaded
        const supplier = this.suppliers.find(v => v.code === res.supplierCode);
        if (supplier) {
          this.searchText = `${supplier.name} (${supplier.code})`;
        }

        // Lock fields in edit mode
        this.supplierReadonly = true;
        this.doctypeReadonly = true;
      },
      error: err => console.error('Error loading PO:', err)
    });
  }

  onClear(): void {
    this.resetDetail();
  }

  populateForm(po: any) {
    // Not needed if we map directly in loadPO
  }

  loadSuppliers(): void {
    this.apiService.getSuppliers().subscribe({
      next: (res: any[]) => {
        this.suppliers = res.map(v => ({
          code: v.supplierCode,
          name: v.supplierName
        }));
        this.filteredSuppliers = [...this.suppliers];

        if (this.pendingSupplierCode) {
          const supplier = this.suppliers.find(
            v => v.code === this.pendingSupplierCode
          );
          if (supplier) {
            this.searchText = `${supplier.name} (${supplier.code})`;
          }
        }
      },
      error: err => console.error('Supplier load failed', err)
    });
  }

  loadPartList() {
    this.http
      .get<any[]>('https://localhost:7231/api/v1/master/parts')
      .subscribe({
        next: (data) => {
          this.parts = data      
            .map(p => ({
              code: p.partCode,           
              name: p.partCode           
            }));

          this.filteredParts = [...this.parts];
          console.log('Loaded parts:', this.parts);
        },
        error: (err) => console.error('Error loading parts', err)
      });
  }

  loadMakeList() {
    this.http.get<any[]>('https://localhost:7231/api/v1/master/makes').subscribe({
      next: data => {
        this.makes = data.map(m => ({
          code: m.code ?? m.makeCode,
          name: m.name ?? m.Name
        }));
        this.filteredMakes = [...this.makes];
        console.log('Loaded makes:', this.makes);
      },
      error: err => console.error('Error loading makes', err)
    });
  }

  loadPoTypeList() {
    this.http.get<any[]>('https://localhost:7231/api/v1/finance/params').subscribe({
      next: data => {
        // Filter only where paramValue === 'PO_TYPW'
        const filtered = data.filter(p => p.paramType === 'PO_STATUS');

        this.poTypes = filtered.map(p => ({
          code: p.code ?? p.paramDesc,
          name: p.name ?? p.paramDesc
        }));

        this.filteredPoTypes = [...this.poTypes];
        console.log('Loaded PO Types:', this.poTypes);
      },
      error: err => console.error('Error loading PO Types', err)
    });
  }


  onSupplierFocus(): void {
    if (this.supplierReadonly) return;
    this.filteredSuppliers = [...this.suppliers];
    this.showDropdown = true;
  }

  onPoTypeFocus(): void {
    if (this.doctypeReadonly) return;
    this.filteredPoTypes = [...this.poTypes];
    this.showPoTypeDropdown = true;
  }

  filterSuppliers(): void {
    if (this.supplierReadonly) return;
    const text = this.searchText.toLowerCase();
    this.filteredSuppliers = this.suppliers.filter(v =>
      v.name.toLowerCase().includes(text) || v.code.toLowerCase().includes(text)
    );
    this.showDropdown = true;
  }

  filterParts() {
    const val = this.partSearch.toLowerCase();
    this.filteredParts = this.parts.filter(p =>
      p.code.toLowerCase().includes(val)
    );
    this.showPartDropdown = true;
  }

  filterMakes() {
    const val = this.makeSearch.toLowerCase();
    this.filteredMakes = this.makes.filter(m =>
      m.name.toLowerCase().includes(val)
    );
    this.showMakeDropdown = true;
  }

  filterPoTypes() {
    const val = this.poTypeSearch.toLowerCase();
    this.filteredPoTypes = this.poTypes.filter(p =>
      p.name.toLowerCase().includes(val)
    );
    this.showPoTypeDropdown = true;
  }

  // ===================== SELECT =====================

  selectPart(p: LookupItem) {
    this.detail.part = p.code;
    this.partSearch = p.code;
    this.showPartDropdown = false;
  }

  selectMake(m: LookupItem) {
    this.detail.make = m.code;
    this.makeSearch = m.name;
    this.showMakeDropdown = false;
  }

  selectPoType(p: LookupItem) {
    this.header.potype = p.code;
    this.poTypeSearch = p.name;
    this.showPoTypeDropdown = false;
  }

  onSupplierSelect(v: any): void {
    this.header.supplierCode = v.code;
    this.searchText = `${v.name} (${v.code})`;
    this.showDropdown = false;
  }

  onClickOutside(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.supplier-dropdown')) this.showDropdown = false;
    if (!target.closest('.part-dropdown')) this.showPartDropdown = false;
    if (!target.closest('.make-dropdown')) this.showMakeDropdown = false;
    if (!target.closest('.poType-dropdown')) this.showPoTypeDropdown = false;
  }

  updateValue(): void {
    this.detail.totalvalue = (this.detail.qty || 0) * (this.detail.unitprice || 0);
  }

  addDetail(): void {
    if (!this.detail.part || !this.detail.make || this.detail.qty <= 0) {
      alert('Item details required');
      return;
    }
    this.details.push({ ...this.detail });
    this.resetDetail();
  }

  editDetail(index: number): void {
    this.isEditing = true;
    this.editIndex = index;
    this.detail = { ...this.details[index] };

    // Set the search inputs so dropdown shows the selected values
    this.partSearch = this.detail.part; 
    this.makeSearch = this.detail.make;

    // Optionally, show dropdowns if you want them open for editing
    this.showPartDropdown = true;
    this.showMakeDropdown = true;
  }

  updateDetail(): void {
    if (this.editIndex === null) return;
    this.details[this.editIndex] = { ...this.detail };
    this.resetDetail();
    this.isEditing = false;
    this.editIndex = null;
  }

  removeDetail(index: number): void {
    this.details.splice(index, 1);
  }

  resetDetail(): void {
    this.detail = {
      part: '',
      make: '',
      qty: 0,
      unitprice: 0,
      discount: 0,
      vatpercentage: 0,
      totalvalue: 0
    };
    // Reset dropdown input text
    this.partSearch = '';
    this.makeSearch = '';

    // Close dropdowns
    this.showPartDropdown = false;
    this.showMakeDropdown = false;

    this.isEditing = false;
    this.editIndex = null;
  }

  onSubmit(): void {

    if (!this.header.supplierCode || !this.header.potype) {
      alert('Please select Supplier and PO Type');
      return;
    }

    if (this.details.length === 0) {
      alert('Please add at least one item');
      return;
    }

    const dto = {
      Fran: this.header.franCode,
      Branch: this.header.branchCode,
      WarehouseCode: this.header.whCode,
      PoType: this.header.potype,
      PoDate: this.header.podt,
      PoNumber: this.header.pono,   // 🔑 KEEP EXISTING PO NUMBER
      SupplierCode: this.header.supplierCode,
      SupplierRefNo: this.header.supplierCode,
      Currency: 'INR',
      NoOfItems: this.details.length,
      Discount: this.details.reduce((s, d) => s + (d.discount || 0), 0),
      TotalValue: this.details.reduce((s, d) => s + (d.totalvalue || 0), 0),
      Lines: this.details.map((d, index) => ({
        Fran: this.header.franCode,
        Branch: this.header.branchCode,
        WarehouseCode: this.header.whCode,
        PoType: this.header.potype,
        PoNumber: this.header.pono,   // 🔑 VERY IMPORTANT
        PoLineNumber: (index + 1).toString(),
        Posrl: (index + 1).toString(),
        Make: d.make,
        PartCode: d.part,
        Qty: d.qty,
        UnitPrice: d.unitprice,
        Discount: d.discount || 0,
        VatPercentage: d.vatpercentage || 0,
        VatValue: 0,
        DiscountValue: 0,
        TotalValue: d.totalvalue,
        CreatedBy: 'User',
        CreatedOn: new Date().toISOString().split('T')[0],
        PoDate: this.header.podt,
        SupplierCode: this.header.supplierCode
      }))
    };

    // 🔀 SWITCH BASED ON MODE
    if (this.isEditMode) {
      this.updatePO(dto);
    } else {
      this.createPO(dto);
    }
  }

  createPO(dto: any): void {
    this.poService.createPurchaseOrder(dto).subscribe({
      next: () => {
        alert('Purchase Order created successfully');
        this.resetForm();
      },
      error: err => {
        console.error('Create failed', err);
        alert(err?.error?.message || 'Create failed');
      }
    });
  }

  updatePO(dto: any): void {
    this.poService.updatePurchaseOrder(
      this.header.franCode,
      this.header.pono,
      this.header.supplierCode,
      dto
    ).subscribe({
      next: () => {
        alert(`Purchase Order ${this.header.pono} updated successfully`);
      },
      error: err => {
        console.error('Update failed', err);
        alert(err?.error?.message || 'Update failed');
      }
    });
  }



  resetForm(): void {
    this.header = {
      franCode: 'A',
      branchCode: 'B1',
      whCode: 'WH1',
      supplierCode: '',
      potype: '',
      pono: '',
      podt: new Date().toISOString().split('T')[0],
    };
    this.searchText = '';
    this.filteredSuppliers = [...this.suppliers];
    this.showDropdown = false;
    this.resetDetail();
    this.details = [];
    this.isEditMode = false;
    this.isEditing = false;
    this.editIndex = null;
    this.supplierReadonly = false;
    this.doctypeReadonly = false;
  }

  goToPOInquiry(): void {
    this.router.navigate(['/po-inquiry']);
  }
}
