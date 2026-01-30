import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Viscard } from './viscard';

describe('Viscard', () => {
  let component: Viscard;
  let fixture: ComponentFixture<Viscard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Viscard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Viscard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
