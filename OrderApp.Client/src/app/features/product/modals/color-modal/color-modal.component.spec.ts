import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorModalComponent } from './color-modal.component';

describe('ColorModalComponent', () => {
  let component: ColorModalComponent;
  let fixture: ComponentFixture<ColorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColorModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ColorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
