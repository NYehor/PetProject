import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { SignupComponent } from './authorization/signup/signup.component';
import { LoginComponent } from './authorization/login/login.component';
import { ConfirmComponent } from './authorization/confirm/confirm.component';
import { LoginMenuComponent } from './authorization/login-menu/login-menu.component';
import { ThingListComponent } from './components/thing-list/thing-list.component';
import { ThingCardComponent } from './components/thing-card/thing-card.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SignupComponent,
    LoginComponent,
    ConfirmComponent,
    LoginMenuComponent,
    ThingCardComponent,
    ThingListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
