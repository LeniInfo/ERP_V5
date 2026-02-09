import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GrnCardComponent } from './grn-card.component';

describe('GrnCardComponent', () => {
  let component: GrnCardComponent;
  let fixture: ComponentFixture<GrnCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GrnCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GrnCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
