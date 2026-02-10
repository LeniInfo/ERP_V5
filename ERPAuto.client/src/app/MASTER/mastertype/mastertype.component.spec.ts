import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterTypeComponent } from './mastertype.component';

describe('MastertypeComponent', () => {
  let component: MasterTypeComponent;
  let fixture: ComponentFixture<MasterTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
