import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { WorkMaster } from './workmaster.model';
import { WorkMasterService } from './workmaster.service';

@Component({
  selector: 'app-workmaster',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './workmaster.component.html',
  styleUrls: ['./workmaster.component.css']
})
export class WorkMasterComponent implements OnInit {

  work: WorkMaster = this.getEmptyWork();

  works: WorkMaster[] = [];
  filteredWorks: WorkMaster[] = [];

  isEditing = false;
  searchText = '';

  constructor(private workService: WorkMasterService) { }

  ngOnInit(): void {
    this.loadWorks();
  }

  // 🔹 Empty model
  getEmptyWork(): WorkMaster {
    return {
      fran: 'A',
      workId:0,
      workType: '',
      name: '',
      unitPrice: 0,
      remarks: ''
    } ;
  }

  // 🔹 Load list
  loadWorks(): void {
    this.workService.getWorks().subscribe({
      next: (data) => {
        console.log(data);
        this.works = data ?? [];
        this.applyFilter();
      },
      error: (err) => {
        alert(err.error?.message || 'Error fetching works');
      }
    });
  }

  // 🔹 Save / Update
  onSubmit(): void {
    if (!this.work.name || !this.work.name.trim()) {
      alert('Work Name is required!');
      return;
    }

    console.log(this.work, 'ON SUBMIT');

    if (this.isEditing) {
      this.workService.updateWork(this.work).subscribe({
        next: () => {
          alert('Work updated successfully');
          this.resetForm();
          this.loadWorks();
        },
        error: (err: any) => {
          console.error(err);
          alert(err.error?.message || 'Error updating work');
        }
      });
    } else {
      this.workService.createWork(this.work).subscribe({
        next: () => {
          alert('Work created successfully');
          this.resetForm();
          this.loadWorks();
        },
        error: (err: any) => {
          console.error(err);
          alert(err.error?.message || 'Error creating work');
        }
      });
    }
  }


  // 🔹 Edit
  onEdit(w: WorkMaster): void {
    this.work = { ...w };
    this.isEditing = true;
  }

  // 🔹 Delete
  onDelete(w: WorkMaster): void {
  if (!w) return;

  this.workService
    .deleteWork(w.fran, w.workType, w.workId)
    .subscribe(() => {
      this.loadWorks();
      this.resetForm();
    });
}


  // 🔹 Reset
  resetForm(): void {
    this.work = this.getEmptyWork();
    this.isEditing = false;
  }

  // 🔹 Search (convert number → string)
  applyFilter(): void {
    const search = this.searchText.toLowerCase().trim();

    this.filteredWorks = this.works.filter(w =>
      w.workId.toString().includes(search) ||
      w.name.toLowerCase().includes(search) ||
      w.workType.toLowerCase().includes(search)
    );
  }

  onClear(): void {
    this.resetForm();
  }
}
