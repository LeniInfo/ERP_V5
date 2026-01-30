import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-viscard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './viscard.html',
  styleUrls: ['./viscard.css']
})
export class ViscardComponent {

  scrollToSection(id: string): void {
    const el = document.getElementById(id);
    if (el) {
      el.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
      });
    }
  }
}
