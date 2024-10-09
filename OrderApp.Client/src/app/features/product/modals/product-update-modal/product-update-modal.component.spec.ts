import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductUpdateModalComponent } from './product-update-modal.component';

describe('ProductUpdateModalComponent', () => {
  let component: ProductUpdateModalComponent;
  let fixture: ComponentFixture<ProductUpdateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductUpdateModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProductUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
