import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, Fran } from '../../services/api.service';

@Component({
  selector: 'app-fran',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './fran.component.html',
  styleUrls: ['./fran.component.css'],
})
export class FranComponent implements OnInit {

  // List of FRAN records
  franList: Fran[] = [];

  // Form object
  franObj: Fran = {
    franCode: '',
    name: '',
    nameAr: ''
  };

  isEditMode = false;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadFran();
  }

  // ----------- LOAD ALL FRAN -----------
  loadFran() {
    this.apiService.getAllFrans().subscribe({
      next: (res) => this.franList = res,
      error: (err) => console.error('Error loading FRAN data:', err)
    });
  }

  // ----------- CREATE / UPDATE FRAN -----------
  onSubmit() {
    if (!this.franObj.franCode || !this.franObj.name || !this.franObj.nameAr) {
      alert('Please fill all required fields.');
      return;
    }

    if (this.isEditMode) {
      this.apiService.updateFran(this.franObj.franCode, this.franObj).subscribe({
        next: () => {
          alert('FRAN updated successfully.');
          this.loadFran();
          this.resetForm();
        },
        error: (err) => console.error('Error updating FRAN:', err)
      });
    } else {
      this.apiService.createFran(this.franObj).subscribe({
        next: () => {
          alert('FRAN added successfully.');
          this.loadFran();
          this.resetForm();
        },
        error: (err) => console.error('Error creating FRAN:', err)
      });
    }
  }

  // ----------- EDIT FRAN -----------
  // Added by: Nishanth
  editFran(fran: any) {
    this.isEditMode = true;

    this.franObj.franCode = fran.fran; // backend field
    this.franObj.name = fran.name;
    this.franObj.nameAr = fran.nameAr;
  }

  // Added on: 12-12-2025

  // ----------- DELETE FRAN -----------
  deleteFran(franCode: string) {
    if (confirm('Are you sure you want to delete this record?')) {
      this.apiService.deleteFran(franCode).subscribe({
        next: () => {
          alert('FRAN deleted successfully.');
          this.loadFran();
        },
        error: (err) => console.error('Error deleting FRAN:', err)
      });
    }
  }

  // ----------- RESET FORM -----------
  resetForm() {
    this.franObj = {
      franCode: '',
      name: '',
      nameAr: ''
    };
    this.isEditMode = false;
  }
}

