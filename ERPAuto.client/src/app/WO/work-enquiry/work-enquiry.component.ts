import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-work-enquiry',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './work-enquiry.component.html',
  styleUrls: ['./work-enquiry.component.css']
})
export class WorkEnquiryComponent implements OnInit, OnDestroy {

  status: string = 'OPEN';

  constructor(private router: Router) { }

  ngOnInit(): void {
    // initialization logic (or leave empty)
  }

  ngOnDestroy(): void {
    // cleanup logic (or leave empty)
  }

  showDummyModal = false;

  openDummyModal(): void {
    this.showDummyModal = true;
  }

  closeDummyModal(): void {
    this.showDummyModal = false;
  }

  // Navigate back to work enquiry list
  goBackToList(): void {
    this.router.navigate(['./work-enquiry-list']);
  }
}
