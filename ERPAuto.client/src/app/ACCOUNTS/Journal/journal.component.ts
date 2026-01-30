import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-journal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.css']
})
export class JournalComponent implements OnInit {

  searchKey: string = '';
  journalType: string = '';
  Description: string = '';
  journalList: any[] = [];
  filteredList: any[] = [];
  paginatedList: any[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalPages: number[] = [];

  constructor(
    private apiService: ApiService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadJournal();
  }

  // ---------- LOAD Journal LIST ----------
  loadJournal(): void {
    this.apiService.getAllJournal().subscribe({
      next: (res: any[]) => {
        this.journalList = res || [];
        this.filteredList = [...this.journalList];
        this.setupPagination();
      },
    });
  }

  // ---------- SEARCH ----------
  onSearch(): void {
    this.filteredList = this.journalList.filter(journal =>
      (!this.searchKey ||
        journal.journalId?.toString().toLowerCase().includes(this.searchKey.toLowerCase())) &&

      (!this.journalType ||
        journal.journalType?.toLowerCase().includes(this.journalType.toLowerCase())) &&

      (!this.Description ||
        journal.description?.toLowerCase().includes(this.Description.toLowerCase()))
    );

    this.currentPage = 1;
    this.setupPagination();
  }

  // ---------- PAGINATION ----------
  setupPagination(): void {
    const totalItems = this.filteredList.length;
    const pageCount = Math.ceil(totalItems / this.itemsPerPage);

    this.totalPages = Array.from({ length: pageCount }, (_, i) => i + 1);
    this.updatePaginatedList();
  }

  updatePaginatedList(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedList = this.filteredList.slice(startIndex, endIndex);
  }

  changePage(page: number): void {
    if (page < 1 || page > this.totalPages.length) {
      return;
    }
    this.currentPage = page;
    this.updatePaginatedList();
  }

  // ---------- NAVIGATION ----------
  goToJournalPage(): void {
    this.router.navigate(['/journal-entry']);
  }
}
