import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateTourComponent } from './create-tour.component';

describe('CreateTourComponent', () => {
  let component: CreateTourComponent;
  let fixture: ComponentFixture<CreateTourComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateTourComponent]
    });
    fixture = TestBed.createComponent(CreateTourComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
