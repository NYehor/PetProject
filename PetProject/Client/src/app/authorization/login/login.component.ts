import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup,  FormBuilder, FormControl, Validators } from '@angular/forms';
import { AuthorizeService } from '../auth.service';
import { Router } from '@angular/router';;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @Output() public resultOfAuthenticationEvent = new EventEmitter<boolean>();
  form: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthorizeService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: new FormControl('', [
        Validators.email,
        Validators.required
      ]),
      password: new FormControl('', [
        Validators.required
      ]),
    });
  }

  submit(): void {
    this.authService.login(this.form.get('email').value, this.form.get('password').value)
      .subscribe(result => {
        this.resultOfAuthenticationEvent.emit(result);
        this.router.navigate(['']);
        console.log(result);
      });
  }
}
