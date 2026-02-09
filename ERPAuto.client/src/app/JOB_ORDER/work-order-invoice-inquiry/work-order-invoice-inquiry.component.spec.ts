import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkOrderInvoiceInquiryComponent } from './work-order-invoice-inquiry.component';

describe('WorkOrderInvoiceInquiryComponent', () => {
  let component: WorkOrderInvoiceInquiryComponent;
  let fixture: ComponentFixture<WorkOrderInvoiceInquiryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkOrderInvoiceInquiryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkOrderInvoiceInquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
