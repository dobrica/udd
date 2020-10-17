import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewScpaperComponent } from './new-scpaper.component';

describe('NewScpaperComponent', () => {
  let component: NewScpaperComponent;
  let fixture: ComponentFixture<NewScpaperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewScpaperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewScpaperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
