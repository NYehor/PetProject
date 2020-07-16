import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ThingPageComponent } from './thing-page.component';

describe('ThingPageComponent', () => {
  let component: ThingPageComponent;
  let fixture: ComponentFixture<ThingPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ThingPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
