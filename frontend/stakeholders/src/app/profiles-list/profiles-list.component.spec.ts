import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilesListComponent } from './profiles-list.component';

describe('ProfilesListComponent', () => {
  let component: ProfilesListComponent;
  let fixture: ComponentFixture<ProfilesListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProfilesListComponent]
    });
    fixture = TestBed.createComponent(ProfilesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
