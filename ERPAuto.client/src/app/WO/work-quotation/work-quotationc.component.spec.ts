import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkQuotationComponent } from './work-quotation.component';

describe('WorkQuotationComponent', () => {
  let component: WorkQuotationComponent;
  let fixture: ComponentFixture<WorkQuotationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkQuotationComponent] // standalone component
    }).compileComponents();

    fixture = TestBed.createComponent(WorkQuotationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
