import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-mastertype',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './mastertype.component.html',
  styleUrls: ['./mastertype.component.css']
})


export class MasterTypeComponent implements OnInit {

  apiUrl = 'https://localhost:7231/api/v1/master/MasterTypes';

  /* ================= LIST STATE ================= */
  masterList: any[] = [];
  filteredList: any[] = [];
  searchText = '';

  /* ================= FORM STATE ================= */
  isEditMode = false;
  originalMasterTypeName = '';
  arabicManuallyEdited = false;
  selectedType: string = '';


  //prefix change
  prefixMap: any = {
    COMPETITOR: 'CO',
    LINER: 'LI',
    TRANSPORTER: 'TR',
    CONTRACTOR: 'CT',
    PARTNER: 'PA'
  };

  /* Button name change*/
  typeLabelMap: any = {
    COMPETITOR: 'Competitor',
    LINER: 'Liner',
    TRANSPORTER: 'Transporter',
    CONTRACTOR: 'Contractor',
    PARTNER: 'Partner'
  };

  get typeLabel(): string {
    return this.typeLabelMap[this.selectedType] || 'Master Type';
  }


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) { }

  /* ================= INIT ================= */

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {

      this.selectedType = params['type'] || 'COMPETITOR';

      // Update masterType dynamically
      this.masterType.masterType = this.selectedType;

      // Auto prefix update
      this.masterType.seqPrefix = this.prefixMap[this.selectedType] || '';

      this.loadMasterTypesByType();

      const { fran, masterType, masterCode, mode } = params;

      if (mode === 'edit' && fran && masterType && masterCode) {
        this.isEditMode = true;
        this.loadMasterType(fran, masterType, masterCode);
      } else {
        this.resetForm();
      }
    });
  }

  //empty model
  masterType = {
    fran: 'A',             // default example
    masterType: this.selectedType,        // default example
    masterCode: '001',        // empty, API may generate
    name: '',
    nameAr: '',
    phone: '',
    email: '',
    address: '',
    vatNo: '',
    seqNo: '1',
    seqPrefix: this.prefixMap[this.selectedType] || ''

  };

  /* ================= LOAD LIST ================= */

  loadMasterType(fran: string, masterType: string, masterCode: string): void {

    const url = `${this.apiUrl}/${fran}/${masterType}/${masterCode}`;

    this.http.get<any>(url).subscribe({
      next: (res) => {

        this.masterType = {
          fran: res.fran,
          masterType: res.masterType,
          masterCode: res.masterCode,
          name: res.name,
          nameAr: res.nameAr,
          phone: res.phone,
          email: res.email,
          address: res.address,
          vatNo: res.vatNo,
          seqNo: res.seqNo,
          seqPrefix: res.seqPrefix
        };

        this.originalMasterTypeName = res.name;
        this.isEditMode = true;
      },
      error: (err) => {
        console.error('Error loading MasterType:', err);
        alert('Failed to load Master Type');
      }
    });
  }

  loadMasterTypeList(): void {

    const url = `${this.apiUrl}?masterType=${this.selectedType}`;

    this.http.get<any[]>(url).subscribe({
      next: (res) => {
        this.masterList = res;
        this.filteredList = [...res];
      },
      error: () => alert('Failed to load data')
    });
  }

  loadMasterTypesByType(): void {
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (res) => {

        console.log(this.selectedType);
        this.masterList = res.filter(m =>
          m.masterType === this.selectedType
        );

        this.filteredList = [...this.masterList];
      },
      error: (err) => {
        console.error('Load failed', err);
        alert('Failed to load data');
      }
    });
  }
  /* ================= SEARCH ================= */

  filterData(): void {

    const text = this.searchText.toLowerCase();

    this.filteredList = this.masterList.filter(m =>
      m.masterCode?.toLowerCase().includes(text) ||
      m.name?.toLowerCase().includes(text) ||
      m.email?.toLowerCase().includes(text)
    );
  }
  /* ================= EDIT FROM GRID ================= */

  editMasterType(row: any): void {

    this.masterType = { ...row };

    this.originalMasterTypeName = row.name;
    this.isEditMode = true;
    this.arabicManuallyEdited = false;
  }
  /* ================= DELETE ================= */

  deleteMasterType(row: any): void {

    if (!confirm('Delete this record?')) return;

    const url = `${this.apiUrl}/${row.fran}/${row.masterType}/${row.masterCode}`;

    this.http.delete(url).subscribe({
      next: () => {
        alert('Deleted successfully');
        this.loadMasterTypesByType();   // reload table
        this.resetForm();            // optional
      },
      error: () => alert('Delete failed')
    });
  }
  // ================= SUBMIT =================
  onSubmit(): void {

    if (!this.masterType.name) {
      alert('Name is required');
      return;
    }
    const dto = {
      fran: this.masterType.fran,
      masterType: this.masterType.masterType,
      masterCode: this.masterType.masterCode,   // 🔑 keep existing code in edit mode
      name: this.masterType.name,
      nameAr: this.masterType.nameAr,
      phone: this.masterType.phone,
      email: this.masterType.email,
      address: this.masterType.address,
      vatNo: this.masterType.vatNo,
      seqNo: this.masterType.seqNo,
      seqPrefix: this.masterType.seqPrefix
    };
    if (this.isEditMode) {
      this.updateMasterType(dto);
    } else {
      this.createMasterType(dto);
    }
  }

  clearForm() {
    this.masterType = {
      fran: 'A',
      masterType: this.selectedType,
      masterCode: '001',
      name: '',
      nameAr: '',
      phone: '',
      email: '',
      address: '',
      vatNo: '',
      seqNo: '1',
      seqPrefix: this.prefixMap[this.selectedType] || ''

    };
  }


  // ================= CREATE =================
  createMasterType(dto: any): void {

    this.http.post(this.apiUrl, dto).subscribe({
      next: () => {
        alert('Master Type created successfully');
        this.resetForm();
        this.loadMasterTypesByType();

      },

      error: err => {
        console.error('Create failed', err);
        alert(err?.error?.message || 'Create failed');
      }
    });
  }


  // ================= UPDATE =================
  updateMasterType(dto: any): void {

    const url = `${this.apiUrl}/${this.masterType.fran}/${this.masterType.masterType}/${this.masterType.masterCode}`;

    this.http.put(url, dto).subscribe({
      next: () => {
        alert(`Master Type ${this.masterType.masterCode} updated successfully`);
        this.resetForm();
       this.loadMasterTypesByType();

      },
      error: err => {
        console.error('Update failed', err);
        alert(err?.error?.message || 'Update failed');
      }
    });
  }


  // ================= RESET =================
  resetForm(): void {
    this.masterType = {
      fran: 'A',
      masterType: this.selectedType,
      masterCode: '',
      name: '',
      nameAr: '',
      phone: '',
      email: '',
      address: '',
      vatNo: '',
      seqNo: '1',
      seqPrefix: this.prefixMap[this.selectedType] || ''

    };

    this.isEditMode = false;
    this.arabicManuallyEdited = false;

  }

  onClear(): void {
    this.originalMasterTypeName = '';
    this.arabicManuallyEdited = false;
    this.resetForm();
  }

  /* ================= ARABIC AUTO ================= */
  onMasterTypeNameChange(value: string): void {

   
    if (!value) {
      this.masterType.nameAr = '';
      this.arabicManuallyEdited = false;
      return;
    }

    const nameChanged = value !== this.originalMasterTypeName;

    if ((!this.isEditMode || nameChanged) && !this.arabicManuallyEdited) {
      this.masterType.nameAr = this.toArabic(value);
    }
  }

  toArabic(text: string): string {

    const combos: any = {
      sh: 'ش',
      kh: 'خ',
      th: 'ث',
      dh: 'ذ',
      gh: 'غ'
    };

    const singles: any = {
      a: 'ا',
      b: 'ب',
      c: 'ك',
      d: 'د',
      e: 'ي',
      f: 'ف',
      g: 'ج',
      h: 'ه',
      i: 'ي',
      j: 'ج',
      k: 'ك',
      l: 'ل',
      m: 'م',
      n: 'ن',
      o: 'و',
      p: 'ب',
      q: 'ق',
      r: 'ر',
      s: 'س',
      t: 'ت',
      u: 'و',
      v: 'ف',
      w: 'و',
      x: 'كس',
      y: 'ي',
      z: 'ز'
    };

    text = text.toLowerCase();

    // Replace combos first
    Object.keys(combos).forEach(c => {
      text = text.replaceAll(c, combos[c]);
    });

    // Replace single letters
    return text
      .split('')
      .map(c => singles[c] || c)
      .join('');
  }

  
}
