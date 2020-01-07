import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerListAlternateComponent } from './customer-list-alternate.component';

describe('CustomerListAlternateComponent', () => {
  let component: CustomerListAlternateComponent;
  let fixture: ComponentFixture<CustomerListAlternateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerListAlternateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerListAlternateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
