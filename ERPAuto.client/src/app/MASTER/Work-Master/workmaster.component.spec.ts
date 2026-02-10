import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkMasterComponent } from './workmaster.component';

describe('WorkMasterComponent', () => {
  let component: WorkMasterComponent;
  let fixture: ComponentFixture<WorkMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkMasterComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(WorkMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
