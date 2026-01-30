import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ApiService } from '../../services/api.service';

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

  suppliers: LookupItem[] = [];
  filteredSuppliers: LookupItem[] = [];
  showDropdown = false;
  searchText = '';

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

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private api: ApiService
  ) { }

  ngOnInit(): void {
    this.loadSuppliers();
    this.loadPartList();
    this.loadMakeList();
    this.loadPoTypeList();

    this.route.queryParams.subscribe(params => {
      const { fran, branch, warehouseCode, poType, pono, mode } = params;

      if (mode === 'edit' && fran && branch && warehouseCode && poType && pono) {
        this.isEditMode = true;
        this.loadPO(fran, branch, warehouseCode, poType, pono);
      }
    });
  }

  // ================= CLICK OUTSIDE =================
  onClickOutside(event: MouseEvent): void {
    this.showDropdown = false;
    this.showPartDropdown = false;
    this.showMakeDropdown = false;
    this.showPoTypeDropdown = false;
  }

  // ================= LOAD DATA =================
  loadSuppliers(): void {
    this.api.getSuppliers().subscribe(res => {
      this.suppliers = res.map(s => ({
        code: s.supplierCode,
        name: s.supplierName
      }));
      this.filteredSuppliers = [...this.suppliers];

      if (this.pendingSupplierCode) {
        const s = this.suppliers.find(v => v.code === this.pendingSupplierCode);
        if (s) this.searchText = `${s.name} (${s.code})`;
      }
    });
  }

  loadPartList(): void {
    this.api.getAllParts().subscribe(res => {
      this.parts = res.map(p => ({
        code: p.partCode,
        name: p.partCode
      }));
      this.filteredParts = [...this.parts];
    });
  }

  loadMakeList(): void {
    this.api.getMakeList().subscribe(res => {
      this.makes = res.map(m => ({
        code: m.makeCode ?? m.code,
        name: m.makeName ?? m.name
      }));
      this.filteredMakes = [...this.makes];
    });
  }

  loadPoTypeList(): void {
    this.api.getParams(this.header.franCode, 'PO_STATUS').subscribe(res => {
      this.poTypes = res.map(p => ({
        code: p.code,
        name: p.paramDesc
      }));
      this.filteredPoTypes = [...this.poTypes];
    });
  }

  loadPO(
    fran: string,
    branch: string,
    warehouseCode: string,
    poType: string,
    pono: string
  ): void {
    this.api.getPurchaseOrderByKey(
      fran, branch, warehouseCode, poType, pono
    ).subscribe(res => {

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
      this.poTypeSearch = res.poType;

      this.details = (res.lines || []).map((l: any) => ({
        part: l.partCode,
        make: l.make,
        qty: l.qty,
        unitprice: l.unitPrice,
        discount: l.discount,
        vatpercentage: l.vatPercentage,
        totalvalue: l.totalValue
      }));

      this.supplierReadonly = true;
      this.doctypeReadonly = true;
    });
  }

  // ================= FILTER / SELECT =================
  onSupplierFocus(): void {
    if (!this.supplierReadonly) {
      this.filteredSuppliers = [...this.suppliers];
      this.showDropdown = true;
    }
  }

  filterSuppliers(): void {
    const t = this.searchText.toLowerCase();
    this.filteredSuppliers = this.suppliers.filter(v =>
      v.name.toLowerCase().includes(t) || v.code.toLowerCase().includes(t)
    );
    this.showDropdown = true;
  }

  onSupplierSelect(v: LookupItem): void {
    this.header.supplierCode = v.code;
    this.searchText = `${v.name} (${v.code})`;
    this.showDropdown = false;
  }

  onPoTypeFocus(): void {
    if (!this.doctypeReadonly) {
      this.filteredPoTypes = [...this.poTypes];
      this.showPoTypeDropdown = true;
    }
  }

  filterPoTypes(): void {
    const v = this.poTypeSearch.toLowerCase();
    this.filteredPoTypes = this.poTypes.filter(p =>
      p.name.toLowerCase().includes(v)
    );
    this.showPoTypeDropdown = true;
  }

  selectPoType(p: LookupItem): void {
    this.header.potype = p.code;
    this.poTypeSearch = p.name;
    this.showPoTypeDropdown = false;
  }

  filterParts(): void {
    const v = this.partSearch.toLowerCase();
    this.filteredParts = this.parts.filter(p =>
      p.code.toLowerCase().includes(v)
    );
    this.showPartDropdown = true;
  }

  selectPart(p: LookupItem): void {
    this.detail.part = p.code;
    this.partSearch = p.code;
    this.showPartDropdown = false;
  }

  filterMakes(): void {
    const v = this.makeSearch.toLowerCase();
    this.filteredMakes = this.makes.filter(m =>
      m.name.toLowerCase().includes(v)
    );
    this.showMakeDropdown = true;
  }

  selectMake(m: LookupItem): void {
    this.detail.make = m.code;
    this.makeSearch = m.name;
    this.showMakeDropdown = false;
  }

  // ================= DETAIL =================
  updateValue(): void {
    this.detail.totalvalue = this.detail.qty * this.detail.unitprice;
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
    const item = this.details[index];
    this.detail = { ...item };
    this.partSearch = item.part;
    this.makeSearch = item.make;
    this.isEditing = true;
    this.editIndex = index;
  }

  updateDetail(): void {
    if (this.editIndex === null) return;
    this.details[this.editIndex] = { ...this.detail };
    this.isEditing = false;
    this.editIndex = null;
    this.resetDetail();
  }

  removeDetail(index: number): void {
    this.details.splice(index, 1);
  }

  onClear(): void {
    this.isEditing = false;
    this.editIndex = null;
    this.resetDetail();
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
    this.partSearch = '';
    this.makeSearch = '';
    this.showPartDropdown = false;
    this.showMakeDropdown = false;
  }

  // ================= SUBMIT =================
  onSubmit(): void {
    if (!this.header.supplierCode || !this.header.potype) {
      alert('Supplier & PO Type required');
      return;
    }

    const dto = {
      header: {
        franCode: this.header.franCode,
        branchCode: this.header.branchCode,
        whCode: this.header.whCode,
        vendorCode: this.header.supplierCode,
        potype: this.header.potype,
        pono: this.header.pono,
        currency: 'INR',
        noofitems: this.details.length,
        discount: 0,
        totalvalue: this.details.reduce((s, d) => s + d.totalvalue, 0),
        createdt: this.header.podt,
        createby: 'User',
        createremarks: ''
      },
      details: this.details.map((d, i) => ({
        franCode: this.header.franCode,
        branchCode: this.header.branchCode,
        whCode: this.header.whCode,
        vendorCode: this.header.supplierCode,
        potype: this.header.potype,
        pono: this.header.pono,
        posrl: (i + 1).toString(),
        make: d.make,
        part: d.part,
        qty: d.qty,
        unitprice: d.unitprice,
        discount: d.discount,
        vatpercentage: d.vatpercentage,
        totalValue: d.totalvalue,
        createdt: this.header.podt,
        createby: 'User'
      }))
    };

    this.isEditMode
      ? this.api.updatePurchaseOrder(
        this.header.franCode,
        this.header.pono,
        this.header.supplierCode,
        dto
      ).subscribe(() => this.router.navigate(['/po-inquiry']))
      : this.api.createPurchaseOrder(dto)
        .subscribe(() => this.router.navigate(['/po-inquiry']));
  }

  goToPOInquiry(): void {
    this.router.navigate(['/po-inquiry']);
  }
}
