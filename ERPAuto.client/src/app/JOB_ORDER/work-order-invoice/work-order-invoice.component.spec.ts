import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkOrderInvoiceComponent } from './work-order-invoice.component';

describe('WorkOrderInvoiceComponent', () => {
  let component: WorkOrderInvoiceComponent;
  let fixture: ComponentFixture<WorkOrderInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkOrderInvoiceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkOrderInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
