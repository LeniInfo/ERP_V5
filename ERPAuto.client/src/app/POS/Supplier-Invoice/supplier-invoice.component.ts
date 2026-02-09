import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SupplierInvoiceService } from './supplier-invoice.service';
import { ApiService, Supplier } from '../../services/api.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';

interface LookupItem {
  code: string;
  name: string;
}

@Component({
  selector: 'app-supplier-invoice',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './supplier-invoice.component.html',
  styleUrls: ['./supplier-invoice.component.css']
})
export class SupplierInvoiceComponent implements OnInit {

  private pendingSupplierCode: string | null = null;

  isEditMode = false;
  supplierReadonly = false;
  sinvtypeReadonly = false;

  isEditing = false;
  editIndex: number | null = null;

  header = {
    fran: 'A',
    brch: 'B1',
    whse: 'WH1',
    sinvno: '',
    sinvtype: '',
    sinvdt: new Date().toISOString().split('T')[0],
    supplier: '',
    currency: 'USD',
    blno: '',
    bldt: new Date().toISOString().split('T')[0],
    eta: '',   
    shippingstatus: '',
    shipCompanyCode: '',
    buyercode: ''
  };

  detail = {
    make: '',
    part: '',
    qty: 0,
    ordqty: 0,
    unitprice: 0,
    discount: 0,
    vatpercentage: 0,
    vatvalue: 0,
    discountvalue: 0,
    totalvalue: 0,
    caseno: '',
    containerno: '',
    sinvtype: '',
    sinvno: '',
    sinvSrl: 0
  };

  details: any[] = [];

  suppliers: LookupItem[] = [];
  filteredSuppliers: LookupItem[] = [];
  showSupplierDropdown = false;
  supplierSearch = '';

  // Part / Make / SINV Type dropdowns
  partSearch = '';
  parts: LookupItem[] = [];
  filteredParts: LookupItem[] = [];
  showPartDropdown = false;

  makeSearch = '';
  makes: LookupItem[] = [];
  filteredMakes: LookupItem[] = [];
  showMakeDropdown = false;

  sinvTypeSearch = '';
  sinvTypes: LookupItem[] = [];
  filteredSinvTypes: LookupItem[] = [];
  showSinvTypeDropdown = false;

  shippingSearch = '';
  shippingList: any[] = [];
  filteredshipping: any[] = [];
  showshippingDropdown = false;
  shippingReadonly = false;

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,
    private invoiceService: SupplierInvoiceService,
    private router: Router,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.loadSuppliers();
    this.loadPartList();
    this.loadMakeList();
    this.loadSinvTypeList();
    this.loadShippingList();

