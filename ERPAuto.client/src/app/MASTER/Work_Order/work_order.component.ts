import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Workshop {
  id: number;
  name: string;
  location: string;
  rating: number;
  reviews: number;
  services: string[];
  price: string;
  description: string;
  color: string;
}

@Component({
  selector: 'app-work-order',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './work_order.component.html',
  styleUrl: './work_order.component.css',
})
export class WorkOrderComponent implements OnInit {
  searchTerm = '';
  selectedLocations: string[] = [];
  selectedServices: string[] = [];
  minRating = 0;

  companies: Workshop[] = [
    {
      id: 1,
      name: 'AutoCare Pro',
      location: 'Downtown',
      rating: 4.8,
      reviews: 324,
      services: ['Oil Change', 'Brake Service', 'Tire Replacement'],
      price: '$$',
      description: 'Premium automotive care with certified technicians',
      color: 'gradient-violet-purple'
    },
    {
      id: 2,
      name: 'SpeedFix Garage',
      location: 'Northside',
      rating: 4.6,
      reviews: 189,
      services: ['Engine Repair', 'AC Repair', 'Oil Change'],
      price: '$$$',
      description: 'Fast and reliable service for all vehicle makes',
      color: 'gradient-blue-cyan'
    },
    {
      id: 3,
      name: 'Elite Motors',
      location: 'Westfield',
      rating: 4.9,
      reviews: 456,
      services: ['Body Work', 'Engine Repair', 'Brake Service'],
      price: '$$$',
      description: 'Luxury vehicle specialists with 20+ years experience',
      color: 'gradient-emerald-teal'
    },
    {
      id: 4,
      name: 'QuickService Auto',
      location: 'Eastside',
      rating: 4.7,
      reviews: 267,
      services: ['Oil Change', 'Tire Replacement', 'AC Repair'],
      price: '$',
      description: 'Affordable quality service with quick turnaround',
      color: 'gradient-orange-red'
    },
    {
      id: 5,
      name: 'Premium Wheels',
      location: 'Southgate',
      rating: 4.5,
      reviews: 198,
      services: ['Tire Replacement', 'Body Work', 'Brake Service'],
      price: '$$',
      description: 'Specialized in wheels and tire services',
      color: 'gradient-pink-rose'
    },
    {
      id: 6,
      name: 'TechDrive Solutions',
      location: 'Downtown',
      rating: 4.9,
      reviews: 542,
      services: ['Engine Repair', 'Oil Change', 'AC Repair'],
      price: '$$$',
      description: 'Advanced diagnostic technology for modern vehicles',
      color: 'gradient-indigo-blue'
    }
  ];

  filteredCompanies: Workshop[] = [];

  locations = ['Downtown', 'Northside', 'Westfield', 'Eastside', 'Southgate'];
  services = ['Oil Change', 'Brake Service', 'Tire Replacement', 'Engine Repair', 'AC Repair', 'Body Work'];

  serviceIcons: { [key: string]: string } = {
    'Oil Change': '🛢️',
    'Brake Service': '🔧',
    'Tire Replacement': '🔄',
    'Engine Repair': '⚙️',
    'AC Repair': '❄️',
    'Body Work': '🎨'
  };

  ngOnInit(): void {
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredCompanies = this.companies.filter(company => {
      const matchesSearch =
        company.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        company.location.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        company.services.some(s => s.toLowerCase().includes(this.searchTerm.toLowerCase()));

      const matchesLocation =
        this.selectedLocations.length === 0 ||
        this.selectedLocations.includes(company.location);

      const matchesService =
        this.selectedServices.length === 0 ||
        this.selectedServices.some(s => company.services.includes(s));

      const matchesRating = company.rating >= this.minRating;

      return matchesSearch && matchesLocation && matchesService && matchesRating;
    });
  }

  toggleLocation(location: string): void {
    if (this.selectedLocations.includes(location)) {
      this.selectedLocations = this.selectedLocations.filter(l => l !== location);
    } else {
      this.selectedLocations.push(location);
    }
    this.applyFilters();
  }

  toggleService(service: string): void {
    if (this.selectedServices.includes(service)) {
      this.selectedServices = this.selectedServices.filter(s => s !== service);
    } else {
      this.selectedServices.push(service);
    }
    this.applyFilters();
  }

  setMinRating(rating: number): void {
    this.minRating = rating;
    this.applyFilters();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedLocations = [];
    this.selectedServices = [];
    this.minRating = 0;
    this.applyFilters();
  }

  getServiceIcon(service: string): string {
    return this.serviceIcons[service] || '🔧';
  }
}
