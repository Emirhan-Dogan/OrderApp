import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StorageModalComponent } from './storage-modal.component';

describe('StorageModalComponent', () => {
  let component: StorageModalComponent;
  let fixture: ComponentFixture<StorageModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StorageModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StorageModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
