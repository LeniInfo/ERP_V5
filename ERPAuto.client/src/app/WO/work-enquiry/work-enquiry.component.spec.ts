import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkEnquiryComponent } from './work-enquiry.component';

describe('WorkEnquiryComponent', () => {
  let component: WorkEnquiryComponent;
  let fixture: ComponentFixture<WorkEnquiryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkEnquiryComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(WorkEnquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
