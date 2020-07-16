import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ThingCardComponent } from './thing-card/thing-card.component';
import { ThingPageComponent } from './thing-page/thing-page.component';
import { ThingListComponent } from './thing-list/thing-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ThingCardComponent,
    ThingPageComponent,
    ThingListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
