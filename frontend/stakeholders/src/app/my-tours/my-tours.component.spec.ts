import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyToursComponent } from './my-tours.component';

describe('MyToursComponent', () => {
  let component: MyToursComponent;
  let fixture: ComponentFixture<MyToursComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MyToursComponent]
    });
    fixture = TestBed.createComponent(MyToursComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
