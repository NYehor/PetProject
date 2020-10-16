import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErrorComponent } from './components/error/error.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ThingListComponent } from './components/thing-list/thing-list.component';


const routes: Routes = [
  { path: '', component: ThingListComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'error', component: ErrorComponent },
  { path: '**', redirectTo: '/error' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
