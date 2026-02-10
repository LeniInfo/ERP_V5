import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-master',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css'],
})

export class MasterComponent {
  sidebarExpanded = false;
  activeMenu: string | null = null;
  isAnimating = false;

  fran: string | null = '';
  constructor(private router: Router) { }


  ngOnInit(): void {
    this.fran = sessionStorage.getItem('fran');

    if (!this.fran) {
      this.router.navigate(['/login']);
    }
  }

  toggleMenu(menu: string): void {
    if (this.isAnimating) return; 

    this.isAnimating = true;

    if (this.activeMenu === menu && this.sidebarExpanded) {
      this.sidebarExpanded = false;

      setTimeout(() => {
        this.activeMenu = null;
        this.isAnimating = false;
      }, 310); 
    } else {
      this.activeMenu = menu;
      this.sidebarExpanded = true;

      setTimeout(() => {
        this.isAnimating = false;
      }, 310); 
    }
  }
  onLogout(): void {
    if (confirm('Are you sure you want to logout?')) {
      sessionStorage.clear();
      this.router.navigate(['/login']);
    }  }
}