    console.log("Hiting onit");
    this.route.queryParams.subscribe(params => {
      console.log('QUERY PARAMS:', params);  
      const { fran, branch, warehouseCode, sinvType, sinvno, mode } = params;
      if (mode === 'edit' && fran && branch && warehouseCode && sinvType && sinvno) {
        this.isEditMode = true;
        console.log("Load hititng");
        this.loadInvoice(fran, branch, warehouseCode, sinvType, sinvno);
      }
    });
  }

  // ================= LOADERS =================

  loadSuppliers() {
    this.apiService.getSuppliers().subscribe(res => {
      this.suppliers = res.map((s: any) => ({ code: s.supplierCode, name: s.supplierName }));
      this.filteredSuppliers = [...this.suppliers];

      if (this.pendingSupplierCode) {
        const s = this.suppliers.find(x => x.code === this.pendingSupplierCode);
        if (s) this.supplierSearch = `${s.name} (${s.code})`;
      }
    });
  }

  loadPartList() {
    this.http.get<any[]>('https://localhost:7231/api/v1/master/parts')
      .subscribe(data => {
        this.parts = data.map(p => ({ code: p.partCode, name: p.partCode }));
        this.filteredParts = [...this.parts];
      });
  }

  loadMakeList() {
    this.http.get<any[]>('https://localhost:7231/api/v1/master/makes')
      .subscribe(data => {
        this.makes = data.map(m => ({ code: m.code ?? m.makeCode, name: m.name ?? m.Name }));
        this.filteredMakes = [...this.makes];
      });
  }

  loadSinvTypeList() {
    this.http.get<any[]>('https://localhost:7231/api/v1/finance/params')
      .subscribe(data => {
        const filtered = data.filter(p => p.paramType === 'SUPPLIERINVOICE_STATUS');
        this.sinvTypes = filtered.map(p => ({ code: p.code ?? p.paramDesc, name: p.name ?? p.paramDesc }));
        this.filteredSinvTypes = [...this.sinvTypes];
      });
  }

  loadShippingList() {
    this.http.get<any[]>('https://localhost:7231/api/v1/finance/params')
      .subscribe(data => {
        const filtered = data.filter(p => p.paramType === 'SHIPPING_STATUS');
        this.shippingList = filtered.map(p => ({ code: p.code ?? p.paramDesc, name: p.name ?? p.paramDesc }));
        this.filteredshipping = [...this.shippingList];
      });
  }

  //loadInvoice(fran: string, branch: string, warehouseCode: string, sinvType: string, sinvno: string) {
  //  this.invoiceService.getShipment(fran, branch, warehouseCode, sinvType, sinvno)
  //    .subscribe(res => {

  //      console.log(res);
  //      // HEADER
  //      this.header = {
  //        ...this.header,
  //        fran: res.Fran,
  //        brch: res.Branch,
  //        whse: res.WarehouseCode,
  //        sinvno: res.ShipmentNumber,
  //        sinvtype: res.ShipmentType,
  //        supplier: res.SupplierCode,
  //        sinvdt: res.ShipmentDate,
  //        shippingstatus: res.ShippingStatus,
  //        currency: res.Currency || 'USD',
  //        blno: res.BlNumber || '',
  //        bldt: res.BlDate || '',
  //        shipCompanyCode: res.ShipCompanyCode || '',
  //        buyercode: res.BuyerCode || ''
  //      };

  //      // DETAILS
  //      this.details = (res.Details || []).map((l: any, index: number) => ({
  //        make: l.Make,
  //        part: l.Part,
  //        qty: l.Qty,
  //        ordqty: l.OrdQty,
  //        unitprice: l.UnitPrice,
  //        discount: l.Discount,
  //        vatpercentage: l.VatPercentage,
  //        vatvalue: l.VatValue,
  //        discountvalue: l.DiscountValue,
  //        totalvalue: l.TotalValue,
  //        caseno: l.CaseNo,
  //        containerno: l.ContainerNo,
  //        sinvtype: l.ShipmentType,
  //        sinvno: l.ShipmentNumber,
  //        sinvSrl: l.ShipmentSerial
  //      }));

  //      this.supplierSearch = res.SupplierCode;
  //      this.sinvTypeSearch = res.shipmentType;
  //      this.shippingSearch = res.shippingStatus;

  //      // READONLY UI
  //      this.supplierReadonly = true;
  //      this.sinvtypeReadonly = true;

  //      // display selected values in search fields
  //      const sup = this.suppliers.find(x => x.code === res.supplierCode);
  //      if (sup) this.supplierSearch = `${sup.name} (${sup.code})`;
  //      this.sinvTypeSearch = res.shipmentType;

  //    });
  //}

  loadInvoice(
    fran: string,
    branch: string,
    warehouseCode: string,
    sinvType: string,
    sinvno: string
  ) {
    this.invoiceService
      .getShipment(fran, branch, warehouseCode, sinvType, sinvno)
      .subscribe(res => {

        console.log('LOAD RESPONSE:', res);

        /* ================= HEADER ================= */
        this.header = {
          fran: res.fran,
          brch: res.branch,
          whse: res.warehouseCode,
          sinvno: res.shipmentNumber,
          sinvtype: res.shipmentType,
          sinvdt: res.shipmentDate,
          supplier: res.supplierCode,
          currency: res.currency || 'USD',
          blno: res.blNumber || '',
          bldt: res.blDate || '',
          shippingstatus: res.shippingStatus || '',
          shipCompanyCode: res.shipCompanyCode || '',
          buyercode: res.buyerCode || '',
          eta: res.eta
        };

        /* ================= DETAILS ================= */
        this.details = (res.details || []).map((d: any) => ({
          make: d.make,
          part: d.part,
          qty: d.qty,
          ordqty: d.ordQty,
          unitprice: d.unitPrice,
          discount: d.discount,
          discountvalue: d.discountValue,
          vatpercentage: d.vatPercentage,
          vatvalue: d.vatValue,
          totalvalue: d.totalValue,
          caseno: d.caseNo,
          containerno: d.containerNo,
          sinvtype: d.shipmentType,
          sinvno: d.shipmentNumber,
          sinvSrl: d.shipmentSerial
        }));

        /* ================= DISPLAY FIELDS ================= */
        this.supplierSearch = res.supplierCode;
        this.sinvTypeSearch = res.shipmentType;
        this.shippingSearch = res.shippingStatus;

        /* ================= READONLY ================= */
        this.isEditMode = true;
        this.supplierReadonly = true;
        this.sinvtypeReadonly = true;
        this.shippingReadonly = true;
      });
  }


  addDetail() {
    if (!this.detail.part || !this.detail.make || this.detail.qty <= 0) {
      alert('Please fill part, make, and quantity');
      return;
    }
    this.details.push({ ...this.detail });
    this.resetDetail();
  }

  editDetail(index: number) {
    this.isEditing = true;
    this.editIndex = index;
    this.detail = { ...this.details[index] };
    this.partSearch = this.detail.part;
    this.makeSearch = this.detail.make;
  }

  updateDetail() {
    if (this.editIndex === null) return;
    this.details[this.editIndex] = { ...this.detail };
    this.resetDetail();
    this.isEditing = false;
    this.editIndex = null;
  }

  removeDetail(index: number) {
    this.details.splice(index, 1);
  }

  resetDetail() {
    this.detail = {
      make: '',
      part: '',
      qty: 0,
      ordqty: 0,
      unitprice: 0,
      discount: 0,
      vatpercentage: 0,
      vatvalue: 0,
      discountvalue: 0,
      totalvalue: 0,
      caseno: '',
      containerno: '',
      sinvtype: '',
      sinvno: '',
      sinvSrl: 0
    };
    this.partSearch = '';
    this.makeSearch = '';
    this.showPartDropdown = false;
    this.showMakeDropdown = false;
    this.isEditing = false;
    this.editIndex = null;
  }

  // ================= SELECTORS =================

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

  selectSinvType(p: LookupItem) {
    this.header.sinvtype = p.code;
    this.sinvTypeSearch = p.name;
    this.showSinvTypeDropdown = false;
  }

  onSupplierSelect(v: LookupItem) {
    this.header.supplier = v.code;
    this.supplierSearch = `${v.name} (${v.code})`;
    this.showSupplierDropdown = false;
  }

  onClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.supplier-dropdown')) this.showSupplierDropdown = false;
    if (!target.closest('.part-dropdown')) this.showPartDropdown = false;
    if (!target.closest('.make-dropdown')) this.showMakeDropdown = false;
    if (!target.closest('.sinvType-dropdown')) this.showSinvTypeDropdown = false;
    if (!target.closest('.shipping-dropdown')) this.showshippingDropdown = false;
  }

  // ================= SUBMIT =================

  onSubmit() {

    if (!this.header.supplier || !this.header.sinvtype) {
      alert('Please select Supplier and Invoice Type');
      return;
    }

    if (this.details.length === 0) {
      alert('Add at least one detail line');
      return;
    }

    // ================= HEADER =================
    
    const payload = {
      Fran: this.header.fran,
      Branch: this.header.brch,
      WarehouseCode: this.header.whse,
      ShipmentType: this.header.sinvtype,
      ShipmentNumber: this.isEditMode ? this.header.sinvno : '',
      ShipmentDate: this.header.sinvdt,
      SupplierCode: this.header.supplier,
      Currency: this.header.currency,
      BlNumber: this.header.blno,
      BlDate: this.header.bldt,
      BuyerCode: this.header.buyercode,
      ShippingStatus: this.header.shippingstatus,
      ShipCompanyCode: this.header.shipCompanyCode,

      Details: this.details.map((d, i) => ({
        Fran: this.header.fran,
        Branch: this.header.brch,
        WarehouseCode: this.header.whse,
        ShipmentType: this.header.sinvtype,
        ShipmentNumber: this.isEditMode ? this.header.sinvno : '',
        ShipmentDate: this.header.sinvdt,
        ShipmentSerial: i + 1,
        Part: d.part,
        Make: d.make,
        Qty: d.qty,
        OrdQty: d.ordqty,
        UnitPrice: d.unitprice,
        Discount: d.discount,
        DiscountValue: d.discountvalue,
        VatPercentage: d.vatpercentage,
        VatValue: d.vatvalue,
        TotalValue: d.totalvalue,
        CaseNo: d.caseno,
        ContainerNo: d.containerno,
      }))
    };

    if (this.details.length === 0) {
      alert('Add at least one detail line');
      return;
    }


    // ================= CREATE =================
    if (!this.isEditMode) {
      this.invoiceService.createShipment(payload).subscribe({
        next: (res: any) => {
          alert(`Invoice created successfully! SINV No: ${res.shipmentNumber}`);
          this.resetForm();
        },
        error: err => {
          console.error('Create failed', err);
          alert('Failed to create invoice');
        }
      });
    }

    // ================= UPDATE =================
    else {
      this.invoiceService.updateShipment(
        this.header.fran,
        this.header.brch,
        this.header.whse,
        this.header.sinvtype,
        this.header.sinvno!,
        payload
      ).subscribe({
        next: () => {
          alert('Invoice updated successfully');
          this.resetForm();
        },
        error: err => {
          console.error('Update failed', err);
          alert('Failed to update invoice');
        }
      });
    }
  }



  // ================= RESET =================

  resetForm() {
    this.header = {
      fran: 'A',
      brch: 'B1',
      whse: 'WH1',
      sinvno: '',
      sinvtype: '',
      sinvdt: new Date().toISOString().split('T')[0],
      supplier: '',
      currency: 'USD',
      blno: '',
      bldt: new Date().toISOString().split('T')[0],
      eta: '',
      shippingstatus: '',
      shipCompanyCode: '',
      buyercode: ''
    };
    this.supplierSearch = '';
    this.filteredSuppliers = [...this.suppliers];
    this.showSupplierDropdown = false;
    this.resetDetail();
    this.details = [];
    this.isEditMode = false;
    this.isEditing = false;
    this.editIndex = null;
    this.supplierReadonly = false;
    this.sinvtypeReadonly = false;
    this.sinvTypeSearch = '';
  }

  goToInvoiceInquiry() {
    this.router.navigate(['/supplier-invoice-inquiry']);
  }


  onSupplierFocus() {
    this.showSupplierDropdown = true;
    this.filteredSuppliers = [...this.suppliers];
  }

  filterSuppliers() {
    const v = this.supplierSearch.toLowerCase();
    this.filteredSuppliers = this.suppliers.filter(s =>
      s.code.toLowerCase().includes(v) ||
      s.name?.toLowerCase().includes(v)
    );
  }

  selectSupplier(s: any) {
    this.header.supplier = s.code;
    this.supplierSearch = s.name;
    this.showSupplierDropdown = false;
  }

  onsinvTypeFocus() {
    this.showSinvTypeDropdown = true;
    this.filteredSinvTypes = [...this.sinvTypes];
  }

  filtersinvTypes() {
    const v = this.sinvTypeSearch.toLowerCase();
    this.filteredSinvTypes = this.sinvTypes.filter(p =>
      p.code.toLowerCase().includes(v)
    );
  }

 
  onshippingFocus() {
    this.showshippingDropdown = true;
    this.filteredshipping = [...this.shippingList];
  }

  filtershipping() {
    const v = this.shippingSearch.toLowerCase();
    this.filteredshipping = this.shippingList.filter(s =>
      s.code.toLowerCase().includes(v) ||
      s.name?.toLowerCase().includes(v)
    );
  }

  selectshipping(p: any) {
    this.header.shippingstatus = p.code;
    this.shippingSearch = p.name;
    this.showshippingDropdown = false;
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


  calculateDetailTotals(): void
  {
    const gross = (this.detail.qty || 0) * (this.detail.unitprice || 0);
    this.detail.discountvalue = (gross * (this.detail.discount || 0)) / 100;
    const net = gross - this.detail.discountvalue;
    this.detail.vatvalue = (net * (this.detail.vatpercentage || 0)) / 100;
    this.detail.totalvalue = net + this.detail.vatvalue;
  }

}
