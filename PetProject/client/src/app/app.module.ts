import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { ThingCardComponent } from './components/thing-card/thing-card.component';
import { ThingListComponent } from './components/thing-list/thing-list.component';
import { UserTabsComponent } from './components/profile-page/user-tabs/user-tabs.component';
import { UserInfoComponent } from './components/profile-page/user-info/user-info.component';
import { RegistrationComponent } from './components/registration/registration.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ThingCardComponent,
    ThingListComponent,
    UserTabsComponent,
    UserInfoComponent,
    RegistrationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
