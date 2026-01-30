import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PoInquiryComponent } from './po-inquiry.component';

describe('PoInquiryComponent', () => {
  let component: PoInquiryComponent;
  let fixture: ComponentFixture<PoInquiryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PoInquiryComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(PoInquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
