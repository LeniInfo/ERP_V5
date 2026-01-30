import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { ApiService } from '../../services/api.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'app-part-master.component',
  templateUrl: './part-master.component.html',
  styleUrls: ['./part-master.component.css']
})
export class PartComponent implements OnInit {

  makeList: any[] = [];
  invClassList: any[] = [];
  categoryList: any[] = [];
  groupList: any[] = [];
  cooList: any[] = [];

  fran: string = '';

  partData = {
    partCode: '',
    make: '',
    invClass: '',
    category: '',
    group: '',
    coo: '',
    description: ''
  };

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.fran = sessionStorage.getItem('fran') || '';

    if (!this.fran) {
      Swal.fire('Session Expired', 'Please login again.', 'warning');
      return;
    }

    this.loadMakeList();
    this.loadCategoryList();
    this.loadCooList();
    this.loadInvClassList();
    this.loadGroupList();
  }

  loadMakeList() {
    this.api.getMakeList().subscribe({
      next: data => this.makeList = data,
      error: err => console.error('Error loading makes', err)
    });
  }

  loadCategoryList() {
    this.api.getParams(this.fran, 'PMCATEGORY').subscribe({
      next: data => this.categoryList = data,
      error: err => console.error('Error loading category', err)
    });
  }

  loadCooList() {
    this.api.getParams(this.fran, 'PARTS_COO').subscribe({
      next: data => this.cooList = data,
      error: err => console.error('Error loading COO', err)
    });
  }

  loadInvClassList() {
    this.api.getParams(this.fran, 'INVENTORY_CLASS').subscribe({
      next: data => this.invClassList = data,
      error: err => console.error('Error loading inventory class', err)
    });
  }

  loadGroupList() {
    this.api.getParams(this.fran, 'PARTS_GROUP').subscribe({
      next: data => this.groupList = data,
      error: err => console.error('Error loading group', err)
    });
  }

  onSubmit() {
    const payload = {
      fran: this.fran,
      make: this.partData.make,
      partCode: this.partData.partCode,
      description: this.partData.description,
      invClass: this.partData.invClass,
      category: this.partData.category,
      group: this.partData.group,
      coo: this.partData.coo
    };

    this.api.addPart(payload).subscribe({
      next: () => {
        Swal.fire('Success', 'Part saved successfully.', 'success');
        this.resetForm();
      },
      error: (err) => {
        Swal.fire('Error', err?.error?.message || 'Unable to save part.', 'error');
      }
    });
  }

  resetForm() {
    this.partData = {
      partCode: '',
      make: '',
      invClass: '',
      category: '',
      group: '',
      coo: '',
      description: ''
    };
  }
}
