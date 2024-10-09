import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserClaimDetailComponent } from './user-claim-detail.component';

describe('UserClaimDetailComponent', () => {
  let component: UserClaimDetailComponent;
  let fixture: ComponentFixture<UserClaimDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserClaimDetailComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserClaimDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
