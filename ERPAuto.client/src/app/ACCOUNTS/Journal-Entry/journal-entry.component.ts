import { Component } from '@angular/core';
import { NgForm, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-journal-entry',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './journal-entry.component.html',
  styleUrls: ['./journal-entry.component.css'],
})
export class JournalEntryComponent {

  journalDate!: string;

  constructor(private apiService: ApiService) {
    this.setTodayDate();
  }

  setTodayDate() {
    const today = new Date();
    this.journalDate = today.toISOString().split('T')[0];
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      const journalData = form.value;

      this.apiService.addjournalEntry(journalData).subscribe({
        next: (res: any) => {
          Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Journal entry submitted successfully!',
          });

          form.resetForm({
            journalDate: this.journalDate
          });
        },
        error: (err: any) => {
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Failed to submit journal entry. Please try again.',
          });
        }
      });
    }
  }
}
