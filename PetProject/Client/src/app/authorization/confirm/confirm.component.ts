import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthorizeService } from '../auth.service';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent implements OnInit {

  isConfirmaed: boolean;

  constructor(private activeRoute: ActivatedRoute, private authService: AuthorizeService) {
    this.isConfirmaed = false;
  }

  ngOnInit(): void {
    const email = this.activeRoute.snapshot.queryParams.email;
    const code = this.activeRoute.snapshot.queryParams.code;

    if (email && code) {
      this.authService.confirmEmail(email, code).subscribe(() => this.isConfirmaed = true);
    }
  }

}
