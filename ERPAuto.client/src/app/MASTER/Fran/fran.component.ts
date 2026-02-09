import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, Fran, FranDropdown, Currency } from '../../services/api.service';

interface LoadParam {
  paramValue: string;
  paramDesc: string;
}

@Component({
  selector: 'app-fran',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './fran.component.html',
  styleUrls: ['./fran.component.css'],
})
export class FranComponent implements OnInit {

  // ===== TABLE LIST =====
  franList: any[] = [];
  saasCustomerList: FranDropdown[] = [];
  customerCurrencyList: any[] = [];
  natureOfBusinessList: LoadParam[] = [];

  franchises: any[] = [];
  currencies: Currency[] = [];
  selectedBaseCurrency: string = '';

  

  // ===== FORM MODEL =====
  franObj: Fran = {
    fran: '',
    name: '',
    nameAr: '',
    saasCustomerId: '',
    customerCurrency: '',
    natureOfBusiness: '',
    vatEnabled: false
  };

  isEditMode = false;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadFran();
    this.loadFranDropdown();
    this.loadNatureOfBusiness();
    this.apiService.getFranchises().subscribe(res => {
      this.franchises = res;
    });
    this.loadCurrencies();
  }

  loadCurrencies() {
    this.apiService.getCurrencies().subscribe({
      next: (res) => {
        this.currencies = res;
      },
      error: (err) => {
        console.error('Currency load failed', err);
      }
    });
  }

  loadNatureOfBusiness() {
    this.apiService.getParams('*', 'FRAN_NATUREOFBUSINESS').subscribe({
      next: (res: LoadParam[]) => this.natureOfBusinessList = res,
      error: err => console.error(err)
    });
  }

  loadFranDropdown() {
    this.apiService.getFranDropdown().subscribe({
      next: res => this.saasCustomerList = res,
      error: err => console.error(err)
    });
  }

  

  loadFran() {
    this.apiService.getAllFrans().subscribe({
      next: res => this.franList = res,
      error: err => console.error(err)
    });
  }

  onNatureChange(value: string) {
    const selected = this.natureOfBusinessList.find(
      x => x.paramValue === value
    );

    this.franObj.paramDesc = selected ? selected.paramDesc : '';
  }

  // ===== SAVE / UPDATE =====
  onSubmit() {
    if (
      !this.franObj.fran ||
      !this.franObj.name ||
      !this.franObj.customerCurrency ||
      !this.franObj.natureOfBusiness
    ) {
      alert('Please fill all required fields');
      return;
    }

    console.log('PAYLOAD 👉', this.franObj);

    if (this.isEditMode) {
      this.apiService.updateFran(this.franObj.fran, this.franObj).subscribe({
        next: () => {
          alert('FRAN updated successfully');
          this.loadFran();
          this.resetForm();
        }
      });
    } else {
      this.apiService.createFran(this.franObj).subscribe({
        next: () => {
          alert('FRAN added successfully');
          this.loadFran();
          this.resetForm();
        }
      });
    }
  }

  


  // ===== EDIT =====
  editFran(row: any) {
    this.isEditMode = true;

    this.franObj = {
      fran: row.fran,
      name: row.name,
      nameAr: row.nameAr,
      saasCustomerId: row.saasCustomerId ?? '',
      customerCurrency: row.customerCurrency,
      //paramValue: row.paramValue,
      natureOfBusiness: row.paramValue,
      //paramDesc: row.paramDesc,
      vatEnabled: row.vatEnabled
    };
  }


  // ===== DELETE =====
  deleteFran(franCode: string) {
    if (confirm('Are you sure you want to delete this record?')) {
      this.apiService.deleteFran(franCode).subscribe({
        next: () => {
          alert('FRAN deleted successfully');
          this.loadFran();
        }
      });
    }
  }

  // ===== RESET =====
  resetForm() {
    this.franObj = {
      fran: '',
      name: '',
      nameAr: '',
      //saasCustomerId: '',
      customerCurrency: '',
      //paramValue: '',
      //paramDesc: '',
      natureOfBusiness: '',
      vatEnabled: false
    };
    this.isEditMode = false;
  }
}
