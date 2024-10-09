import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerUpdateModalComponent } from './customer-update-modal.component';

describe('CustomerUpdateModalComponent', () => {
  let component: CustomerUpdateModalComponent;
  let fixture: ComponentFixture<CustomerUpdateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerUpdateModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CustomerUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
