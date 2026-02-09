import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplierInvoiceInquiryComponent } from './supplier-invoice-inquiry.component';

describe('SupplierInvoiceInquiryComponent', () => {
  let component: SupplierInvoiceInquiryComponent;
  let fixture: ComponentFixture<SupplierInvoiceInquiryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SupplierInvoiceInquiryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupplierInvoiceInquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
