import { ReturnStatement } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
import { PasswordValidators, UniqEmail, UniqUsername } from './signup.validators';
import { AuthorizeService } from '../auth.service';
import { Observable } from 'rxjs';
import { User } from '../../models/user';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  form: FormGroup;
  document: Document;

  constructor(private formBuilder: FormBuilder, private authService: AuthorizeService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: new FormControl('', [
        Validators.email,
        Validators.required
      ], [
        UniqEmail(this.authService)
        ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8)
      ]),
      confirmPassword: new FormControl('', [
        Validators.required,
      ]),
      username: new FormControl('', [
        Validators.required,
      ], [
          UniqUsername(this.authService)
        ]),
      firstname: new FormControl('', [
        Validators.required
      ]),
      lastname: new FormControl('', [
        Validators.required
      ])
      }, {
      validators: PasswordValidators.mustMatch('password', 'confirmPassword')
    });
  }

  submit(): void {
    console.log('Form created: ', this.form);
    console.log(this.form.value);

    const formData = new FormData();
    formData.append('email', this.form.get('email').value);
    formData.append('username', this.form.get('username').value);
    formData.append('firstname', this.form.get('firstname').value)
    formData.append('lastname', this.form.get('lastname').value)
    formData.append('password', this.form.get('password').value)

    this.authService.uploadForm(formData).subscribe(result => {
      console.log('POST: ', result);
    });

    this.form.reset();
  }
}
