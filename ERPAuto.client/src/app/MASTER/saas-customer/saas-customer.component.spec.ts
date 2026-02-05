import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaasCustomerComponent } from './saas-customer.component';

describe('SaasCustomerComponent', () => {
  let component: SaasCustomerComponent;
  let fixture: ComponentFixture<SaasCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaasCustomerComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(SaasCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
