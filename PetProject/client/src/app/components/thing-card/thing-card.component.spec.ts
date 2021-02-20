import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ThingCardComponent } from './thing-card.component';

describe('ThingCardComponent', () => {
  let component: ThingCardComponent;
  let fixture: ComponentFixture<ThingCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ThingCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThingCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
