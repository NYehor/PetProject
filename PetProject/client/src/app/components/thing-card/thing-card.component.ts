import { Component, OnInit } from '@angular/core';
import { Card } from '../../models/card';
@Component({
  selector: 'app-thing-card',
  templateUrl: './thing-card.component.html',
  styleUrls: ['./thing-card.component.css']
})
export class ThingCardComponent implements OnInit {

  card: Card = {
    id: 5,
    title: 'MyTitle'
  };

  constructor() { }

  ngOnInit(): void {
  }

}
