import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthorizeService } from './../auth.service';
@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})

export class LoginMenuComponent implements OnInit {

  public isAuthenticated: Observable<boolean>;
  constructor(private authorizeService: AuthorizeService) {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
  }

  ngOnInit(): void {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
  }

  logout() {
      this.authorizeService.logout().subscribe();
  }

}
