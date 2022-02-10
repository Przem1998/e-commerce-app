import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FailPayComponent } from './fail-pay.component';

describe('FailPayComponent', () => {
  let component: FailPayComponent;
  let fixture: ComponentFixture<FailPayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FailPayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FailPayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
