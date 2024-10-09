import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StorageUpdateModalComponent } from './storage-update-modal.component';

describe('StorageUpdateModalComponent', () => {
  let component: StorageUpdateModalComponent;
  let fixture: ComponentFixture<StorageUpdateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StorageUpdateModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StorageUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
